using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class IronMeltTransDAL
    {
        #region 静态实例
        private static volatile IronMeltTransDAL instance = null;
        private static object lockHelper = new object();

        public static IronMeltTransDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new IronMeltTransDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region 新增铁水转运信息
        public static int AddIronMeltTransInfo(IronMeltTrans ironMeltTrans)
        {

            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(ironMeltTrans, "ProductionScheduleSID,DeptSID,GroupSID,EmployeeSID,EquipmentSID,ElectricFurnaceCount,IronMeltTransNum,IronMeltTransWeight,BackWeight,MeasureUnitSID,IronMeltTransTime", "MW_IronMeltTrans_INS");
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
