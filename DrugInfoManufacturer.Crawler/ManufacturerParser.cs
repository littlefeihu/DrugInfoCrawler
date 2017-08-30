using Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfoManufacturer.Crawler
{
    public class ManufacturerParser
    {

        public static IEnumerable<DrugItem> Parse(HtmlDocument doc)
        {
            var rowNodes = doc.DocumentNode.SelectNodes("//table[@class='msgtab' and @style='background:#eaeaea' and @width='100%' and @cellspacing='1' and @cellpadding='0' and @border='0']/tbody/tr");
            for (int i = 1; i < rowNodes.Count; i++)
            {
                var columns = rowNodes[i].SelectNodes("./td");

                var item = new DrugItem
                {
                    Name = columns[1].InnerText,
                    PiZhunWenHao = columns[2].InnerText,
                    Manufacturer = columns[3].InnerText,
                    DosageForm = columns[4].InnerText,
                    Specification = columns[5].InnerText,
                    CompanyUrl = columns[3].SelectSingleNode("./a").GetAttributeValue("href", ""),
                    DrugUrl = columns[1].SelectSingleNode("./a").GetAttributeValue("href", "")
                };
                yield return item;
            }
        }
    }
}
