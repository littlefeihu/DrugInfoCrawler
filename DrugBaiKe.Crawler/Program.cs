using Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrugBaiKe.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Model1();
            foreach (var product in db.Productions.Where(o => !o.Done))
            {
                var db1 = new Model1();
                var name = product.ProductionName;
                //name = "单硝酸异山梨酯片";
                string html = "";
                html = HttpHelper.Get("https://baike.baidu.com/item/" + name).Result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var pro = BaiduBaikeParser.Parse(doc);
                if (pro == null)
                    continue;
                pro.ID = product.ID;
                pro.ProductionName = product.ProductionName;
                pro.LSST = DateTime.Now;
                pro.Done = true;
                pro.FromPage = product.FromPage;
                db1.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                db1.SaveChanges();

                Console.WriteLine("保存成功" + name);
                Thread.Sleep(1000);
            }

            Console.WriteLine("解析结束");
            Console.ReadKey();
        }
    }
}
