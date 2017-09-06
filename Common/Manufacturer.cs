namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Manufacturer")]
    public partial class Manufacturer
    {
        public int ID { get; set; }

        public string ManufacturerName { get; set; }

        public DateTime? LSST { get; set; }

        public string Link { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string EnterpriseNumber { get; set; }
        /// <summary>
        /// 社会信用代码
        /// </summary>
        public string SocialCreditCode { get; set; }
        /// <summary>
        /// 分类码
        /// </summary>
        public string ClassificationCode { get; set; }
        /// <summary>
        /// 省市
        /// </summary>
        public string ProvinceCity { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 法定代表人
        /// </summary>
        public string LegalRepresentative { get; set; }
        /// <summary>
        /// 企业负责人
        /// </summary>
        public string OwnersofEnterprises { get; set; }
        /// <summary>
        /// 质量负责人
        /// </summary>
        public string QualityDirecter { get; set; }
        /// <summary>
        /// 注册地址
        /// </summary>
        public string RegisteredAddress { get; set; }
        /// <summary>
        /// 生产地址
        /// </summary>
        public string ProductionAddress { get; set; }
        /// <summary>
        /// 生产范围
        /// </summary>
        public string ProductionRange { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public string LicenceDate { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string DateofExpiry { get; set; }
        /// <summary>
        /// 发证机关
        /// </summary>
        public string LicenceIssuingAuthority { get; set; }
        /// <summary>
        /// 签发人
        /// </summary>
        public string Signer { get; set; }
        /// <summary>
        /// 日常监管机构
        /// </summary>
        public string RoutineSupervisionOrganization { get; set; }
        /// <summary>
        /// 日常监管人
        /// </summary>
        public string DailySupervisor { get; set; }
        /// <summary>
        /// 举报电话
        /// </summary>
        public string ComplainingTelephone { get; set; }
        public string Remark { get; set; }

        public bool Done { get; set; }
    }
}
