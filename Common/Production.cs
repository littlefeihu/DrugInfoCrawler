namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production")]
    public partial class Production
    {
        public int ID { get; set; }

        [StringLength(550)]
        public string ProductionName { get; set; }

        public DateTime? LSST { get; set; }

        public int FromPage { get; set; }

        public bool Done { get; set; }


        public int Importeddrug { get; set; }

        /// <summary>
        /// 成分
        /// </summary>
        public string Ingredient { get; set; }
        /// <summary>
        /// 性状
        /// </summary>
        public string Character { get; set; }
        /// <summary>
        /// 适应症
        /// </summary>
        public string PrimaryUses { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// 用法
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 不良反应
        /// </summary>
        public string UntowardEffect { get; set; }
        /// <summary>
        /// 禁忌
        /// </summary>
        public string Tabu { get; set; }
        /// <summary>
        /// 注意事项
        /// </summary>
        public string Matters { get; set; }
        /// <summary>
        /// 孕妇用药
        /// </summary>
        public string PregnantUse { get; set; }

        /// <summary>
        /// 儿童用药
        /// </summary>
        public string PediatricDrugs { get; set; }
        /// <summary>
        /// 老人用药
        /// </summary>
        public string OlderDrugs { get; set; }
        /// <summary>
        /// 药物相互作用
        /// </summary>
        public string DrugInteractions { get; set; }
        /// <summary>
        /// 药物过量
        /// </summary>
        public string OverDose { get; set; }
        /// <summary>
        /// 药理毒理
        /// </summary>
        public string Toxicology { get; set; }
        /// <summary>
        /// 药代动力学
        /// </summary>
        public string Pharmacokinetics { get; set; }
        /// <summary>
        /// 存储
        /// </summary>
        public string Store { get; set; }
        /// <summary>
        /// 打包
        /// </summary>
        public string Packaging { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string Indate { get; set; }
        /// <summary>
        /// HTML内容
        /// </summary>
        public string htmlContent { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string Summary { get; set; }
        public string BasicInfo { get; set; }
        /// <summary>
        /// 目录
        /// </summary>
        public string Catalog { get; set; }

        public virtual ICollection<DrugItem> DrugItems { get; set; }
    }
}
