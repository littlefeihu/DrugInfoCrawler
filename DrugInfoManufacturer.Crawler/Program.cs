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

namespace DrugInfoManufacturer.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {


            var db = new Model1();
            foreach (var product in db.Productions.Where(o => !o.Done).OrderBy(o => o.FromPage))
            {
                var db1 = new Model1();

                var frompage = int.Parse(System.Configuration.ConfigurationManager.AppSettings["fromPage"]);

                Pager page = new Pager { Currentpage = frompage };
                int correctPage = frompage;
                do
                {
                    var url = GetUrl(page.Currentpage + 1, product.ProductionName);
                    try
                    {
                        var html = HttpHelper.Get(url).Result;
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        //Thread.Sleep(1000);

                        //共 18702条&nbsp;&nbsp;&nbsp;&nbsp;第 1页/共1247页
                        var pageNodeText = doc.DocumentNode.SelectSingleNode(@"//tr[@height='70']/td[@width='30%' and @style='padding-left:30px']").InnerText;
                        page = PageParser.MedicalListParse(pageNodeText);

                        if (page.TotalCount == 0)
                        {
                            Console.WriteLine(product.ProductionName + "没有发现记录");
                            break;
                        }

                        Console.WriteLine("当前页：" + page.Currentpage + ",共" + page.TotalPage + "页");
                        foreach (var item in DrugListParser.Parse(doc))
                        {
                            item.ProductId = product.ID;
                            item.FromPage = page.Currentpage;
                            item.Class = "国产药";
                            db1.DrugItems.Add(item);
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

                var p = db1.Productions.FirstOrDefault(o => o.ID == product.ID);
                p.Done = true;
                db1.SaveChanges();
            }
            Console.ReadKey();
        }

        private static string GetUrl(int page, string name)
        {
            //进口药
            //return string.Format("http://app2.sfda.gov.cn/datasearchp/all.do?page={0}&name={1}&tableName=TABLE36&formRender=gjcx&searchcx=&paramter0={2}&paramter1=&paramter2=", page, HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(name));

            //国产药
            return string.Format("http://app2.sfda.gov.cn/datasearchp/all.do?page={0}&name={1}&tableName=TABLE25&formRender=cx&searchcx=&paramter0=&paramter1=&paramter2=", page, HttpUtility.UrlEncode(name));
        }

        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            //Proxy proxy = new Proxy();
            //proxy.HttpProxy = string.Format("127.0.0.1:9999");
            //service.ProxyType = "http";
            //service.Proxy = proxy.HttpProxy;
            return service;
        }
    }
}
