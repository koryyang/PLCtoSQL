using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class ProductionProblemDAL
    {
        #region 静态实例
        private static volatile ProductionProblemDAL instance = null;
        private static object lockHelper = new object();

        public static ProductionProblemDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new ProductionProblemDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region 新增生产故障信息
        public static int AddProductionProblem(int problemCateSID, DateTime productionProblemBeginTime, DateTime productionProblemEndTime,DateTime productionRecoveryTime)
        {
            ProductionProblem productionProblem = new ProductionProblem();
            productionProblem.ProblemCateSID = problemCateSID;
            productionProblem.ProductionProblemBeginTime = productionProblemBeginTime;
            productionProblem.ProductionProblemEndTime = productionProblemEndTime;
            productionProblem.ProductionRecoveryTime = productionRecoveryTime;
        


            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(productionProblem, "ProblemCateSID,ProductionProblemBeginTime,ProductionProblemEndTime,ProductionRecoveryTime", "MW_ProductionProblem_INS");
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
