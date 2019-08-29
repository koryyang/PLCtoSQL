using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class PackDAL
    {
        #region 静态实例
        private static volatile PackDAL instance = null;
        private static object lockHelper = new object();

        public static PackDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new PackDAL();
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

        #region 查询包装最新SID
        public static int GetPackSID( )
        {
            List<Pack> packs = null;
            Pack pack = new Pack();
            try
            {
                packs = DataFactory.FillEntities<Pack>(pack, "", "MW_PackSID_SEL");
            }
            catch (Exception)
            {
                packs = new List<Pack>();
            }
            return packs[0].PackSID;
        }

        #endregion

        #region 更新包装信息
        public static int UpdPack(Pack pack)
        {
            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(pack
                    , "PackSID,PackNum,PackTotalNum,PackEndTime", "MW_Pack_UPD");
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
