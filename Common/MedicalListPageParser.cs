using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    public class PageParser
    {
        /// <summary>
        /// 解析药品搜索页面页码信息
        /// </summary>
        /// <param name="pageNodeText"></param>
        /// <returns></returns>
        public static Pager MedicalListParse(string pageNodeText)
        { //共 18702条&nbsp;&nbsp;&nbsp;&nbsp;第 1页/共1247页
            var totalCount = pageNodeText.Substring(1, pageNodeText.IndexOf("条") - 1);
            var from = pageNodeText.IndexOf("第") + 1;
            var to = pageNodeText.IndexOf("页") - from;
            var currentPageNum = pageNodeText.Substring(from, to);

            from = pageNodeText.LastIndexOf("共") + 1;
            to = pageNodeText.Length - 1 - from;
            var totalPageNum = pageNodeText.Substring(from, to);

            return new Pager { Currentpage = int.Parse(currentPageNum), TotalCount = int.Parse(totalCount), TotalPage = int.Parse(totalPageNum) };
        }
    }
}
