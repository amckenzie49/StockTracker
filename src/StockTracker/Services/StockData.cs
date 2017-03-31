using System;
using StockTracker.Entities;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;

namespace StockTracker.Services
{
    public interface IStockData
    {
        //the only exposed method
        Task getStockData(Stock s);
    }

    public class StockData : IStockData
    {
        public async Task getStockData(Stock stock)
        {
            await getCurrentStockData(stock);

            //this is less likely to work right
            await getHistoricalStockData(stock);

        }

        //historical data is provided by yahoo but must be consumed using the csv api rather than YQL
        //NOTE g=y and c=whatever doesn't work at all for the csv thing....
        private async Task getHistoricalStockData(Stock stock)
        {
            //what was the year 20 years ago?
            string year = DateTime.Today.AddYears(-20).Year.ToString();

            //hisotrical stock data only showing annual data
            string baseURL = "http://ichart.finance.yahoo.com/table.csv?s={0}&c={1}&g=m";
            string symbol = stock.TickerSymbol;
            string url = string.Format(baseURL, symbol, year);

            using (var client = new HttpClient())
            {
                int i = 1;
                string data = await client.GetStringAsync(url);
                string[] rows = data.Split(Environment.NewLine.ToCharArray());
                //DateTime lastDate = DateTime.Today.AddYears(-1);

                //start at 1, skip the header
                //also check that the row has something, the last row will exist but be blank
                //NOTE: blank row of data will break this
                while (rows[i].Length > 0)
                {
                    string[] columns = rows[i].Split(',');
                    DateTime date;
                    decimal close;
                    decimal adjclose;

                    //is there anything I can/should do if a date doesn't parse?
                    DateTime.TryParse(columns[0], out date);
                    Decimal.TryParse(columns[4], out close);
                    Decimal.TryParse(columns[6], out adjclose);

                    //will these actually be annual or monthly then?
                    stock.AnnualClosingValues.Add(new StockClose(close, date, adjclose));

                    //keep the last determined date around just in case we can't parse?
                    //lastDate = date;

                    i++;
                }
            }
        }


        //grab the current data on this ticker symbol
        private static async Task getCurrentStockData(Stock stock)
        {
            string baseURL = "http://query.yahooapis.com/v1/public/yql?q=" +
                                                    "select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})" +
                                                    "&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

            string symbolList = "%22" + stock.TickerSymbol + "%22";
            string url = string.Format(baseURL, symbolList);

            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(url))
            {
                XDocument doc = XDocument.Load(stream);

                try
                {
                    XElement quote = doc.Root.Element("results").Element("quote");

                    stock.Ask = Decimal.Parse(quote.Element("Ask").Value.Replace("%", ""));
                    stock.Name = quote.Element("Name").Value;
                }
                catch (Exception)
                {
                    stock.Ask = 0;
                }
            }
        }
    }
}
