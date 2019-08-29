using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Paint
    {
            [ColumnMapping("PaintSID", "-1")]           //喷漆SID
            public int PaintSID { get; set; }

            //[ColumnMapping("GroupSID", "-1")]           //班组
            //public int GroupSID { get; set; }

            //[ColumnMapping("EquipmentSID", "-1")]       //设备
            //public int EquipmentSID { get; set; }

            //[ColumnMapping("DeptSID", "-1")]            //部门
            //public int DeptSID { get; set; }

            //[ColumnMapping("MaterialSID", "-1")]        //产品
            //public int MaterialSID { get; set; }

            [ColumnMapping("PaintNum", "-1")]            //喷漆数量
            public int PaintNum { get; set; }

            [ColumnMapping("PaintTotalNum", "-1")]       //总数量
            public int PaintTotalNum { get; set; }

            //[ColumnMapping("PolishBegimTime", "9999-12-31")]      //打磨开始时间
            //public DateTime PolishBegimTime { get; set; }

            [ColumnMapping("PaintEndTime", "9999-12-31")]       //喷漆结束时间
            public DateTime PaintEndTime { get; set; }

            //[ColumnMapping("PolishWorkTime", "-1")]       //工作时间
            //public decimal PolishWorkTime { get; set; }

            //[ColumnMapping("PolishRemark", "")]                 //打磨备注
            //public string PolishRemark { get; set; }

    }
}
