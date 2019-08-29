using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class ProductionProblem
    {
       
        // 生产故障序号
        [ColumnMapping("ProductionProblemSID", "-1")]
        public int ProductionProblemSID { get; set; }

        // 故障类别序号
        [ColumnMapping("ProblemCateSID", "-1")]
        public int ProblemCateSID { get; set; }

        // 故障开始时间
        [ColumnMapping("ProductionProblemBeginTime", "9999-12-31")]
        public DateTime ProductionProblemBeginTime { get; set; }

        // 故障结束时间
        [ColumnMapping("ProductionProblemEndTime", "9999-12-31")]
        public DateTime ProductionProblemEndTime { get; set; }

        // 恢复生产时间
        [ColumnMapping("ProductionRecoveryTime", "9999-12-31")]
        public DateTime ProductionRecoveryTime { get; set; }

        // 生产故障备注
        [ColumnMapping("ProductionProblemRemark", "")]
        public String ProductionProblemRemark { get; set; }

    }
}
