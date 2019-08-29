using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class IronMeltSuppMaterialDAL
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

        #region 新增铁水转运加料信息
        public static int AddIronMeltSuppMaterial(IronMeltSuppMaterial ironMeltSuppMaterial)
        {
       
            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(ironMeltSuppMaterial, "GroupSID,IronMeltTranSID,MaterialSID,IronMeltSuppMaterialWeight,IronMeltSuppMaterialTime", "MW_IronMeltSuppMaterial_INS");
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
