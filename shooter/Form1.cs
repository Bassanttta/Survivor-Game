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
    public partial class Form1 : Form
    {
        Thread NEXTLEVEL,OP_MAIN;
        bool P_up, P_down, P_left, P_right,gameover,gameon;
        string P_pos = "up";
        int P_health = 100;
        public int level=1;
        int kills = 0;
        int P_speed = 15, Z_speed = 3, Z_num = 3, Z_kills_to_win=25;
        int Ammo = 10,Health_c,cntHealth=0;
        Random r = new Random();    
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Dont forget to press enter to start the actual level !", "Attention Player !", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        public void SetDifficulty(int l,int z,int h,int w) {
            level = l;
            Z_num = z;
            Health_c = h;
            Z_kills_to_win = w;
        }



        private void Game_events(object sender, EventArgs e)
        {
            // progressbr does not a transparent backcolor
            //mainLevel_theme();
           
            updateValues();
           
            if (P_health > 0 &&kills<Z_kills_to_win)
            {
                progressBar1.Value = P_health;
                if (P_health < 50)
                    progressBar1.ForeColor = Color.Red;

            }
            else if (kills == Z_kills_to_win && level < 3)
            {
                Gametimer0.Stop();
                
                MessageBox.Show("Winner Winner Winner Q_Q!", "WINNING LEVEL " + level, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                
                NEXTLEVEL = new Thread(N_LEVEL);
                NEXTLEVEL.SetApartmentState(ApartmentState.STA);
                NEXTLEVEL.Start();
                
            }
            else if (kills == Z_kills_to_win && level == 3) {
                Gametimer0.Stop();

                MessageBox.Show("Winner Winner Winner Q_Q!", "WINNING LEVEL " + level, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                NEXTLEVEL = new Thread(Final);
                NEXTLEVEL.SetApartmentState(ApartmentState.STA);
                NEXTLEVEL.Start();
               
            }
            else if (P_health < 0)
            {
                gameover = true;
                player.Image = Properties.Resources.dead;
                player.BringToFront();
                Gametimer0.Stop();
                DialogResult res = MessageBox.Show("GAME OVER ! \nReturn to the main menu ?", "LOSER -_-", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.Yes)
                {
                    //Thread to open the <MAIN> form
                    this.Close();

                    OP_MAIN = new Thread(open_main);
                    OP_MAIN.SetApartmentState(ApartmentState.STA);
                    OP_MAIN.Start();
                }
                else
                {
                    MessageBox.Show("May the odds be ever in ur favor", "BYE !", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Close();
                }
            }
            
            /***************************************************************************************/
            if (P_left == true && player.Left > 0)
                player.Left -= P_speed;
            else if (P_right == true && player.Left + player.Width < this.ClientSize.Width)
                player.Left += P_speed;
            else if (P_up == true && player.Top > 45)
                player.Top -= P_speed;
            else if (P_down == true && player.Top + player.Height < this.ClientSize.Height)
                player.Top += P_speed;

            /***************************************************************************************/
            foreach (Control x in this.Controls)
            {
                if (player.Bounds.IntersectsWith(x.Bounds))
                {
                  // take ammo
                    if (Convert.ToString(x.Tag) == "ammo")
                    {
                        this.Controls.Remove(x);
                         x.Dispose();
                        Ammo += 5;
                    }
                    // take damge
                    if (Convert.ToString(x.Tag) == "zombie")
                    {
                        P_health--;
                    }
                    // take health
                    if (Convert.ToString(x.Tag) == "health")
                    { P_health +=(100-P_health);
                    
                        progressBar1.ForeColor = Color.Lime;
                
                        this.Controls.Remove(x);
                    }
                }
                if (Convert.ToString(x.Tag) == "zombie") {
                    
                    
                    if (x.Left > player.Left)
                    {
                        x.Left -= Z_speed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    } if (x.Left < player.Left)
                    {
                        x.Left += Z_speed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    } 
                    if (x.Top > player.Top)
                    {
                        x.Top -= Z_speed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    } if (x.Top < player.Top)
                    {
                        x.Top += Z_speed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }

                }
                foreach(Control y in this.Controls)
                    if (Convert.ToString(y.Tag) == "bullet" && Convert.ToString(x.Tag) == "zombie" && x.Bounds.IntersectsWith(y.Bounds))
                    {
                        kills++;
                        this.Controls.Remove(y);
                        ((PictureBox)y).Dispose();
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        Zombies_Creation();
                    }

            }



        }



        // player start moving  once we  press any key


        private void PlayerMovements_start(object sender, KeyEventArgs e)
        {
            if (gameover == true || gameon == false)
                return;

            if (e.KeyCode == Keys.Z &&  cntHealth>0 )
            {
                MedDrop();
                cntHealth--;
            }
            else if (e.KeyCode == Keys.Left)
            {
                P_pos = "left";
                P_left = true;
                player.Image = Properties.Resources.left;
                
            }
            else if (e.KeyCode == Keys.Right)
            {
                P_pos = "right";
                P_right = true;
                player.Image = Properties.Resources.right;
                
            }
            else if (e.KeyCode == Keys.Down)
            {
                P_pos = "down";
                P_down = true;
                player.Image = Properties.Resources.down;
                
            }
            else if (e.KeyCode == Keys.Up)
            {
                P_pos = "up";
                P_up = true;
                player.Image = Properties.Resources.up;
                
            }
            else if (e.KeyCode == Keys.Space && Ammo > 0 && gameover == false)
            {
                Ammo--;
                BULLETS_ON(P_pos);
                if (Ammo < 2)
                    AmmoDrop();
            }
            
            
        }
        // SToping player from moving once we dont press any key

        private void PlayerMovements_stop(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                P_left = false;
            else if (e.KeyCode == Keys.Right)
                P_right = false;
            else if (e.KeyCode == Keys.Down)
                P_down = false;
             
            else if (e.KeyCode == Keys.Up)
                P_up = false;
            else if (e.KeyCode == Keys.Enter && gameover == false && gameon == false)
            {
                RESETGAME();
                gameon = true;
            }
            
        }
        // Create shooting bullets
        
        private void BULLETS_ON(string dir){
        
        BulletS B_shoot=new BulletS();
        B_shoot.bullet_direction = dir;
        B_shoot.b_left = player.Left + (player.Width >> 1);
        B_shoot.b_top = player.Top + (player.Height >> 1);
        B_shoot.Create_Bullets(this);
        }

        // create Ammo when Ammo run out
        
        private void AmmoDrop() {
            PictureBox ammo = new PictureBox();
            
            ammo.Tag = "ammo";
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.BackColor = Color.Transparent;
            
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = r.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = r.Next(45, this.ClientSize.Height- ammo.Height);
            
            this.Controls.Add(ammo);
            ammo .BringToFront();
            player.BringToFront();
            
        }
        
        
        //Create Med_aid to increase player health
        private void MedDrop() {
            PictureBox Med = new PictureBox();
            Med.Tag = "health";
            Med.Image = Properties.Resources.health;
            Med.SizeMode = PictureBoxSizeMode.StretchImage;
            Med.BackColor = Color.Transparent;
            Med.Size = new System.Drawing.Size(80, 76);
            Med.Left = r.Next(10, this.ClientSize.Width - Med.Width);
            Med.Top = r.Next(45, this.ClientSize.Height - Med.Height);
            this.Controls.Add(Med);
            Med.BringToFront();
            player.BringToFront();
        }
        
        
        // Create  Zombies
        
        
        private void Zombies_Creation(){
            PictureBox zombie = new PictureBox();
            // properities
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombie.BackColor = Color.Transparent;
            //position
            zombie.Top = r.Next(0, 900);
            zombie.Left = r.Next(0, 900);
            this.Controls.Add(zombie);
            zombie.BringToFront();
            
            //player.BringToFront();
         
        }
        
        
        private void RESETGAME() {


            P_up=false;
            P_down=false;
            P_left=false; 
            P_right=false;
            gameover = false;
            gameon = false;
            progressBar1.ForeColor = Color.Lime;
             P_pos = "up";
             P_health = 100;
             cntHealth = Health_c;
             kills = 0;
             P_speed = 15;
             Z_speed = 2;
             Ammo = 10;
            
            for (int z = 0; z < Z_num; z++)
                Zombies_Creation();

             Gametimer0.Start();
        }


        //Next level form
        private void N_LEVEL() {
            level++;
            LevelGG B = new LevelGG();
            
            if (level == 2)
                B.BackgroundImage = Properties.Resources.level2;
            else
                B.BackgroundImage = Properties.Resources.level3;

            B.setlevel(level,Z_num,Health_c,Z_kills_to_win);
            Application.Run(B);
         
        }
        
        public void mainLevel_theme()
        {
            if (level == 1)
            {
                this.BackgroundImage = Properties.Resources.backgrounddetailed5;
                this.BackgroundImageLayout = ImageLayout.Stretch;
           
                player.BringToFront();
            }
            else if (level == 2)
            {
                this.BackgroundImage = Properties.Resources.Texture_Resouce_Paint_cangood_CanGood94_CanGood_cangood94;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                player.BringToFront();

            }
            else if(level==3)
            {
                this.BackgroundImage = Properties.Resources.Texture_By_League_of_Legends_Maps_CanGood;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                player.BringToFront();
            }
        }
        
        private void updateValues()
        {
            label1.Text = "AMMO :" + Ammo;
            label2.Text = "KILLS :" + kills;
            label4.Text = "Med :" + cntHealth;

        }

        private void open_main()
        {
            Application.Run(new Main());
        }

        // final level after winnig
        private void Final()
        {
            Application.Run(new winning());
        }

    }
}
