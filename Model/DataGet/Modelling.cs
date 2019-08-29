using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Modelling
    {
            [ColumnMapping("ModellingSID", "-1")]           //造型SID
            public int ModellingSID { get; set; }

            //[ColumnMapping("GroupSID", "-1")]           //班组
            //public int GroupSID { get; set; }

            //[ColumnMapping("EquipmentSID", "-1")]       //设备
            //public int EquipmentSID { get; set; }

            //[ColumnMapping("DeptSID", "-1")]            //部门
            //public int DeptSID { get; set; }

            //[ColumnMapping("MaterialSID", "-1")]        //产品
            //public int MaterialSID { get; set; }

            [ColumnMapping("ModellingBoxNum", "-1")]            //造型箱数
            public int ModellingBoxNum { get; set; }

            [ColumnMapping("ModellingBadBoxNum", "-1")]       //坏箱数
            public int ModellingBadBoxNum { get; set; }

        [ColumnMapping("ModellingEmptyBoxNum", "-1")]       //空箱数
        public int ModellingEmptyBoxNum { get; set; }

        //[ColumnMapping("PolishBegimTime", "9999-12-31")]      //打磨开始时间
        //public DateTime PolishBegimTime { get; set; }

        [ColumnMapping("ModellingEndTime", "9999-12-31")]       //造型结束时间
        public DateTime ModellingEndTime { get; set; }

        //[ColumnMapping("PolishWorkTime", "-1")]       //工作时间
        //public decimal PolishWorkTime { get; set; }

        //[ColumnMapping("PolishRemark", "")]                 //打磨备注
        //public string PolishRemark { get; set; }

    }
}
