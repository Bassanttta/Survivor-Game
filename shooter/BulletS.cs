using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace shooter
{
    class BulletS
    {
        public string bullet_direction;
        public int b_left,b_top;
        private int speed = 25;
        private PictureBox Bullet = new PictureBox();
        private Timer Bullet_event = new Timer();

        public void Create_Bullets(Form f) {
            Bullet.BackColor = Color.Yellow;
            Bullet.Size = new Size(6, 6);
            Bullet.Tag ="bullet"; 
            Bullet.Left = b_left;
            Bullet.Top = b_top;
            Bullet.BringToFront();
            f.Controls.Add(Bullet);

            Bullet_event.Interval = 30;
            Bullet_event.Tick += new EventHandler(Bullet_shoot_event);
            Bullet_event.Start();

        }
        private void Bullet_shoot_event(object sender, EventArgs e) {
            if (bullet_direction == "up")
                Bullet.Top -= speed;
            else if (bullet_direction == "down")
                Bullet.Top += speed;
            else if (bullet_direction == "left")
                Bullet.Left -= speed;
            else if (bullet_direction == "right")
                Bullet.Left += speed;
            
        
        }

    }
}
