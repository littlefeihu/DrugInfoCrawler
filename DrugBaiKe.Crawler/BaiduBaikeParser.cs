using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugBaiKe.Crawler
{
    public class BaiduBaikeParser
    {
        public static void Parse(HtmlDocument doc)
        {
            var titles = doc.DocumentNode.SelectNodes("\\dt[@class='catalog-title level1']");
            foreach (var title in titles)
            {
                title.SelectSingleNode("./span[@nslog-type='10002802']");
            }

        }
    }
}
