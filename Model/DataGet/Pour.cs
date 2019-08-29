using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Pour
    {
            [ColumnMapping("PourSID", "-1")]           //浇注SID
            public int PourSID { get; set; }

            //[ColumnMapping("GroupSID", "-1")]           //班组
            //public int GroupSID { get; set; }

            //[ColumnMapping("EquipmentSID", "-1")]       //设备
            //public int EquipmentSID { get; set; }

            //[ColumnMapping("DeptSID", "-1")]            //部门
            //public int DeptSID { get; set; }

            //[ColumnMapping("MaterialSID", "-1")]        //产品
            //public int MaterialSID { get; set; }

            [ColumnMapping("PourProductFBoxNum", "-1")]            //浇注首箱号
            public int PourProductFBoxNum { get; set; }

            [ColumnMapping("PourProductLBoxNum", "-1")]       //浇注尾箱号
            public int PourProductLBoxNum { get; set; }

            [ColumnMapping("PourProductPassBoxNum", "-1")]       //浇注过箱号
            public int PourProductPassBoxNum { get; set; }


            [ColumnMapping("PourProductBeginTime", "9999-12-31")]      //浇注开始时间
            public DateTime PourProductBeginTime { get; set; }

            [ColumnMapping("PourProductEndTime", "9999-12-31")]       //浇注结束时间
            public DateTime PourProductEndTime { get; set; }

        //[ColumnMapping("PolishWorkTime", "-1")]       //工作时间
        //public decimal PolishWorkTime { get; set; }

        //[ColumnMapping("PolishRemark", "")]                 //打磨备注
        //public string PolishRemark { get; set; }

    }
}
