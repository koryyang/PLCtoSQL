using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class BlastingDAL
    {
        #region 静态实例
        private static volatile BlastingDAL instance = null;
        private static object lockHelper = new object();

        public static BlastingDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new BlastingDAL();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        //#region 新增抛丸信息
        //public static int AddBlastingInfo(int equipmentSID,int groupSID,int materialSID, int blastingNum,int blastingTotalNum, DateTime blastingBeginTime, DateTime blastingEndTime)
        //{
        //    Blasting blasting = new Blasting();
        //    blasting.EquipmentSID = equipmentSID;
        //    blasting.GroupSID = groupSID;
        //    blasting.MaterialSID = materialSID;
        //    blasting.BlastingNum = blastingNum;
        //    blasting.BlastingTotalNum = blastingTotalNum;
        //    blasting.BlastingBeginTime = blastingBeginTime;
        //    blasting.BlastingEndTime = blastingEndTime;

        //    int result = 0;
        //    try
        //    {
        //        result = DataFactory.UpdateEntity(blasting, "EquipmentSID,GroupSID,MaterialSID,BlastingNum,BlastingTotalNum,BlastingBeginTime,BlastingEndTime", "DG_Blasting_INS_MW");
        //    }
        //    catch (Exception)
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}

        //#endregion

        #region 查询抛丸最新SID
        public static int GetBlastingSID()
        {
            List<Blasting> blastings = null;
            Blasting blasting = new Blasting();

            try
            {
                blastings = DataFactory.FillEntities<Blasting>(blasting, "", "MW_BlastingSID_SEL");
            }
            catch (Exception)
            {
                blastings = new List<Blasting>();
            }
            return blastings[0].BlastingSID;
        }

        #endregion

        #region 更新抛丸信息
        public static int UpdBlasting(Blasting blasting)
        {
            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(blasting
                    , "BlastingSID,BlastingNum,BlastingTotalNum,BlastingEndTime", "MW_Blasting_UPD");
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
