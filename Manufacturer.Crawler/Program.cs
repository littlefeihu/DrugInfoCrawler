using Common;
using DrugInfo.Crawler;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GetManufacturer.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {

            var driver1 = new PhantomJSDriver(GetPhantomJSDriverService());

            Pager page = new Pager { Currentpage = 0 };
            var url = GetUrl();
            do
            {
                try
                {
                    var db1 = new Model1();
                    //driver1.Navigate().GoToUrl(url);
                    var html = HttpHelper.Get(url).Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(driver1.PageSource);

                    //Thread.Sleep(1000); 

                    //共 18702条&nbsp;&nbsp;&nbsp;&nbsp;第 1页/共1247页
                    var pageNodeText = doc.DocumentNode.SelectSingleNode(@"//tr/td[@width='200' and @align='center']").InnerText;
                    page = PageParser.MedicalListParse(pageNodeText);

                    if (page.TotalCount == 0)
                    {
                        break;
                    }

                    Console.WriteLine("当前页：" + page.Currentpage + ",共" + page.TotalPage + "页");
                    foreach (var item in ManufacturerListParser.Parse(doc))
                    {
                        db1.Manufacturers.Add(item);
                    }
                    try
                    {
                        db1.SaveChanges();
                    }
                    catch (Exception)
                    {
                        db1.SaveChanges();
                    }

                    Console.WriteLine("保存完成");
                }
                catch (Exception)
                {
                    continue;
                }
            } while (page.Currentpage < page.TotalPage);


            Console.ReadKey();
        }

        private static string GetUrl()
        {
            return string.Format("http://app1.sfda.gov.cn/datasearch/face3/base.jsp?tableId=34&tableName=TABLE34&title=%E8%8D%AF%E5%93%81%E7%94%9F%E4%BA%A7%E4%BC%81%E4%B8%9A&bcId=118103348874362715907884020353");
        }
        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            //Proxy proxy = new Proxy();
            //proxy.HttpProxy = string.Format("36.41.143.57:8118");
            //service.ProxyType = "http";
            //service.Proxy = proxy.HttpProxy;
            return service;
        }

    }
}
