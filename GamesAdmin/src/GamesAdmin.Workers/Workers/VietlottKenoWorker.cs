using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Workers._Shared.Configurations;
using GamesAdmin.Workers.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sentry;

namespace GamesAdmin.Workers
{
    public class VietlottKenoWorker : BackgroundService
    {
        private readonly VietlottKenoConfiguration vietlottKenoConfiguration;
        private readonly IKenoService kenoService;
        private readonly ISentryClient sentryClient;

        public VietlottKenoWorker(
            IOptions<VietlottKenoConfiguration> options, 
            IKenoService kenoService,
            ISentryClient sentryClient)
        {
            vietlottKenoConfiguration = options.Value;
            this.kenoService = kenoService;
            this.sentryClient = sentryClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!vietlottKenoConfiguration.Enable)
            {
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                using var socket = new ClientWebSocket();
                try
                {
                    await socket.ConnectAsync(new Uri(vietlottKenoConfiguration.WebSocketUrl), stoppingToken);
                    await Receive(socket, stoppingToken);
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                }
            }
        }

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using var ms = new MemoryStream();
                do
                {
                    result = await socket.ReceiveAsync(buffer, stoppingToken);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                ms.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(ms, Encoding.UTF8);
                var jsonString = await reader.ReadToEndAsync();
                var kenoResult = JsonSerializer.Deserialize<VietLottKenoSocketResult>(jsonString);

                if (!string.IsNullOrWhiteSpace(kenoResult.LiveData) && kenoResult.Rounds.Any())
                {
                    var latestRound = kenoResult.Rounds.FirstOrDefault();
                    await kenoService.AddNewRoundResultAsync(latestRound);

                    Console.WriteLine($"{DateTime.Now} - Added round {latestRound.DrawId} to database!");
                }
            }
        }
    }

    public class VietLottKenoSocketResult
    {
        public string LiveData { get; set; }

        [JsonPropertyName("kenos")]
        public IEnumerable<VietLottKenoRound> Rounds { get; set; }
    }

    public class VietLottKenoRound
    {
        public string Data { get; set; }

        public string DrawDate { get; set; }

        public string DrawId { get; set; }
    }
}