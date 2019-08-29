using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Blasting
    {
            [ColumnMapping("BlastingSID", "-1")]           //SID
            public int BlastingSID { get; set; }

            [ColumnMapping("GroupSID", "-1")]           //班组
            public int GroupSID { get; set; }

            [ColumnMapping("EquipmentSID", "-1")]       //设备
            public int EquipmentSID { get; set; }

            [ColumnMapping("DeptSID", "-1")]            //部门
            public int DeptSID { get; set; }

            [ColumnMapping("MaterialSID", "-1")]        //产品
            public int MaterialSID { get; set; }

            [ColumnMapping("BlastingNum", "-1")]            //抛丸数量
            public int BlastingNum { get; set; }

            [ColumnMapping("BlastingTotalNum", "-1")]            //总数量
            public int BlastingTotalNum { get; set; }
  
            [ColumnMapping("BlastingBeginTime", "9999-12-31")]      //抛丸开始时间
            public DateTime BlastingBeginTime { get; set; }

            [ColumnMapping("BlastingEndTime", "9999-12-31")]       //抛丸结束时间
            public DateTime BlastingEndTime { get; set; }

            [ColumnMapping("BlastingRemark", "")]                 //抛丸备注
            public string BlastingRemark { get; set; }

    }
}
