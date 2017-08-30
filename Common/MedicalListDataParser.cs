using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    public class MedicalListDataParser
    {
        public static IEnumerable<string> MedicalListParse(HtmlDocument doc)
        {
            var dataNodes = doc.DocumentNode.SelectNodes("//tr[@height=30]/td[@width=241 and @style='padding-left:15px;color:#000000;font-size:12px;text-align:left']");
            foreach (var dataNode in dataNodes)
            {
                yield return dataNode.InnerText;
            }
        }
    }
}
