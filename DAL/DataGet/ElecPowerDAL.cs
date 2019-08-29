using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class ElecPowerDAL
    {
        #region 静态实例
        private static volatile ElecPowerDAL instance = null;
        private static object lockHelper = new object();

        public static ElecPowerDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new ElecPowerDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region 新增电能数据
        public static int AddElecPower(ElecPower elecPower)
        {
            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(elecPower, "ElecMeter,ElecPowerData,ElecDate", "MW_ElecPower_INS");
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        #endregion

    
    }
}
