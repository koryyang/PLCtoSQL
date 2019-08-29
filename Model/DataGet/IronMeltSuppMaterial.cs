using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class IronMeltSuppMaterial
    {

        [ColumnMapping("GroupSID", "-1")]       //班组
        public int GroupSID { get; set; }

        [ColumnMapping("IronMeltTranSID", "-1")]     //铁水转运主表
        public int IronMeltTranSID { get; set; }

        [ColumnMapping("MaterialSID", "-1")]       //物料
        public int MaterialSID { get; set; }

        [ColumnMapping("IronMeltSuppMaterialWeight", "-1")]          //加料重量
        public int IronMeltSuppMaterialWeight { get; set; }

        [ColumnMapping("IronMeltSuppMaterialTime", "9999-12-31")]  //加料时间
        public DateTime IronMeltSuppMaterialTime { get; set; }

        [ColumnMapping("IronMeltSuppMaterialRemark", "")]          //备注
        public string IronMeltSuppMaterialRemark { get; set; }




    }
}
