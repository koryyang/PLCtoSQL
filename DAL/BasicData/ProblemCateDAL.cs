using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class ProblemCateDAL
    {
        #region 静态实例
        private static volatile ProblemCateDAL instance = null;
        private static object lockHelper = new object();

        public static ProblemCateDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new ProblemCateDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

   

        #region 查询故障类别信息
        public static List<ProblemCate> LoadProblemCates(int problemType)
        {
            List<ProblemCate> problemCates = null;
            ProblemCate problemCate = new ProblemCate();
            problemCate.ProblemType = problemType;




            try
            {
                problemCates = DataFactory.FillEntities<ProblemCate>(problemCate, "ProblemType", "MW_ProblemCate_SEL");
            }
            catch (Exception)
            {
                problemCates = new List<ProblemCate>();
            }
            return problemCates;
        }
        #endregion
    }
}
