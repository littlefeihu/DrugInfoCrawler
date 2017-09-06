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
            var db = new Model1();
            foreach (var manufacturer in db.Manufacturers.Where(o => !o.Done).OrderBy(o => o.ID))
            {
                try
                {
                    var db1 = new Model1();

                    string html = "";
                    html = HttpHelper.Get("http://app2.sfda.gov.cn" + manufacturer.Link).Result;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    List<GMPInfo> gmpInfos;
                    var filledManufacturer = ManufacturerListParser.Parse(doc, out gmpInfos);
                    if (filledManufacturer != null)
                    {
                        filledManufacturer.Link = manufacturer.Link;
                        filledManufacturer.LSST = DateTime.Now;
                        filledManufacturer.Done = true;
                        filledManufacturer.ManufacturerName = manufacturer.ManufacturerName;
                        filledManufacturer.ID = manufacturer.ID;
                        db1.Entry(filledManufacturer).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        db1.Manufacturers.FirstOrDefault(o => o.ID == manufacturer.ID).Done = true;

                    }

                    foreach (var gmpInfo in gmpInfos)
                    {
                        gmpInfo.ManufacturerID = manufacturer.ID;
                        db1.GMPInfos.Add(gmpInfo);
                    }

                    db1.SaveChanges();

                    Thread.Sleep(3000);
                    Console.WriteLine("保存完成" + manufacturer.ManufacturerName);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            Console.ReadKey();
        }

        private static string GetUrl()
        {
            return "http://app2.sfda.gov.cn//datasearchp/index1.do?tableId=25&company=company&tableName=TABLE25&tableView=%E5%9B%BD%E4%BA%A7%E8%8D%AF%E5%93%81&Id=207919";
            //return string.Format("http://app2.sfda.gov.cn//datasearchp/index1.do?tableId=25&company=company&tableName=TABLE25&tableView=%E5%9B%BD%E4%BA%A7%E8%8D%AF%E5%93%81&Id=65346");
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
