using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Polish
    {
            [ColumnMapping("PolishSID", "-1")]           //打磨SID
            public int PolishSID { get; set; }

            [ColumnMapping("GroupSID", "-1")]           //班组
            public int GroupSID { get; set; }

            [ColumnMapping("EquipmentSID", "-1")]       //设备
            public int EquipmentSID { get; set; }

            [ColumnMapping("DeptSID", "-1")]            //部门
            public int DeptSID { get; set; }

            [ColumnMapping("MaterialSID", "-1")]        //产品
            public int MaterialSID { get; set; }
            private int _polishNum = -1;

            [ColumnMapping("PolishNum", "-1")]            //打磨数量
            public int PolishNum {
                get { return this._polishNum; }
                set { this._polishNum = value; }
            }

            private int _polishTotalNum = -1;

            [ColumnMapping("PolishTotalNum", "-1")]       //总数量
            public int PolishTotalNum
            {
                get { return this._polishTotalNum; }
                set { this._polishTotalNum = value; }
            }
    
            [ColumnMapping("PolishBegimTime", "9999-12-31")]      //打磨开始时间
            public DateTime PolishBegimTime { get; set; }

            [ColumnMapping("PolishEndTime", "9999-12-31")]       //打磨结束时间
            public DateTime PolishEndTime { get; set; }

            [ColumnMapping("PolishWorkTime", "-1")]       //工作时间
            public decimal PolishWorkTime { get; set; }

            [ColumnMapping("PolishRemark", "")]                 //打磨备注
            public string PolishRemark { get; set; }

    }
}
