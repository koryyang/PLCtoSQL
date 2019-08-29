using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class MaterialDAL
    {
        #region 静态实例
        private static volatile MaterialDAL instance = null;
        private static object lockHelper = new object();

        public static MaterialDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new MaterialDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

   

        #region 查询孕育剂物料信息
        public static List<Material> LoadMaterals(string materialShortName)
        {
            List<Material> materials = null;
            Material material = new Material();

            material.MaterialShortName = materialShortName;


            try
            {
                materials = DataFactory.FillEntities<Material>(material, "MaterialShortName", "MW_Material_SEL");
            }
            catch (Exception)
            {
                materials = new List<Material>();
            }
            return materials;
        }
        #endregion
    }
}
