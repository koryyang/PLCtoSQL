using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;

namespace FuhuaMiddleware.Model
{
    public class ElecPower
    {
            [ColumnMapping("ElecPowerSID", "-1")]           //电能SID
            public int ElecPowerSID { get; set; }

            [ColumnMapping("ElecMeter", "-1")]            //电表号
            public int ElecMeter { get; set; }

            [ColumnMapping("ElecPowerData", "-1")]       //电能
            public float ElecPowerData { get; set; }

            [ColumnMapping("ElecDate", "9999-12-31")]       //时间
            public DateTime ElecDate { get; set; }


    }
}
