using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class IronMeltTrans
    {
        [ColumnMapping("ProductionScheduleSID", "-1")]   //订单
        public int ProductionScheduleSID { get; set; }

        [ColumnMapping("DeptSID", "-1")]   //部门
        public int DeptSID { get; set; }

        [ColumnMapping("IronMeltTranSID", "-1")]   //铁水SID
        public int IronMeltTranSID { get; set; }

        [ColumnMapping("EmployeeSID", "-1")]   //员工
        public int EmployeeSID { get; set; }

        [ColumnMapping("EquipmentSID", "-1")]   //电炉设备
        public int EquipmentSID { get; set; }

        [ColumnMapping("GroupSID", "-1")]       //班组
        public int GroupSID { get; set; }

        [ColumnMapping("ElectricFurnaceCount", "-1")]     //炉次
        public int ElectricFurnaceCount { get; set; }

        [ColumnMapping("IronMeltTransNum", "-1")]       //包次
        public int IronMeltTransNum { get; set; }

        [ColumnMapping("IronMeltTransWeight", "-1")]          //铁水重量
        public int IronMeltTransWeight { get; set; }

        [ColumnMapping("BackWeight", "-1")]                   //回水重量（行车吊装）
        public int BackWeight { get; set; }

        [ColumnMapping("MeasureUnitSID", "-1")]       //计量单位
        public int MeasureUnitSID { get; set; }

        [ColumnMapping("IronMeltTransTime", "9999-12-31")]  //转运时间
        public DateTime IronMeltTransTime { get; set; }

        [ColumnMapping("IronMeltTransRemark", "")]          //备注
        public string IronMeltTransRemark { get; set; }

        public IronMeltTrans()
        {
            DeptSID = 1;
            MeasureUnitSID = 1;
        }


    }
  
}
