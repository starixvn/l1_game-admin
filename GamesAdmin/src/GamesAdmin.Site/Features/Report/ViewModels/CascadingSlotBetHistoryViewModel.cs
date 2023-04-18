using GamesAdmin.Core;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features._Shared.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CascadingSlotBetHistoryViewModel
    {
        private const string SymbolPrefix = "symbol";
        public CascadingSlotRecordViewModel Round { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplierViewModel> Multipliers { get; set; }

        public bool ConvertToUSD { get; set; }

        public string Game { get; set; }

        public string FolderGame => Game?.ToLowerInvariant();

        public string StaticResourceUrl { get; set; }

        public IEnumerable<CascadingSlotTumbleViewModel> Tumbles => Round.SettlementResult.Tumbles;

        public bool IsMultipliers => Multipliers != null && Multipliers.Any();

        public string FreeSpinsLabel => Round.FreeSpin ? "Free Spin" : string.Empty;

        public string WinLabel => Round.ReachMaxWin ? "(You reached the Max Win) Win" : "Win";

        public decimal TotalAmount => Round.FreeSpin ? 0 : Round.TotalAmount;

        public string GetSymbolUrl(string symbol)
       => ($"{StaticResourceUrl}/{FolderGame}/{SymbolPrefix}-{symbol}.png");

        public bool HasTotalMultiplier => Round.BetWin > 0 && Round.Multiplier > 0;

        private byte[,] RenderedBoard(IEnumerable<byte> board)
        {          
                var result = new byte[CascadingSlotGameConstant.NumberOfRows, CascadingSlotGameConstant.NumberOfColumns];
                ;
                for (var row = 0; row < CascadingSlotGameConstant.NumberOfRows; row++)
                {
                    var boardArray = board.ToArray(); ;
                    for (var column = 0; column < CascadingSlotGameConstant.NumberOfColumns; column++)
                    {
                        result[row, column] = boardArray[row + column * CascadingSlotGameConstant.NumberOfRows];
                    }
                }

                return result;
            
        }

        public string GetTumbles()
        {
            var strBuilder = new TagBuilder("tbody");
            var tumbles = Tumbles.ToList();
            decimal tumbleWin = 0;
            var rowTotalMultiplier = new TagBuilder("tr");
            var rowWin = new TagBuilder("tr");
            for (byte index = 0; index < tumbles.Count; index++)
            {
                var rowspan = tumbles[index].Icons.Keys.Count > 0 ? tumbles[index].Icons.Keys.Count : 1;
                tumbleWin += tumbles[index].Win;
                var renderBoard = RenderedBoard(tumbles[index].Board);
                if (tumbles[index].Icons.Any())
                {
          
                    foreach (var item in tumbles[index].Icons.Keys.Select((key, i) => new { index = i, key }))
                    {
                        var rowTumble = new TagBuilder("tr");
                        if (item.index == 0)
                        {
                            rowTumble.InnerHtml.AppendHtml(GetIndexRowHtml(index, rowspan));
                            rowTumble.InnerHtml.AppendHtml(GetResultBoardHtml(rowspan, renderBoard, tumbles[index].Multipliers));
                            rowTumble.InnerHtml.AppendHtml(GetRowSymbolHtml(item.key));
                            rowTumble.InnerHtml.AppendHtml(GetNumberHtml(tumbles[index].Icons[item.key].Count));
                            rowTumble.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbles[index].Icons[item.key].Win));
                            rowTumble.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbleWin));             
                        }
                        else
                        {
                            rowTumble.InnerHtml.AppendHtml(GetRowSymbolHtml(item.key));
                            rowTumble.InnerHtml.AppendHtml(GetNumberHtml(tumbles[index].Icons[item.key].Count));
                            rowTumble.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbles[index].Icons[item.key].Win));
                        }
                        strBuilder.InnerHtml.AppendHtml(rowTumble.GetHtmlString());
                    }
                }
                else
                {
                    if (Round.FreeSpin)
                    {
                        var rowFreespin = new TagBuilder("tr");
                        var additionalFreeSpins = Round.SettlementResult.FreeSpins;
                        if (additionalFreeSpins != null && additionalFreeSpins.Odds > 0)
                        {
                            rowFreespin.InnerHtml.AppendHtml(GetIndexRowHtml(index, rowspan));
                            rowFreespin.InnerHtml.AppendHtml(GetResultBoardHtml(rowspan, renderBoard, tumbles[index].Multipliers));
                            rowFreespin.InnerHtml.AppendHtml(GetRowSymbolHtml(CascadingSlotGameConstant.FreeSpinSymbol));
                            rowFreespin.InnerHtml.AppendHtml(GetNumberHtml(additionalFreeSpins.Count));
                            rowFreespin.InnerHtml.AppendHtml(GetFreeSpinsHtml(additionalFreeSpins.Odds, "Extra Free Spins"));
                            rowFreespin.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbleWin));
                        }
                        else
                        {
                            rowFreespin.InnerHtml.AppendHtml(GetIndexRowHtml(index, rowspan));
                            rowFreespin.InnerHtml.AppendHtml(GetResultBoardHtml(rowspan, renderBoard, tumbles[index].Multipliers));
                            rowFreespin.InnerHtml.AppendHtml(GetCellWithColSpanHtml(2, "-"));
                            rowFreespin.InnerHtml.AppendHtml(GetNumberHtml(0));
                            rowFreespin.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbleWin));                     
                        }
                        strBuilder.InnerHtml.AppendHtml(rowFreespin.GetHtmlString());
                    }
                    else
                    {
                        var freeSpins = Round.SettlementResult.FreeSpins;
                        var rowMultiplier = new TagBuilder("tr");
                        if (freeSpins.Odds > 0)
                        {
                            rowMultiplier.InnerHtml.AppendHtml(GetIndexRowHtml(index, rowspan));
                            rowMultiplier.InnerHtml.AppendHtml(GetResultBoardHtml(rowspan, renderBoard, tumbles[index].Multipliers));
                            rowMultiplier.InnerHtml.AppendHtml(GetRowSymbolHtml(CascadingSlotGameConstant.FreeSpinSymbol));
                            rowMultiplier.InnerHtml.AppendHtml(GetNumberHtml(freeSpins.Count));
                            rowMultiplier.InnerHtml.AppendHtml(GetFreeSpinsHtml(freeSpins.Odds, "Free Spins"));
                            rowMultiplier.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbleWin));
                        }
                        else
                        {
                            rowMultiplier.InnerHtml.AppendHtml(GetIndexRowHtml(index, rowspan));
                            rowMultiplier.InnerHtml.AppendHtml(GetResultBoardHtml(rowspan, renderBoard, tumbles[index].Multipliers));
                            rowMultiplier.InnerHtml.AppendHtml(GetCellWithColSpanHtml(2, "-"));
                            rowMultiplier.InnerHtml.AppendHtml(GetNumberHtml(0));
                            rowMultiplier.InnerHtml.AppendHtml(GetFormatNumberHtml(tumbleWin));
                        }
                        strBuilder.InnerHtml.AppendHtml(rowMultiplier.GetHtmlString());
                    }
                }                        
            }

            if(HasTotalMultiplier)
            {
                rowTotalMultiplier.InnerHtml.AppendHtml(GetCellWithColSpanHtml(5, "Total Multiplier", true));
                var multiplierValue = Round.FreeSpin ? Round.TotalMultiplier : Round.Multiplier;
                rowTotalMultiplier.InnerHtml.AppendHtml(GetNumberHtml(multiplierValue));
                strBuilder.InnerHtml.AppendHtml(rowTotalMultiplier.GetHtmlString());
            }

            rowWin.InnerHtml.AppendHtml(GetCellWithColSpanHtml(5, WinLabel, true));
            rowWin.InnerHtml.AppendHtml(GetFormatNumberHtml(Round.BetWin));
            strBuilder.InnerHtml.AppendHtml(rowWin.GetHtmlString());
            return strBuilder.GetHtmlString();
        }

        private string GetCellWithColSpanHtml(int col, string text = "", bool isClass = false)
        {
            var cell = new TagBuilder("td");
            var className = isClass ? "text-right" : "";
            cell.AddCssClass($"{className}");
            cell.MergeAttribute("colspan", $"{col}");
            cell.InnerHtml.Append($"{text}");
            return cell.GetHtmlString();
        }

        private string GetResultBoardHtml(int rowspan, byte[,] board, IEnumerable<byte> multipliers)
        {
            var cell = new TagBuilder("td");
            cell.Attributes.Add("rowspan", $"{rowspan}");
            cell.InnerHtml.AppendHtml(GetBoardContentHtml(board, multipliers));
            return cell.GetHtmlString();
        }

        private string GetFreeSpinsHtml(decimal odds, string freeSpins)
        {
            var cell = new TagBuilder("td");
            cell.InnerHtml.Append($"{odds} {freeSpins}");

            return cell.GetHtmlString();
        }

        private string GetNumberHtml(decimal value)
        {
            var cell = new TagBuilder("td");
            cell.InnerHtml.Append($"{value}");

            return cell.GetHtmlString();
        }

        private string GetFormatNumberHtml(decimal value)
        {
            var cell = new TagBuilder("td");
            cell.InnerHtml.Append($"{HtmlHelpers.formatNumber(value)}");

            return cell.GetHtmlString();
        }
      
        private string GetIndexRowHtml(byte index, int rowspan)
        {
            var cell = new TagBuilder("td");
            cell.Attributes.Add("rowspan", $"{rowspan}");
            cell.InnerHtml.Append($"{index+1}");

            return cell.GetHtmlString();
        }

        public string GetBoardContentHtml(byte[,] renderedBoard, IEnumerable<byte> multipliers)
        {
            var indexMultiplier = 0;
            var tempMultiplier = multipliers.ToList();
            var table = new TagBuilder("table");
            table.AddCssClass("table-result-board");
            var tbody = new TagBuilder("tbody");
            for (var row = 0; row < renderedBoard.GetLength(0); row++)
            {
                var rowTumble = new TagBuilder("tr");
                for (var column = 0; column < renderedBoard.GetLength(1); column++)
                {
                    var symbol = renderedBoard[row, column];
                    if (symbol >= CascadingSlotGameConstant.MinMultiplierSymbol)
                    {
                        rowTumble.InnerHtml.AppendHtml(GetMultiplierHtml(tempMultiplier[indexMultiplier]));
                        indexMultiplier++;
                    }
                    else
                    {
                        rowTumble.InnerHtml.AppendHtml(GetRowSymbolHtml(symbol));
                    }
                }
                tbody.InnerHtml.AppendHtml(rowTumble);
            }
          
            table.InnerHtml.AppendHtml(tbody);

            return table.GetHtmlString();
        }

        private string GetMultiplierHtml(byte value)
        {
            var cell = new TagBuilder("td");
            cell.AddCssClass("result-board");
            cell.InnerHtml.AppendHtml(GetImageMultiplier(value));
            return cell.GetHtmlString();
        }

        private string GetRowSymbolHtml(byte value)
        {
            var cell = new TagBuilder("td");
            cell.InnerHtml.AppendHtml(GetSymbolHtml(value));
            return cell.GetHtmlString();
        }

        public string GetSymbolHtml(byte item)
        {
            var tagBuilder = new TagBuilder("image");
            var imageUrl = GetSymbolUrl(item.ToString());
            tagBuilder.AddCssClass("symbol");
            tagBuilder.Attributes.Add("src", $"{imageUrl}");
            return tagBuilder.GetHtmlString();
        }

        public string GetImageMultiplier(byte value)
        {
            var strBuilder = new StringBuilder();
            string symbol;
            if (value <= 10)
            {
                symbol = this.GetSymbolHtml(13);
                strBuilder.Append(symbol);
            }
            else
            {
                if (value <= 20)
                {
                    symbol = this.GetSymbolHtml(11);
                    strBuilder.Append(symbol);
                }
                else if (value <= 30)
                {
                    symbol = this.GetSymbolHtml(12);
                    strBuilder.Append(symbol);
                }
            }
            strBuilder.Append(GetValueMultiplierHtml(value));
            return strBuilder.ToString();
        }

        private string GetValueMultiplierHtml(byte value)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass($"multiplier {GetSymbolUrl("")}");
            tagBuilder.InnerHtml.Append($"x{value}");

            return tagBuilder.GetHtmlString();
        }
    }
}
