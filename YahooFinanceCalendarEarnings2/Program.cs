using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace YahooFinanceCalendarEarnings2
{
    class Program
    {
        static void Main(string[] args)
        {
            Int32 i = 0;

            // Parameter Symbol
            String symbol = "";
            foreach (var arg in args)
            {
                if (i == 0) { symbol = arg; }
                i++;
            }

            WebClient webClient = new WebClient();
            string page = webClient.DownloadString("https://finance.yahoo.com/calendar/earnings?symbol=" + symbol + "&fbclid=IwAR219Kj9C7K5C8r9Gxwm2sbvC6s5y4oO28rYD85_JBWdPygZ8xj9QgX83qg");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            List<Earnings> earnings = new List<Earnings>();

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table[contains(@class, 'W(100%)')]"))
            {
                foreach (HtmlNode tableBody in table.SelectNodes("tbody"))
                {
                    Console.WriteLine("No. of Earnings: " + tableBody.SelectNodes("tr").Count());
                    Console.WriteLine();

                    foreach (HtmlNode tableRow in tableBody.SelectNodes("tr"))
                    {
                        HtmlNodeCollection tableColumns = tableRow.SelectNodes("td");
                        if (tableColumns.Any())
                        {
                            earnings.Add(new Earnings()
                            {
                                Symbol = tableColumns[0].InnerText,
                                Company = tableColumns[1].InnerText,
                                EarningsDate = tableColumns[2].InnerText,
                                EPSEstimate = tableColumns[3].InnerText,
                                ReportedEPS = tableColumns[4].InnerText,
                                SurprisePercentage = tableColumns[5].InnerText
                            });
                        }
                    }
                }
            }

            if (earnings.Any())
            {
                foreach (var earning in earnings)
                {
                    Console.WriteLine("Symbol: " + earning.Symbol);
                    Console.WriteLine("Company: " + earning.Company);
                    Console.WriteLine("Earnings Date: " + earning.EarningsDate);
                    Console.WriteLine("EPS Estimate: " + earning.EPSEstimate);
                    Console.WriteLine("Reported EPS: " + earning.ReportedEPS);
                    Console.WriteLine("Surprise (%): " + earning.SurprisePercentage);
                    Console.WriteLine();
                }
            }

            Console.ReadKey();
        }

        class Earnings
        {
            public String Symbol { get; set; }
            public String Company { get; set; }
            public String EarningsDate { get; set; }
            public String EPSEstimate { get; set; }
            public String ReportedEPS { get; set; }
            public String SurprisePercentage { get; set; }
        }
    }
}
