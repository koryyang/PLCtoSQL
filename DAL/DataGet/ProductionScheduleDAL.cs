using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class ProductionScheduleDAL
    {
        #region 静态实例
        private static volatile ProductionScheduleDAL instance = null;
        private static object lockHelper = new object();

        public static ProductionScheduleDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new ProductionScheduleDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion


        #region 查询订单最新SID
        public static int GetProductionScheduleSID()
        {
            List<ProductionSchedule> productionSchedules = null;
            ProductionSchedule productionSchedule = new ProductionSchedule();

            try
            {
                productionSchedules = DataFactory.FillEntities<ProductionSchedule>(productionSchedule, "", "MW_ProductionScheduleSID_SEL");
            }
            catch (Exception ex)
            {
                productionSchedules = new List<ProductionSchedule>();
            }
            return productionSchedules[0].ProductionScheduleSID;
        }

        #endregion

    }
}
