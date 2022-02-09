using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace shooter
{
    public partial class LevelGG : Form
    {
        Thread GameLevelStart;
        int level = 1,Z_num=5,Health_num=6,towin=25;

        public LevelGG()
        {
            InitializeComponent();
            
        }
     

        public void setlevel(int x,int z,int h,int w) {
            level = x;
            label1.Text = "LEVEL :"+level;
            
            Z_num = z + 2;
            Health_num = h -2;
            towin = w + 15 ;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            GameLevelStart = new Thread(o);
            GameLevelStart.SetApartmentState(ApartmentState.STA);
            GameLevelStart.Start();
       
        }
        private void o() {
            Form1 play = new Form1();
            
            play.SetDifficulty(level, Z_num, Health_num, towin);
            play.mainLevel_theme();
            Application.Run(play) ;
         
        }
    }
}
