using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Pack
    {
            [ColumnMapping("PackSID", "-1")]           //包装SID
            public int PackSID { get; set; }

            //[ColumnMapping("GroupSID", "-1")]           //班组
            //public int GroupSID { get; set; }

            //[ColumnMapping("EquipmentSID", "-1")]       //设备
            //public int EquipmentSID { get; set; }

            //[ColumnMapping("DeptSID", "-1")]            //部门
            //public int DeptSID { get; set; }

            //[ColumnMapping("MaterialSID", "-1")]        //产品
            //public int MaterialSID { get; set; }

            [ColumnMapping("PackNum", "-1")]            //包装数量
            public int PackNum { get; set; }

            [ColumnMapping("PackTotalNum", "-1")]       //总数量
            public float PackTotalNum { get; set; }

            //[ColumnMapping("PolishBegimTime", "9999-12-31")]      //打磨开始时间
            //public DateTime PolishBegimTime { get; set; }

            [ColumnMapping("PackEndTime", "9999-12-31")]       //包装结束时间
            public DateTime PackEndTime { get; set; }

            //[ColumnMapping("PolishWorkTime", "-1")]       //工作时间
            //public decimal PolishWorkTime { get; set; }

            //[ColumnMapping("PolishRemark", "")]                 //打磨备注
            //public string PolishRemark { get; set; }

    }
}
