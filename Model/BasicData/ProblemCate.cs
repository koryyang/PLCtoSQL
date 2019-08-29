using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class ProblemCate
    {

        // 故障类别序号
        [ColumnMapping("ProblemCateSID", "-1")]
        public int ProblemCateSID { get; set; }


        // 故障类别描述
        [ColumnMapping("ProblemCateDesc", "")]
        public String ProblemCateDesc { get; set; }

        // 故障类别
        [ColumnMapping("ProblemType", "-1")]
        public int ProblemType { get; set; }


    }
}
