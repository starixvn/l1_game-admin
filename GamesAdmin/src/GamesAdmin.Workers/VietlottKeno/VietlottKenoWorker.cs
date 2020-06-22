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
using GamesAdmin.Core.Models;
using GamesAdmin.Workers._Shared;
using GamesAdmin.Workers._Shared.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sentry;

namespace GamesAdmin.VietlottKeno
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
                sentryClient.CaptureMessage("Start connecting to vietlott web socket");

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
            bool checkMissedRounds = true;

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

                if (checkMissedRounds)
                {
                    checkMissedRounds = false;
                    await AddMissedRounds(kenoResult.Rounds);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(kenoResult.LiveData) && kenoResult.Rounds.Any())
                    {
                        await AddNewRound(kenoResult.Rounds.FirstOrDefault());
                    }
                }
            }
        }

        private async Task AddNewRound(VietLottKenoRoundResult vietLottKenoRoundResult)
        {
            var createModel = ToCreateModel(vietLottKenoRoundResult);

            var round = await kenoService.GetByRoundIdAsync(createModel.Number);

            if(round.Result == null || round == null)
            {
                var result = await kenoService.AddNewRoundResultAsync(createModel);
                Console.WriteLine($"{DateTime.Now} - Added round {result.Number} to database!");
                _ = Task.Run(() => kenoService.NotifyRoundResult(new[] { result }));
            }
        }

        private async Task AddMissedRounds(IEnumerable<VietLottKenoRoundResult> rounds)
        {
            var result = await kenoService.AddMissedRoundsAsync(rounds.Select(x => ToCreateModel(x)));

            if (result.Any())
            {
                Console.WriteLine($"{DateTime.Now} - Added missing rounds to database!");
                _ = Task.Run(() => kenoService.NotifyRoundResult(result));
            }
        }

        private CreateProVietKenoRound ToCreateModel(VietLottKenoRoundResult kenoRound)
        {
            var result = kenoRound.Data.Split("|").Select(x => byte.Parse(x)).OrderBy(x => x);
            var sum = result.Sum(x => x);

            var entity = new CreateProVietKenoRound
            {
                Time = DateTime.UtcNow,
                Number = long.Parse(kenoRound.DrawId),
                Result = result,
            };

            return entity;
        }
    }

    public class VietLottKenoSocketResult
    {
        public string LiveData { get; set; }

        [JsonPropertyName("kenos")]
        public IEnumerable<VietLottKenoRoundResult> Rounds { get; set; }
    }

    public class VietLottKenoRoundResult
    {
        public string Data { get; set; }

        public string DrawDate { get; set; }

        public string DrawId { get; set; }
    }
}