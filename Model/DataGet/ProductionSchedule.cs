using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class ProductionSchedule
    {
            [ColumnMapping("ProductionScheduleSID", "-1")]           //SID
            public int ProductionScheduleSID { get; set; }


    }
}
