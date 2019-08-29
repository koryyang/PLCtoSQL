using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XORM;
using FuhuaMiddleware.Model;

namespace FuhuaMiddleware.DAL
{
    class PaintDAL
    {
        #region 静态实例
        private static volatile PaintDAL instance = null;
        private static object lockHelper = new object();

        public static PaintDAL GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new PaintDAL();
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

        #region 查询喷漆最新SID
        public static int  GetPaintSID( )
        {
            List<Paint> paints = null;
            Paint paint = new Paint();
            try
            {
                paints = DataFactory.FillEntities<Paint>(paint, "", "MW_PaintSID_SEL");
            }
            catch (Exception)
            {
                paints = new List<Paint>();
            }
            return paints[0].PaintSID;
        }

        #endregion

        #region 更新喷漆信息
        public static int UpdPaint(Paint paint)
        {

            int result = 0;
            try
            {
                result = DataFactory.UpdateEntity(paint
                    , "PaintSID,PaintNum,PaintTotalNum,PaintEndTime", "MW_Paint_UPD");
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
