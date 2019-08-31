using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;  
using System.Text;
using System.Windows.Forms;
using System.Threading;         //多线程
using HslCommunication;     //通讯库
using HslCommunication.Profinet;
using HslCommunication.Profinet.Siemens; //通讯库-西门子
using FuhuaMiddleware.Model;
using FuhuaMiddleware.DAL;
using System.Data.OleDb;    //数据库



namespace HslCommunicationDemo
{

    //构造函数

    public partial class FormSiemens : Form
    {
        public FormSiemens(SiemensPLCS siemensPLCS)
        {
            InitializeComponent();
            siemensTcpNet = new SiemensS7Net(siemensPLCS);
            timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += timer_Elapsed;

            //铁水转运线程
            thread1 = new Thread(ThreadReadServer1);
            thread1.IsBackground = true;
            thread1.Start();

            //异常检测线程
            thread9 = new Thread(ThreadReadServer9);
            thread9.IsBackground = true;
            thread9.Start();

        }

        private SiemensS7Net siemensTcpNet = null;

        private void FormSiemens_Load(object sender, EventArgs e)
        {
            panel2.Enabled = true;
        

        }

        //关闭窗口时把线程关闭
        private void FormSiemens_FormClosing(object sender, FormClosingEventArgs e)
        {
            isThreadRun = false;
        }

        /// <summary>
        /// 统一的，读取结果的数据解析，显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="address"></param>
        /// <param name="textBox"></param>
        private void readResultRender<T>(OperateResult<T> result, string address, TextBox textBox) //存储地点：TextBox textBox  ？
        {
            if (result.IsSuccess)
            {
                //将数据附加到/添加到文本框中  ；{Environment.NewLine}用来换行的
                textBox.AppendText(DateTime.Now.ToString("[HH:mm:ss] ") + $"[{address}] {result.Content}{Environment.NewLine}");  //result.Content是读取的信息存储           
            }
            else
            {
                MessageBox.Show(DateTime.Now.ToString("[HH:mm:ss] ") + $"[{address}] 读取失败{Environment.NewLine}原因：{result.ToMessageShowString()}");
            }
        }

        /// <summary>
        /// 统一的，数据写入的结果显示
        /// </summary>
        /// <param name="result"></param>
        /// <param name="address"></param>
        private void writeResultRender(OperateResult result, string address)
        {
            if (result.IsSuccess)
            {
                listBox1.Items.Add($"[{address}] 写入PLC成功");
            }
            else
            {
                MessageBox.Show(DateTime.Now.ToString("[HH:mm:ss] ") + $"[{address}] 写入失败{Environment.NewLine}原因：{result.ToMessageShowString()}");
            }
        }

        #region 与PLC建立连接/关闭操作

