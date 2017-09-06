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
        public static Manufacturer Parse(HtmlDocument doc, out List<GMPInfo> gmpInfos)
        {
            //<table border="0" cellpadding="0" cellspacing="1" width="100%" style="border:0px #989898 solid;background:#eaeaea" class=msgtab>
            var tableNodes = doc.DocumentNode.SelectNodes("//table[@class='msgtab']");

            gmpInfos = new List<GMPInfo>();
            var trCompanyArr = tableNodes[0].SelectNodes("./tr");
            Manufacturer manufacturer = null;
            if (trCompanyArr.Count >= 10)
            {
                manufacturer = new Manufacturer
               {
                   EnterpriseNumber = trCompanyArr[0].SelectNodes("./td")[0].InnerText,
                   SocialCreditCode = trCompanyArr[0].SelectNodes("./td")[1].InnerText,
                   ClassificationCode = trCompanyArr[1].SelectNodes("./td")[0].InnerText,
                   ProvinceCity = trCompanyArr[1].SelectNodes("./td")[1].InnerText,
                   EnterpriseName = trCompanyArr[2].SelectNodes("./td")[0].InnerText,
                   LegalRepresentative = trCompanyArr[2].SelectNodes("./td")[1].InnerText,
                   OwnersofEnterprises = trCompanyArr[3].SelectNodes("./td")[0].InnerText,
                   QualityDirecter = trCompanyArr[3].SelectNodes("./td")[1].InnerText,
                   RegisteredAddress = trCompanyArr[4].SelectNodes("./td")[0].InnerText,
                   ProductionAddress = trCompanyArr[4].SelectNodes("./td")[1].InnerText,
                   ProductionRange = trCompanyArr[5].SelectNodes("./td")[0].InnerText,
                   LicenceDate = trCompanyArr[5].SelectNodes("./td")[1].InnerText,
                   DateofExpiry = trCompanyArr[6].SelectNodes("./td")[0].InnerText,
                   LicenceIssuingAuthority = trCompanyArr[6].SelectNodes("./td")[1].InnerText,
                   Signer = trCompanyArr[7].SelectNodes("./td")[0].InnerText,
                   RoutineSupervisionOrganization = trCompanyArr[7].SelectNodes("./td")[1].InnerText,
                   DailySupervisor = trCompanyArr[8].SelectNodes("./td")[0].InnerText,
                   ComplainingTelephone = trCompanyArr[8].SelectNodes("./td")[1].InnerText,
                   Remark = trCompanyArr[9].SelectNodes("./td")[0].InnerText
               };
            }

            HtmlNodeCollection trGMPArr = null;
            if (tableNodes.Count == 4)
            {
                trGMPArr = tableNodes[0].SelectNodes("./tr/td/table/tr");
            }
            else
            {
                trGMPArr = tableNodes[1].SelectNodes("./tr/td/table/tr");
            }
            if (trGMPArr != null)
            {
                foreach (var tr in trGMPArr)
                {
                    var gmpInfo = new GMPInfo
                    {
                        GMPDrugName = tr.SelectNodes("./td")[2].InnerText,
                        GMPNUM = tr.SelectNodes("./td")[1].InnerText,
                        ManufacturerName = tr.SelectNodes("./td")[0].InnerText
                    };
                    gmpInfos.Add(gmpInfo);
                }
            }
            return manufacturer;
        }
    }
}
