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
using System.Media;
namespace shooter
{
    public partial class winning : Form
    {
        Thread GG;
        public winning()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            GG = new Thread(open);
            GG.SetApartmentState(ApartmentState.STA);
            GG.Start();

        }

        private void open() {
            Application.Run(new Main());
        }
    }
}
