using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;   //绘图
using System.Linq;      //操作内存数据的方式，查询数据库
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet.Siemens;        //调用库HslCommunication.Profinet.Siemens（开源的）

namespace HslCommunicationDemo
{
    public partial class FormLoad : Form
    {
        public FormLoad( )
        {
            InitializeComponent( );
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Hide( );
            System.Threading.Thread.Sleep( 200 );
            using (FormSiemens form = new FormSiemens( SiemensPLCS.S1200 ))     //新建窗口，调用了了当前对应的PLC型号的类库
            {
                form.ShowDialog( );
            }
            System.Threading.Thread.Sleep( 200 );       //将当前线程休眠200ms，留给加载通讯空间的时间
            Show( );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            Hide( );
            System.Threading.Thread.Sleep( 200 );
            using (FormSiemens form = new FormSiemens( SiemensPLCS.S1500 )) //新建窗口，调用了了当前对应的PLC型号的类库
            {
                form.ShowDialog( );
            }
            System.Threading.Thread.Sleep( 200 );   //将当前线程休眠200ms，留给加载通讯空间的时间
            Show( );
        }

        private void button3_Click( object sender, EventArgs e )
        {
            Hide( );
            System.Threading.Thread.Sleep( 200 );
            using (FormSiemens form = new FormSiemens( SiemensPLCS.S300 ))
            {
                form.ShowDialog( );
            }
            System.Threading.Thread.Sleep( 200 );
            Show( );
        }

        private void button5_Click( object sender, EventArgs e )
        {
            Hide( );
            System.Threading.Thread.Sleep( 200 );
            using (FormSiemens form = new FormSiemens( SiemensPLCS.S200Smart ))
            {
                form.ShowDialog( );
            }
            System.Threading.Thread.Sleep( 200 );
            Show( );
        }

 
        private void FormLoad_Load( object sender, EventArgs e )
        {
        }
//以下为功能实现

        //private void button30_Click( object sender, EventArgs e )
        //{
        //    Hide( );
        //    System.Threading.Thread.Sleep( 200 );
        //    using (FormTcpDebug form = new FormTcpDebug( ))
        //    {
        //        form.ShowDialog( );
        //    }
        //    System.Threading.Thread.Sleep( 200 );
        //    Show( );
        //}

        private void 使用帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("可选择西门子不同PLC系列进行测试\n读取的所有地址如果是来自DB块，则必须是非优化快\n提供TCP/IP连接测试工具");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = System.DateTime.Now;              // 获取系统当前时间
            toolStripStatusLabel1.Text = dateTime.ToLongDateString();  // 显示日期
            toolStripStatusLabel2.Text = dateTime.ToLongTimeString(); 	// 显示时间
        }

        private void 软件作者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2016052594 自动化 黎永熙完成\n本软件基于siemensTcpNet库实现操作功能\n需要满足Net4.5环境\n测试请使用西门子PLC\n");
        }
        //点击西门子图片，使用系统默认浏览器打开网页
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.industry.siemens.com.cn/topics/cn/zh/future-of-manufacturing/Pages/future-of-manufacturing.aspx");
        }
     //点击暨南大学图片，使用系统默认浏览器打开网页
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jnu.edu.cn/");
        }
    }
}
