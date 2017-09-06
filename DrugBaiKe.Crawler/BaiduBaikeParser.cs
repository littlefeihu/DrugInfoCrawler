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
            var sidetitleElements = doc.DocumentNode.SelectNodes("//dt[@class='catalog-title level1']");
            foreach (var sidetitleElement in sidetitleElements)
            {
                var title = sidetitleElement.SelectSingleNode("./a/span/span[@nslog-type='10002802']").InnerText;

            }

            var titleElements = doc.DocumentNode.SelectNodes("//div[@class='para-title level-2']");
            foreach (var titleElement in titleElements)
            {
                var content = GetContent(titleElement);

            }

        }


        private static string GetContent(HtmlNode titleElement)
        {
            string content = "";
            var node = titleElement.NextSibling;
            var attrclass = node.GetAttributeValue("class", "");
            do
            {
                if (attrclass == "para")
                {
                    content += node.OuterHtml;
                }

                node = node.NextSibling;
                attrclass = node.GetAttributeValue("class", "");

            } while (attrclass == "para" || node.NodeType == HtmlNodeType.Text);
            return content;

        }
    }
}
