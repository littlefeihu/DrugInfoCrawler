using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            string optionType = "V1";
            //查看第N页药品
            string retrievedrugUrl = string.Format("http://app2.sfda.gov.cn/datasearchp/gzcxSearch.do?page={0}&searchcx=&optionType={1}&paramter0=null&paramter1=null&paramter2=null&formRender=cx", 1, optionType);



        }
    }
}
