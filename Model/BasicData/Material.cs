using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class Material
    {
       
        // 物料序号
        [ColumnMapping("MaterialSID", "-1")]
        public int MaterialSID { get; set; }

        // 物料类别序号
        [ColumnMapping("MaterialCategorySID", "")]
        public String MaterialCategorySID { get; set; }

        // 物料编号
        [ColumnMapping("MaterialID", "")]
        public String MaterialID { get; set; }

        // 物料名称
        [ColumnMapping("MaterialName", "")]
        public String MaterialName { get; set; }

        // 物料名称缩写
        [ColumnMapping("MaterialShortName", "")]
        public String MaterialShortName { get; set; }

        // 物料规格
        [ColumnMapping("MaterialSpec", "")]
        public String MaterialSpec { get; set; }

    }
}
