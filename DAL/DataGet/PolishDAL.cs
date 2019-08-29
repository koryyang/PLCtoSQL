using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class PolishDAL
    {
        #region 静态实例
        private static volatile PolishDAL instance = null;
        private static object lockHelper = new object();

        public static PolishDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new PolishDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        //#region 新增打磨信息
        //public static int AddPolishInfo(int equipmentSID,int groupSID,int materialSID, int polishNum, int polishTotalNum ,DateTime polishBegimTime, DateTime polishEndTime,decimal polishWorkTime)
        //{
        //    Polish polish = new Polish();
        //    polish.EquipmentSID = equipmentSID;
        //    polish.GroupSID = groupSID;
        //    polish.MaterialSID = materialSID;
        //    polish.PolishNum = polishNum;
        //    polish.PolishTotalNum = polishTotalNum;
        //    polish.PolishBegimTime = polishBegimTime;
        //    polish.PolishEndTime = polishEndTime;
        //    polish.PolishWorkTime = polishWorkTime;

        //    int result = 0;
        //    try
        //    {
        //        result = DataFactory.UpdateEntity(polish, "EquipmentSID,GroupSID,MaterialSID,PolishNum,PolishTotalNum,PolishBegimTime,PolishEndTime,PolishWorkTime", "DG_Polish_INS_MW");
        //    }
        //    catch (Exception)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}

        //#endregion

        #region 查询打磨最新SID
        public static int GetPolishSID(int num)
        {
            List<Polish> polishs = null;
            Polish polish = new Polish();
            try
            {
                polishs = DataFactory.FillEntities<Polish>(polish, "", "MW_PolishSID_SEL");
            }
            catch (Exception)
            {
                polishs = new List<Polish>();
            }

            if(num == 1){ return polishs[0].PolishSID; }

            else{return polishs[1].PolishSID; }

        }

        #endregion

        #region 更新打磨信息
        public static int UpdPolish(Polish polish)
        {
            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(polish
                    , "PolishSID,PolishNum,PolishTotalNum,PolishEndTime", "MW_Polish_UPD");
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        #endregion
    }
}
