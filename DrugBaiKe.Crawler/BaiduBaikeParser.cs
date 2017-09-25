using Common;
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
        public static Production Parse(HtmlDocument doc)
        {
            var sidetitleElements = doc.DocumentNode.SelectNodes("//dt[@class='catalog-title level1']");
            if (sidetitleElements == null)
                return null;

            List<string> titles = new List<string>();
            List<string> contents = new List<string>();
            foreach (var sidetitleElement in sidetitleElements)
            {
                var title = sidetitleElement.SelectSingleNode("./a/span/span[@nslog-type='10002802']").InnerText;
                titles.Add(title);
            }
            var titleElements = doc.DocumentNode.SelectNodes("//div[@class='para-title level-2']");
            foreach (var titleElement in titleElements)
            {
                var content = GetContent(titleElement);
                contents.Add(content);
            }
            var pro = new Production();

            var summarydiv = doc.DocumentNode.SelectSingleNode("//div[@class='lemma-summary' and @label-module='lemmaSummary']");
            if (summarydiv != null)
                pro.Summary = summarydiv.InnerText;
            var basicInfoDiv = doc.DocumentNode.SelectSingleNode("//div[@class='basic-info cmn-clearfix']");
            if (basicInfoDiv != null)
                pro.BasicInfo = basicInfoDiv.InnerHtml;


            pro.Catalog = doc.DocumentNode.SelectSingleNode("//dl[@class='catalog-list']").InnerHtml;


            for (int i = 0; i < titles.Count; i++)
            {
                switch (titles[i])
                {
                    case "成份":
                        pro.Ingredient = contents[i];
                        break;
                    case "性状":
                        pro.Character = contents[i];
                        break;
                    case "适应症":
                        pro.PrimaryUses = contents[i];
                        break;
                    case "规格":
                        pro.Specification = contents[i];
                        break;
                    case "用法用量":
                        pro.Usage = contents[i];
                        break;
                    case "不良反应":
                        pro.UntowardEffect = contents[i];
                        break;
                    case "禁忌":
                        pro.Tabu = contents[i];
                        break;
                    case "注意事项":
                        pro.Matters = contents[i];
                        break;
                    case "孕妇及哺乳期妇女用药":
                        pro.PregnantUse = contents[i];
                        break;
                    case "儿童用药":
                        pro.PediatricDrugs = contents[i];
                        break;
                    case "老年用药":
                        pro.OlderDrugs = contents[i];
                        break;
                    case "药物相互作用":
                        pro.DrugInteractions = contents[i];
                        break;
                    case "药物过量":
                        pro.OverDose = contents[i];
                        break;
                    case "药理毒理":
                        pro.Toxicology = contents[i];
                        break;
                    case "药代动力学":
                        pro.Pharmacokinetics = contents[i];
                        break;
                    case "贮藏":
                        pro.Store = contents[i];
                        break;
                    case "包装":
                        pro.Packaging = contents[i];
                        break;
                    case "有效期":
                        pro.Indate = contents[i];
                        break;
                    case "执行标准":
                        pro.CarriedStandard = contents[i];
                        break;
                    default:
                        break;
                }
            }
            return pro;
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
                if (node != null)
                    attrclass = node.GetAttributeValue("class", "");
                else
                    break;

            } while (attrclass == "para" || node.NodeType == HtmlNodeType.Text);
            return content;

        }
    }
}
