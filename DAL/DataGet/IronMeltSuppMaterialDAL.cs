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
        public static int AddIronMeltSuppMaterialInfo(int groupSID,int ironMeltTranSID, int materialSID, int ironMeltSuppMaterialWeight, DateTime ironMeltSuppMaterialTime)
        {
            IronMeltSuppMaterial ironMeltSuppMaterial = new IronMeltSuppMaterial();
            ironMeltSuppMaterial.GroupSID = groupSID;
            ironMeltSuppMaterial.IronMeltTranSID = ironMeltTranSID;
            ironMeltSuppMaterial.MaterialSID = materialSID;
            ironMeltSuppMaterial.IronMeltSuppMaterialWeight = ironMeltSuppMaterialWeight;
            ironMeltSuppMaterial.IronMeltSuppMaterialTime = ironMeltSuppMaterialTime;


            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(ironMeltSuppMaterial, "GroupSID,IronMeltTranSID,MaterialSID,IronMeltSuppMaterialWeight,IronMeltSuppMaterialTime", "DG_IronMeltSuppMaterial_INS_MW");
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
