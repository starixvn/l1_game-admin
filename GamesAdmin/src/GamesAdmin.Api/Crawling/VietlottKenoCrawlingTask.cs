using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;

namespace GamesAdmin.Api.Crawling
{
    public class VietlottKenoCrawlingTask : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load("https://vietlott.vn/vi/trung-thuong/ket-qua-trung-thuong/winning-number-keno");

                var latestRow = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'divRightContent']/div[2]/table/tbody/tr[2]");

                if (latestRow != null)
                {
                    var date = DateTime.ParseExact(latestRow.SelectSingleNode("td[1]/a").InnerText, "dd/MM/yyyy", null);
                    var round = latestRow.SelectSingleNode("td[1]/div/a").InnerText;
                    var results = latestRow.SelectNodes("td[2]/div/span").Select(x => int.Parse(x.InnerText));
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}