        private void button1_Click(object sender, EventArgs e)
        {
            // 连接

            siemensTcpNet.IpAddress = textBox1.Text;
            //尝试连接操作
            try
            {
                OperateResult connect = siemensTcpNet.ConnectServer();
                if (connect.IsSuccess)
                {
                    MessageBox.Show("连接成功！");
                    button2.Enabled = true;
                    button1.Enabled = false;
                    panel2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("连接失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 断开连接 ，带有窗口确定
            if (System.Windows.Forms.DialogResult.Yes != MessageBox.Show("是否确认断开PLC连接", "确认", MessageBoxButtons.YesNo))
                siemensTcpNet.ConnectClose();
            button2.Enabled = false;
            button1.Enabled = true;
            panel2.Enabled = false;
        }

        #endregion

        #region 单数据读取测试


        private void button_read_bool_Click(object sender, EventArgs e)
        {
            // 读取bool变量
            readResultRender(siemensTcpNet.ReadBool(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_byte_Click(object sender, EventArgs e)
        {
            // 读取byte变量
            readResultRender(siemensTcpNet.ReadByte(textBox3.Text), textBox3.Text, textBox4);
        }
        private void button_read_short_Click(object sender, EventArgs e)
        {
            // 读取short变量，就是标准的INT
            readResultRender(siemensTcpNet.ReadInt16(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_ushort_Click(object sender, EventArgs e)
        {
            // 读取ushort变量,就是标准的UInt
            readResultRender(siemensTcpNet.ReadUInt16(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_int_Click(object sender, EventArgs e)
        {
            // 读取int变量 ，就是Dint
            readResultRender(siemensTcpNet.ReadInt32(textBox3.Text), textBox3.Text, textBox4);
        }
        private void button_read_uint_Click(object sender, EventArgs e)
        {
            // 读取uint变量,就是UDint
            readResultRender(siemensTcpNet.ReadUInt32(textBox3.Text), textBox3.Text, textBox4);
        }
        private void button_read_long_Click(object sender, EventArgs e)
        {
            // 读取long变量,就是标准LInt
            readResultRender(siemensTcpNet.ReadInt64(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_ulong_Click(object sender, EventArgs e)
        {
            // 读取ulong变量，就是标准ULInt
            readResultRender(siemensTcpNet.ReadUInt64(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_float_Click(object sender, EventArgs e)
        {
            // 读取float变量,就是标准REAL
            readResultRender(siemensTcpNet.ReadFloat(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_double_Click(object sender, EventArgs e)
        {
            // 读取double变量,就是标准LREAL
            readResultRender(siemensTcpNet.ReadDouble(textBox3.Text), textBox3.Text, textBox4);
        }

        private void button_read_string_Click(object sender, EventArgs e)
        {
            // 读取字符串
            readResultRender(siemensTcpNet.ReadString(textBox3.Text, ushort.Parse(textBox5.Text)), textBox3.Text, textBox4);
        }


        #endregion

        #region 单数据写入测试

        //以下为各个数据类型写入的对应按钮事件函数，具体操作数据类型看上面就好

        private void button24_Click(object sender, EventArgs e)
        {
            // bool写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, bool.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            // byte写入
            try
            {
                byte[] buffer = new byte[500];
                for (int i = 0; i < 500; i++)
                {
                    buffer[i] = (byte)i;
                }
                writeResultRender(siemensTcpNet.Write(textBox8.Text, buffer), textBox8.Text);
                writeResultRender(siemensTcpNet.Write(textBox8.Text, byte.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            // short写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, short.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            // ushort写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, ushort.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button20_Click(object sender, EventArgs e)
        {
            // int写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, int.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // uint写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, uint.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // long写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, long.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            // ulong写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, ulong.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            // float写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, float.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // double写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, double.Parse(textBox7.Text)), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button14_Click(object sender, EventArgs e)
        {
            // string写入
            try
            {
                writeResultRender(siemensTcpNet.Write(textBox8.Text, textBox7.Text), textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        #endregion

        #region 操作示例
        /*
         读取操作，这里的M100可以替换成I100,Q100,DB20.100效果时一样的
   bool M100_7 = siemensTcpNet.ReadBool( "M100.7" ).Content;  // 读取M100.7是否通断，注意M100.0等同于M100
   byte byte_M100 = siemensTcpNet.ReadByte( "M100" ).Content; // 读取M100的值
   short short_M100 = siemensTcpNet.ReadInt16( "M100" ).Content; // 读取M100-M101组成的字
   ushort ushort_M100 = siemensTcpNet.ReadUInt16( "M100" ).Content; // 读取M100-M101组成的无符号的值
   int int_M100 = siemensTcpNet.ReadInt32( "M100" ).Content;         // 读取M100-M103组成的有符号的数据
   uint uint_M100 = siemensTcpNet.ReadUInt32( "M100" ).Content;      // 读取M100-M103组成的无符号的值
   float float_M100 = siemensTcpNet.ReadFloat( "M100" ).Content;   // 读取M100-M103组成的单精度值
   long long_M100 = siemensTcpNet.ReadInt64( "M100" ).Content;      // 读取M100-M107组成的大数据值
   ulong ulong_M100 = siemensTcpNet.ReadUInt64( "M100" ).Content;   // 读取M100-M107组成的无符号大数据
   double double_M100 = siemensTcpNet.ReadDouble( "M100" ).Content; // 读取M100-M107组成的双精度值
   string str_M100 = siemensTcpNet.ReadString( "M100", 10 ).Content;// 读取M100-M109组成的ASCII字符串数据
        */

        /*
         / 写入操作，这里的M100可以替换成I100,Q100,DB20.100效果时一样的
    siemensTcpNet.Write( "M100.7", true );                // 写位，注意M100.0等同于M100
    siemensTcpNet.Write( "M100", (byte)0x33 );            // 写单个字节
    siemensTcpNet.Write( "M100", (short)12345 );          // 写双字节有符号   --int
    siemensTcpNet.Write( "M100", (ushort)45678 );         // 写双字节无符号    --  uint
    siemensTcpNet.Write( "M100", 123456789 );             // 写双字有符号 --  dint
    siemensTcpNet.Write( "M100", (uint)3456789123 );      // 写双字无符号---udint
    siemensTcpNet.Write( "M100", 123.456f );              // 写单精度   ---real
    siemensTcpNet.Write( "M100", 1234556434534545L );     // 写大整数有符号---lint
    siemensTcpNet.Write( "M100", 523434234234343UL );     // 写大整数无符号---ulint
    siemensTcpNet.Write( "M100", 123.456d );              // 写双精度---lreal
    siemensTcpNet.Write( "M100", "K123456789" );// 写ASCII字符串----string
        */
        #endregion

        #region 批量读取测试

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                OperateResult<byte[]> read = siemensTcpNet.Read(textBox6.Text, ushort.Parse(textBox9.Text));
                if (read.IsSuccess)
                {   //这里是在文本框中刷新数据，不是附加新的信息
                    textBox10.Text = "结果：" + HslCommunication.BasicFramework.SoftBasic.ByteToHexString(read.Content);
                }
                else
                {
                    MessageBox.Show("读取失败：" + read.ToMessageShowString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取失败：" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 读取订货号
            OperateResult<string> read = siemensTcpNet.ReadOrderNumber();
            if (read.IsSuccess)
            {
                textBox10.Text = "订货号：" + read.Content;
            }
            else
            {
                MessageBox.Show("读取失败：" + read.ToMessageShowString());
            }
        }

        #endregion

        #region 数据采集

        private Thread thread1, thread2, thread3, thread4, thread5, thread6 ,thread7 ,thread8,thread9 = null;
        private bool isThreadRun = false;          // 用来标记线程的运行状态，初始未点击启动按钮时不开启  
        System.Timers.Timer timer = null;
        private int day = 0;
        private bool ironWriteSQL = false;
        
        
        //时延函数，启动线程
        void timer_Elapsed(object sender,System.Timers.ElapsedEventArgs e)
        {

            thread2 = new Thread(ThreadReadServer2);
            thread2.IsBackground = true;
            thread2.Start();
            thread3 = new Thread(ThreadReadServer3);
            thread3.IsBackground = true;
            thread3.Start();
            thread4 = new Thread(ThreadReadServer4);
            thread4.IsBackground = true;
            thread4.Start();
            thread5 = new Thread(ThreadReadServer5);
            thread5.IsBackground = true;
            thread5.Start();
            thread6 = new Thread(ThreadReadServer6);
            thread6.IsBackground = true;
            thread6.Start();
            thread7 = new Thread(ThreadReadServer7);
            thread7.IsBackground = true;
            thread7.Start();
            if(DateTime.Today.Day != day)    //每天执行一次
            {
                if(DateTime.Now.Hour == 23)     //23点执行
                {
                    thread8 = new Thread(ThreadReadServer8);
                    thread8.IsBackground = true;
                    thread8.Start();
                }
            }
   
        }
   
        //铁水转运线程
        private void ThreadReadServer1()
        {
            int result = 0;
            IronMeltTrans ironMeltTrans = new IronMeltTrans();

            try
            {
                //持续读取                                                                   
                int groupinfo = siemensTcpNet.ReadInt16("DB1.540").Content;   //班次
                if (groupinfo == 2) { ironMeltTrans.GroupSID = 6; }
                if (groupinfo == 3) { ironMeltTrans.GroupSID = 5; }
                ironMeltTrans.ProductionScheduleSID = ProductionScheduleDAL.GetProductionScheduleSID(); //订单
                ironMeltTrans.EmployeeSID = siemensTcpNet.ReadInt16("DB1.540").Content;   //员工（测试）
                ironMeltTrans.EquipmentSID = siemensTcpNet.ReadInt16("DB1.4").Content;//炉号
                ironMeltTrans.ElectricFurnaceCount = siemensTcpNet.ReadInt16("DB1.6").Content;//炉次
                ironMeltTrans.IronMeltTransNum = siemensTcpNet.ReadInt16("DB1.8").Content;//包次
                ironMeltTrans.IronMeltTransWeight = siemensTcpNet.ReadInt16("DB1.0").Content;//铁水重量 
                ironMeltTrans.IronMeltTransTime = DateTime.Now;//铁水转运时间                    
                if (siemensTcpNet.ReadBool("DB1.552").Content == true) ironMeltTrans.BackWeight = siemensTcpNet.ReadInt16("DB1.550").Content;    //行车吊装
                
                //根据标志上升沿写入数据库
                if(Convert.ToInt16(siemensTcpNet.ReadBool("DB1.826.4").Content) - Convert.ToInt16(ironWriteSQL) == 1)
                { 
                    result = IronMeltTransDAL.AddIronMeltTransInfo(ironMeltTrans);

                    if (result == 1)
                    {
                       
                        listBox2.Items.Add("铁水转运写入数据库成功  " + DateTime.Now);
                        int result2 = 0;
                        IronMeltSuppMaterial ironMeltSuppMaterial = new IronMeltSuppMaterial();
                        int groupSID;
                        ironMeltSuppMaterial.IronMeltTranSID = IronMeltTransDAL.GetIronMeltTransSID(out groupSID);
                        ironMeltSuppMaterial.GroupSID = groupSID;
                        ironMeltSuppMaterial.MaterialSID = 77; //暂定这种孕育剂
                        ironMeltSuppMaterial.IronMeltSuppMaterialWeight = siemensTcpNet.ReadInt16("DB1.2").Content;//加料(孕育剂)重量
                        ironMeltSuppMaterial.IronMeltSuppMaterialTime = DateTime.Now;
                        try
                        {
                            result2 = IronMeltSuppMaterialDAL.AddIronMeltSuppMaterial(ironMeltSuppMaterial);
                            if (result2 == 1)
                            {
                                listBox2.Items.Add("铁水转运加料写入数据库成功  " + DateTime.Now);
                            }
                            else
                            {
                                listBox2.Items.Add("铁水转运加料写入数据库失败  " + DateTime.Now);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("铁水转运加料出现问题：" + ex.Message);
                        }

                    }
                    else
                    {
                        listBox2.Items.Add("铁水转运写入数据库失败  " + DateTime.Now);
                    }
                }

                ironWriteSQL = siemensTcpNet.ReadBool("DB1.826.4").Content;//更新标志


            }
            catch (Exception ex)
            {
                MessageBox.Show("铁水转运出现问题：" + ex.Message);
            }
  

        }

        //1号打磨线程
        private void ThreadReadServer2()
        {
            Polish polish = new Polish();
            int result;
                        try
                        {
                            polish.PolishSID = PolishDAL.GetPolishSID(1);
                            polish.PolishNum = siemensTcpNet.ReadInt32("DB11.4").Content;//当班次当产品打磨件数 打磨机1
                            polish.PolishTotalNum = siemensTcpNet.ReadInt32("DB11.8").Content;//总件数
                            polish.PolishEndTime = DateTime.Now;
          
                            
                            result = PolishDAL.UpdPolish(polish);
                            if(result == 1)
                            {
                                listBox2.Items.Add("1号打磨写入数据库成功  " + DateTime.Now);
                            }
                             else
                            {
                                listBox2.Items.Add("1号打磨写入数据库失败  " + DateTime.Now);
                            }
                       
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("1号打磨出现问题：" + ex.Message);
                        }
                    
            this.thread2.Abort();          
        }

        //2号打磨线程
        private void ThreadReadServer3()
        {
            Polish polish = new Polish();
            int result;
            try
            {
                polish.PolishSID = PolishDAL.GetPolishSID(2);
 
                polish.PolishNum = siemensTcpNet.ReadInt32("DB16.4").Content;//当班次当产品打磨件数 打磨机2
                polish.PolishTotalNum = siemensTcpNet.ReadInt32("DB16.8").Content;//总件数
                polish.PolishEndTime = DateTime.Now;

                result = PolishDAL.UpdPolish(polish);
                if (result == 1)
                {
                    listBox2.Items.Add("2号打磨写入数据库成功  " + DateTime.Now);
                }
                else
                {
                    listBox2.Items.Add("2号打磨写入数据库失败  " + DateTime.Now);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("2号打磨出现问题：" + ex.Message);
            }

            this.thread3.Abort();
        }

        //抛丸线程
        private void ThreadReadServer4()
        {
            Blasting blasting = new Blasting();
            int result;
                #region  注释
                //持续读取

                //GroupSID = siemensTcpNet.ReadInt16("DB1.508").Content;   //班次

                ////物料查询         
                //if (Convert.ToInt16(siemensTcpNet.ReadBool("DB1.242").Content) - Convert.ToInt16(MaterialIDSearchSignal) != 0)
                //{
                //    try
                //    {
                //        materials = MaterialDAL.LoadMaterals("");                   //查询物料料号和SID                     

                //        writeResultRender(siemensTcpNet.Write("DB1.238", materials[0].MaterialID + "/" + materials[0].MaterialSID), "DB1.238");   //物料信息写入HMI
                //        writeResultRender(siemensTcpNet.Write("DB1.260", materials[1].MaterialID + "/" + materials[1].MaterialSID), "DB1.260");
                //        writeResultRender(siemensTcpNet.Write("DB1.282", materials[2].MaterialID + "/" + materials[2].MaterialSID), "DB1.282");
                //        writeResultRender(siemensTcpNet.Write("DB1.304", materials[3].MaterialID + "/" + materials[3].MaterialSID), "DB1.304");
                //        writeResultRender(siemensTcpNet.Write("DB1.326", materials[4].MaterialID + "/" + materials[4].MaterialSID), "DB1.326");
                //        writeResultRender(siemensTcpNet.Write("DB1.348", materials[5].MaterialID + "/" + materials[5].MaterialSID), "DB1.348");
                //        writeResultRender(siemensTcpNet.Write("DB1.370", materials[6].MaterialID + "/" + materials[6].MaterialSID), "DB1.370");
                //        writeResultRender(siemensTcpNet.Write("DB1.392", materials[7].MaterialID + "/" + materials[7].MaterialSID), "DB1.392");
                //        writeResultRender(siemensTcpNet.Write("DB1.414", materials[8].MaterialID + "/" + materials[8].MaterialSID), "DB1.414");
                //        writeResultRender(siemensTcpNet.Write("DB1.436", materials[9].MaterialID + "/" + materials[9].MaterialSID), "DB1.436");
                //    }
                //    catch (Exception ex) { }
                //}
                //MaterialIDSearchSignal = siemensTcpNet.ReadBool("DB1.242").Content;    //更新物料查询标志

                ////产品查询         
                //if (Convert.ToInt16(siemensTcpNet.ReadBool("DB1.242").Content) - Convert.ToInt16(MaterialIDSearchSignal) != 0)
                //{
                //    try
                //    {
                //        materials2 = MaterialDAL.LoadMaterals("");                   //查询产品料号和SID                     

                //        writeResultRender(siemensTcpNet.Write("DB1.238", materials2[0].MaterialID + "/" + materials2[0].MaterialSID), "DB1.238");   //产品信息写入HMI
                //        writeResultRender(siemensTcpNet.Write("DB1.260", materials2[1].MaterialID + "/" + materials2[1].MaterialSID), "DB1.260");
                //        writeResultRender(siemensTcpNet.Write("DB1.282", materials2[2].MaterialID + "/" + materials2[2].MaterialSID), "DB1.282");
                //        writeResultRender(siemensTcpNet.Write("DB1.304", materials2[3].MaterialID + "/" + materials2[3].MaterialSID), "DB1.304");
                //        writeResultRender(siemensTcpNet.Write("DB1.326", materials2[4].MaterialID + "/" + materials2[4].MaterialSID), "DB1.326");
                //        writeResultRender(siemensTcpNet.Write("DB1.348", materials2[5].MaterialID + "/" + materials2[5].MaterialSID), "DB1.348");
                //        writeResultRender(siemensTcpNet.Write("DB1.370", materials2[6].MaterialID + "/" + materials2[6].MaterialSID), "DB1.370");
                //        writeResultRender(siemensTcpNet.Write("DB1.392", materials2[7].MaterialID + "/" + materials2[7].MaterialSID), "DB1.392");
                //        writeResultRender(siemensTcpNet.Write("DB1.414", materials2[8].MaterialID + "/" + materials2[8].MaterialSID), "DB1.414");
                //        writeResultRender(siemensTcpNet.Write("DB1.436", materials2[9].MaterialID + "/" + materials2[9].MaterialSID), "DB1.436");
                //    }
                //    catch (Exception ex) { }
                //}
                //MaterialIDSearchSignal = siemensTcpNet.ReadBool("DB1.242").Content;    //更新产品查询标志

                ////故障查询                 
                //if (Convert.ToInt16(siemensTcpNet.ReadBool("DB1.726.1").Content) - Convert.ToInt16(ProblemSearchSignal) != 0)
                //{
                //    try
                //    {
                //        int ProblemPage = siemensTcpNet.ReadInt16("DB1.734").Content;
                //        problemCates = ProblemCateDAL.LoadProblemCates(1);
                //        this.listBox1.Items.Add(problemCates[0].ProblemCateDesc);

                //        if (ProblemPage == 0)
                //        {
                //            writeResultRender(siemensTcpNet.Write("DB1.510", problemCates[0].ProblemCateDesc + "/" + problemCates[0].ProblemCateSID), "DB1.510");
                //            writeResultRender(siemensTcpNet.Write("DB1.552", problemCates[1].ProblemCateDesc + "/" + problemCates[1].ProblemCateSID), "DB1.552");
                //            writeResultRender(siemensTcpNet.Write("DB1.594", problemCates[2].ProblemCateDesc + "/" + problemCates[2].ProblemCateSID), "DB1.594");
                //            writeResultRender(siemensTcpNet.Write("DB1.636", problemCates[3].ProblemCateDesc + "/" + problemCates[3].ProblemCateSID), "DB1.636");
                //            writeResultRender(siemensTcpNet.Write("DB1.678", problemCates[4].ProblemCateDesc + "/" + problemCates[4].ProblemCateSID), "DB1.678");
                //        }
                //        if (ProblemPage == 1)
                //        {
                //            writeResultRender(siemensTcpNet.Write("DB1.510", problemCates[5].ProblemCateDesc), "DB1.510");
                //        }
                //    }
                //    catch (Exception ex) { }
                //}
                //ProblemSearchSignal = siemensTcpNet.ReadBool("DB1.726.1").Content;     //更新故障查询标志

                //EquipmentSID = 74;//抛丸机



                //if (siemensTcpNet.ReadBool("抛丸开始标志").Content == true)
                //{
                //    BlastingBeginTime = DateTime.Now;
                //}
                //if (siemensTcpNet.ReadBool("抛丸结束标志").Content == true)
                //{
                //    BlastingEndTime = DateTime.Now;
                //}
                #endregion
     
                    try
                    {
                        blasting.BlastingSID = BlastingDAL.GetBlastingSID();
                        blasting.BlastingNum = siemensTcpNet.ReadInt16("DB1.2").Content;      //当班次当产品抛丸件数
                        blasting.BlastingTotalNum = siemensTcpNet.ReadInt16("DB1.2").Content;       //总件数
                        blasting.BlastingEndTime = DateTime.Now;
                        result = BlastingDAL.UpdBlasting(blasting);
                    if (result == 1)
                    {
                        listBox2.Items.Add("抛丸写入数据库成功  " + DateTime.Now);
                    }
                    else
                    {
                        listBox2.Items.Add("抛丸写入数据库失败  " + DateTime.Now);
                    }
            }
                    catch (Exception ex)
                    {
                        MessageBox.Show("抛丸出现问题：" + ex.Message);
                    }
                
            this.thread4.Abort();
        }

        //喷漆线程
        private void ThreadReadServer5()
        {        
            Paint paint = new Paint();
            int result;
 
    
                    try
                    {
                        paint.PaintSID = PaintDAL.GetPaintSID();
                        paint.PaintNum = siemensTcpNet.ReadInt16("DB1.2").Content;      //当班次当产品抛丸件数
                        paint.PaintTotalNum = siemensTcpNet.ReadInt16("DB1.2").Content;       //总件数
                        paint.PaintEndTime = DateTime.Now;                                       
                        result = PaintDAL.UpdPaint(paint);
                    if (result == 1)
                    {
                        listBox2.Items.Add("喷漆写入数据库成功  " + DateTime.Now);
                    }
                    else
                    {
                        listBox2.Items.Add("喷漆写入数据库失败  " + DateTime.Now);
                    }
                    }
                    catch (Exception ex) { MessageBox.Show("喷漆出现问题：" + ex.Message); }
                
            this.thread5.Abort();
        }

        //包装线程
        private void ThreadReadServer6()
        {
            Pack pack = new Pack();
            int result;
                  
                        try
                        {
                            pack.PackSID = PackDAL.GetPackSID();
                            pack.PackNum = siemensTcpNet.ReadInt32("DB5.4").Content;      //当班次当产品包装件数
                            pack.PackTotalNum = siemensTcpNet.ReadFloat("DB5.0").Content;       //总件数
                            

                            pack.PackEndTime = DateTime.Now;
                            result = PackDAL.UpdPack(pack);
                            if (result == 1)
                            {
                                listBox2.Items.Add("包装写入数据库成功  " + DateTime.Now);
                            }
                            else
                            {
                                listBox2.Items.Add("包装写入数据库失败  " + DateTime.Now);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("包装出现问题：" + ex.Message);
                        }                                  
           
            this.thread6.Abort();
        }

        //造型线程
        private void ThreadReadServer7()
        {
            Modelling modelling = new Modelling();
            int result;
             

                    try
                    {

                            modelling.ModellingSID = ModellingDAL.GetModellingSID();
                            modelling.ModellingBoxNum = siemensTcpNet.ReadInt16("DB19.4").Content;      //当班次当产品造型箱数
                            modelling.ModellingBadBoxNum = siemensTcpNet.ReadInt16("DB21.64").Content;       //当班次当产品造型坏箱数
                            modelling.ModellingEmptyBoxNum = siemensTcpNet.ReadInt16("DB19.2").Content;       //当班次当产品造型空箱数
                            modelling.ModellingEndTime = DateTime.Now;

                            result = ModellingDAL.UpdModelling(modelling);
                            if (result == 1)
                            {
                                listBox2.Items.Add("造型写入数据库成功  " + DateTime.Now);
                            }
                            else
                            {
                                listBox2.Items.Add("造型写入数据库失败  " + DateTime.Now);
                            }
                    }
                    catch (Exception ex) { MessageBox.Show("造型出现问题：" + ex.Message); }
                
            this.thread7.Abort();
        }
      
        //电能线程
        private void ThreadReadServer8()
        {
            int result = 0;
            int addr = 0;
            string address = null;
            ElecPower elecPower = new ElecPower();

            //1-25号从站 DB13.X

            for (int i = 1; i <= 25; i++)
            {               
                addr = 8 + 124*(i-1) + DateTime.Now.Day * 4;  //从站地址
                address = "DB13." + addr; //总地址
                elecPower.ElecPowerData = siemensTcpNet.ReadInt32(address).Content; //获取电能数据
                elecPower.ElecMeter = i;   //获取电表号
                elecPower.ElecDate = DateTime.Now;  //获取日期
                try
                {
                    result = 0;
                    result = ElecPowerDAL.AddElecPower(elecPower);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("电表出现问题：" + ex.Message);
                }           
                if (result == 1)
                {
                    listBox2.Items.Add("电能写入数据库成功  " + DateTime.Now);
                }
                else
                {
                    listBox2.Items.Add("电能写入数据库失败  " + DateTime.Now);
                }
            }

            //26-30号从站 DB14.X

            for(int i = 1; i <= 5; i++)
            {
                addr = 8 + 124 * (i - 1) + DateTime.Now.Day * 4;  //从站地址
                address = "DB14." + addr;
                elecPower.ElecPowerData = siemensTcpNet.ReadInt32(address).Content; //获取电能数据
                elecPower.ElecMeter = i;   //获取电表号
                elecPower.ElecDate = DateTime.Now;  //获取日期
                try
                {
                    result = 0;
                    result = ElecPowerDAL.AddElecPower(elecPower);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("电表出现问题：" + ex.Message);
                }
                if (result == 1)
                {
                    listBox2.Items.Add("电能写入数据库成功  " + DateTime.Now);
                }
                else
                {
                    listBox2.Items.Add("电能写入数据库失败  " + DateTime.Now);
                }
            }

            day = DateTime.Today.Day;
            this.thread8.Abort();
        }

        //通讯异常检测线程
        private void ThreadReadServer9()
        {
            //定义错误列表，传入地址时要注意顺序           
            var errors = ReadBoolList(new List<string>() {
                #region 读取地址
                //铁水
                "M10.1",
                "M10.3",
                "M10.5",
                "M10.7",
                "M11.1",
                "M11.3",
                "M11.5",
                "M12.1",
                //造型机
                "M14.0",
                "M14.3",
                //造型线
                "M15.0",
                "M15.2",
                //抛丸
                "M21.1",
                "M21.3",
                //打磨1
                "M20.1",
                "M20.5",
                //打磨2
                "M20.3",
                "M20.7",
                //喷漆
                "M23.0",
                //包装
                "M24.1",
                "M24.3"
                #endregion
            });
            List<int> nums = new List<int>(); //报错的号码列表
                //遍历errors，看哪几个报错
                errors.ForEach(error =>
                {                
                    if (error == true)
                    {
                        int index = errors.IndexOf(error);
                        if (index <= 7)  nums.Add(0);
                        if (index == 8 || index == 9) nums.Add(1);
                        if (index == 10 || index == 11) nums.Add(2);
                        if (index == 12 || index == 13) nums.Add(3);
                        if (index == 14 || index == 15) nums.Add(4);
                        if (index == 16 || index == 17) nums.Add(5);
                        if (index == 18) nums.Add(6);
                        if (index == 19 || index == 20) nums.Add(7);
                    }
                });

            //定义标签列表，顺序很重要，要与地址顺序对应
            var labels = new List<Label>() { TS, ZXJ, ZXX, PW, DM1, DM2, PQ, BZ };

            labels.ForEach(label => 
            {
                if (nums.Exists(num=> num == labels.IndexOf(label)))    //如果label索引在报错列表中
                typeof(Label).GetProperty("Visible").SetValue(label, true, null);   //报错
                else typeof(Label).GetProperty("Visible").SetValue(label, false, null); //不在则取消错误
            });

        }

        //传入地址列表读取布尔值列表
        private List<bool> ReadBoolList(List<string> adds)
        {
            List<bool> results = new List<bool>();
            adds.ForEach(add => results.Add(siemensTcpNet.ReadBool(add).Content));
            return results;
        }

        //传入地址列表读取int16列表
        private List<int> ReadInt16List(List<string> adds)
        {
            List<int> results = new List<int>();
            adds.ForEach(add => results.Add(siemensTcpNet.ReadInt16(add).Content));
            return results;
        }

        //传入地址列表读取int32列表
        private List<int> ReadInt32List(List<string> adds)
        {
            List<int> results = new List<int>();
            adds.ForEach(add => results.Add(siemensTcpNet.ReadInt32(add).Content));
            return results;
        }



        //铁水转运线程旧
        //private void ThreadReadServer1()
        //{
        //    int result;
        //    IronMeltTrans ironMeltTrans = new IronMeltTrans();
        //    int MaterialIDSearchSignal = 0, ProblemSearchSignal = 0;
        //    int ProblemCateSID, MaterialSID, IronMeltSuppMaterialWeight;
        //    string ProductionProblemRemark, ProductionProblemBeginTime, ProductionProblemEndTime, ProblemCateDesc, MaterialID;
        //    DateTime IronMeltTransTime, IronMeltSuppMaterialTime;
        //    List<Material> materials = null;
        //    List<ProblemCate> problemCates = null;

        //    try
        //    {
        //        //持续读取                                                                   
        //        int groupinfo = siemensTcpNet.ReadInt16("DB1.540").Content;   //班次
        //        if (groupinfo == 2) { ironMeltTrans.GroupSID = 6; }
        //        if (groupinfo == 3) { ironMeltTrans.GroupSID = 5; }

        //        ironMeltTrans.EmployeeSID = siemensTcpNet.ReadInt16("DB1.540").Content;   //员工（测试）

        //        ironMeltTrans.EquipmentSID = siemensTcpNet.ReadInt16("DB1.4").Content;//炉号
        //        ironMeltTrans.ElectricFurnaceCount = siemensTcpNet.ReadInt16("DB1.6").Content;//炉次
        //        ironMeltTrans.IronMeltTransNum = siemensTcpNet.ReadInt16("DB1.8").Content;//包次
        //        ironMeltTrans.IronMeltTransWeight = siemensTcpNet.ReadInt16("DB1.0").Content;//铁水重量   

        //        bool HangCheSignal = siemensTcpNet.ReadBool("DB1.552").Content;      //行车吊装写入标志
        //        if (HangCheSignal == true)
        //        {
        //            ironMeltTrans.BackWeight = siemensTcpNet.ReadInt16("DB1.550").Content;    //回炉铁水重量
        //        }

        //        IronMeltTransTime = DateTime.Now;//铁水转运时间

        //        //物料查询         
        //        if (siemensTcpNet.ReadInt16("DB1.830").Content - MaterialIDSearchSignal != 0)
        //        {
        //            try
        //            {
        //                materials = MaterialDAL.LoadMaterals("YYJ");                   //查询料号                     

        //                writeResultRender(siemensTcpNet.Write("DB1.312", materials[0].MaterialID + "/" + materials[0].MaterialSID), "DB1.312");   //物料信息写入HMI
        //                writeResultRender(siemensTcpNet.Write("DB1.334", materials[1].MaterialID + "/" + materials[1].MaterialSID), "DB1.334");
        //                writeResultRender(siemensTcpNet.Write("DB1.356", materials[2].MaterialID + "/" + materials[2].MaterialSID), "DB1.356");
        //                writeResultRender(siemensTcpNet.Write("DB1.378", materials[3].MaterialID + "/" + materials[3].MaterialSID), "DB1.378");
        //                writeResultRender(siemensTcpNet.Write("DB1.400", materials[4].MaterialID + "/" + materials[4].MaterialSID), "DB1.400");
        //                writeResultRender(siemensTcpNet.Write("DB1.422", materials[5].MaterialID + "/" + materials[5].MaterialSID), "DB1.422");
        //                writeResultRender(siemensTcpNet.Write("DB1.444", materials[6].MaterialID + "/" + materials[6].MaterialSID), "DB1.444");
        //                writeResultRender(siemensTcpNet.Write("DB1.466", materials[7].MaterialID + "/" + materials[7].MaterialSID), "DB1.466");
        //                writeResultRender(siemensTcpNet.Write("DB1.488", materials[8].MaterialID + "/" + materials[8].MaterialSID), "DB1.488");
        //                writeResultRender(siemensTcpNet.Write("DB1.510", materials[9].MaterialID + "/" + materials[9].MaterialSID), "DB1.510");
        //            }
        //            catch (Exception ex) { }
        //        }
        //        MaterialIDSearchSignal = siemensTcpNet.ReadInt16("DB1.830").Content;    //更新物料查询标志

        //        IronMeltSuppMaterialWeight = siemensTcpNet.ReadInt32("地址名").Content;//加料(孕育剂)重量

        //        //故障查询                 
        //        if (siemensTcpNet.ReadInt16("DB1.828").Content - ProblemSearchSignal != 0)
        //        {
        //            try
        //            {
        //                int ProblemPage = siemensTcpNet.ReadInt16("DB1.776").Content;
        //                problemCates = ProblemCateDAL.LoadProblemCates(1);
        //                this.listBox1.Items.Add(problemCates[0].ProblemCateDesc);

        //                if (ProblemPage == 0)
        //                {
        //                    writeResultRender(siemensTcpNet.Write("DB1.564", problemCates[0].ProblemCateDesc + "/" + problemCates[0].ProblemCateSID), "DB1.564");
        //                    writeResultRender(siemensTcpNet.Write("DB1.606", problemCates[1].ProblemCateDesc + "/" + problemCates[1].ProblemCateSID), "DB1.606");
        //                    writeResultRender(siemensTcpNet.Write("DB1.648", problemCates[2].ProblemCateDesc + "/" + problemCates[2].ProblemCateSID), "DB1.648");
        //                    writeResultRender(siemensTcpNet.Write("DB1.690", problemCates[3].ProblemCateDesc + "/" + problemCates[3].ProblemCateSID), "DB1.690");
        //                    writeResultRender(siemensTcpNet.Write("DB1.732", problemCates[4].ProblemCateDesc + "/" + problemCates[4].ProblemCateSID), "DB1.732");
        //                }
        //                if (ProblemPage == 1)
        //                {
        //                    writeResultRender(siemensTcpNet.Write("DB1.564", problemCates[5].ProblemCateDesc), "DB1.564");
        //                }
        //            }
        //            catch (Exception ex) { }
        //        }
        //        ProblemSearchSignal = siemensTcpNet.ReadInt16("DB1.828").Content;     //更新故障查询标志

        //        bool ProblemStopSignal = siemensTcpNet.ReadBool("地址名").Content;    //故障停机标志，写入故障类别，故障开始时间，故障结束时间，恢复生产时间
        //        if (ProblemStopSignal == true)
        //        {
        //            string Ftime = DateTime.Now.ToString("[HH:mm:ss] ");

        //            bool RepairStopSignal = siemensTcpNet.ReadBool("地址名").Content;
        //            if (RepairStopSignal == true)
        //            {
        //                string FRtime = DateTime.Now.ToString("[HH:mm:ss] ");
        //            }

        //            bool 恢复生产读取标志 = siemensTcpNet.ReadBool("地址名").Content;
        //            if (恢复生产读取标志 == true)
        //            {
        //                string PRtime = DateTime.Now.ToString("[HH:mm:ss] ");
        //                string 停机原因 = siemensTcpNet.ReadString("地址", 40).Content;
        //                //存入数据库“ProductionProblem	”的“ProductionProblemRemark”
        //            }

        //        }


        //        IronMeltSuppMaterialTime = DateTime.Now;//铁水转运加料时间

        //        result = IronMeltTransDAL.AddIronMeltTransInfo(ironMeltTrans);
        //        if (result == 1)
        //        {
        //            listBox2.Items.Add("铁水转运写入数据库成功  " + DateTime.Now);
        //        }
        //        else
        //        {
        //            listBox2.Items.Add("铁水转运写入数据库失败  " + DateTime.Now);
        //        }
        //        //    IronMeltSuppMaterialDAL.AddIronMeltSuppMaterialInfo(GroupSID,,MaterialSID, IronMeltTransWeight, IronMeltSuppMaterialTime);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("铁水转运出现问题：" + ex.Message);
        //    }

        //}

        #endregion
    }
}







