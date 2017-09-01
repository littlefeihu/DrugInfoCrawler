using Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetManufacturer.Crawler
{
    public class ManufacturerListParser
    {
        /// <summary>
        /// 药品生产企业列表解析
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IEnumerable<Manufacturer> Parse(HtmlDocument doc)
        {
            var rowNodes = doc.DocumentNode.SelectNodes("//div[@id='content']/table[1]/tr/td[@height='30']/p[@align='left']");
            for (int i = 1; i < rowNodes.Count; i++)
            {
                var columns = rowNodes[i].SelectNodes("./a");

                var item = new Manufacturer
                {
                    ManufacturerName = rowNodes[i].InnerText,
                    Link = rowNodes[i].InnerHtml,
                    LSST = DateTime.Now
                };
                yield return item;
            }
        }
    }
}
