using projProj.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace projProj
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static bool right, left, up, down, cshoot, burshoot, lastd, firstshoot,anim,shoot = false;
        int interval, totX, totY = 0;
        int limit;
        int tic = 0;
        int goTic = 0;
        int dCount = 0;

        PictureBox[] ship = new PictureBox[1];
        List<bool> mods = new List<bool> { right, left, up, down }; //(bool right = false, left, up, down);\]
        List<Keys> keys = new List<Keys> { Keys.D, Keys.A, Keys.W, Keys.S };
        List<PictureBox> enemys = new List<PictureBox> ();
        List<List<PictureBox>> eLists = new List<List<PictureBox>> ();
        List<Tuple<Panel,int,int>> projs = new List<Tuple<Panel,int,int>>();
        List<Panel> enProj = new List<Panel> ();
        List<string> dirsL = new List<string>();
        List<List<string>> scripts = new List<List<string>> ();
        List<PictureBox> hpShip = new List<PictureBox>();

        int tmarg=0;
        bool tmargc = false;
        bool lr = true;
        int lrtic = 0;
        int lor = 1;
        bool lorstart = true;

        bool rndenProj = true;
        Random rnd = new Random();

        Label lblscore = new Label();
        int score = 0;

        Panel pnlGo = new Panel ();
        Label lblGo = new Label ();
        bool go = false;

        PictureBox ammo;
        bool amS = false;
        int ammoTic = 0;
        bool ammoP = false;

        PrivateFontCollection pfc = new PrivateFontCollection();

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cshoot = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                burshoot = false;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitCustomLabelFont();

            foreach (string temp in Resources.script1.Split('/'))
            {
                dirsL.Add(temp);
            }
            foreach (string dir in dirsL)
            {
                string[] dirtemp = dir.Split(',');

                if (dirtemp[0] != "") { totX += Convert.ToInt32(dirtemp[0]); totY += Convert.ToInt32(dirtemp[1]); }

            }
            for (int i = 0; i < 3; i++){ eLists.Add(new List<PictureBox> ()); }

            ship[0] = shipspawn();
            Espawn();

            lrtic = tmarg;
        }

        private void InitCustomLabelFont()
        {
            int fontLength = Resources.PublicPixel.Length;
            byte[] fontdata = Resources.PublicPixel;
            IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);
        }

        private void tmrAnim_Tick(object sender, EventArgs e)
        {
            bool gcheck = false;
            string[] di = dirsL[tic].Split(',');

            if (di.Length != 3) {  gcheck = true;  }

            
            if (gcheck != true) { 
                foreach (PictureBox enemy in enemys.ToList())
                {
                    if (di[1] != "" || di[0] != "")
                    {
                        double tempx = enemy.Location.X + Convert.ToDouble(di[0]) * Math.Pow(-1.00, Convert.ToDouble(enemy.Tag));
                        enemy.Location = new Point(Convert.ToInt32(tempx), enemy.Location.Y + Convert.ToInt32(di[1]));
                    }
                }
            }

            if (tic == limit - 1)
            {
                tmrAnim.Stop();
                tic = 0;
                anim = false;
            }
            else { tic++; }
        }

        private void enProjTic_Tick(object sender, EventArgs e)
        {
            if (anim) { return; }
            if (interval % 60 == 0)
            {
                bool projStopper = true;
                for (int i = 0; i < enemys.Count; i++)
                {
                    int temp = rnd.Next(0, 100) + 1;
                    if (temp >= 1 && temp <= 20)
                    {
                        if (projStopper)
                        {
                            Panel enprojectile = projS(Color.Red, enemys[i].Location.X + enemys[i].Width / 2, enemys[i].Location.Y + enemys[i].Width / 2);
                            enProj.Add(enprojectile);
                        }

                        projStopper = false;
                    }
                }
            }


            foreach (Panel proj in enProj.ToList())
            {
                proj.Top += 7;

                if (proj.Location.Y > this.ClientSize.Height)
                {
                    enProj.Remove(proj);
                    proj.Dispose();
                    break;
                }

                if (proj.Bounds.IntersectsWith(ship[0].Bounds))
                {
                    

                    enProj.Remove(proj);
                    proj.Dispose();

                    int temp = Convert.ToInt32(ship[0].Tag) - 1;
                    ship[0].Tag = temp;

                    PictureBox hpS = hpShip[temp];
                    hpS.Visible = false;

                    if (Convert.ToInt32(ship[0].Tag) == 0)
                    {
                        GameOver();
                        
                        break;
                    }
                    else { break;}
                }
            }
        }
        public void GameOver()
        {
            pnlGo.Location = new System.Drawing.Point(0, 0);
            pnlGo.Name = "pnlGo";
            pnlGo.Size = new System.Drawing.Size(415, 526);
            pnlGo.TabIndex = 0;
            Controls.Add(pnlGo);

            lblGo.AutoSize = true;
            lblGo.Font = new Font(pfc.Families[0], 8);
            lblGo.UseCompatibleTextRendering = true;
            lblGo.ForeColor = System.Drawing.Color.White;
            lblGo.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - lblGo.Width / 2, this.ClientSize.Height / 2);
            lblGo.Name = "lblGo";
            lblGo.Size = new System.Drawing.Size(109, 21);
            lblGo.TabIndex = 0;
            lblGo.Text = "GAME OVER";
            Controls.Add(lblGo);
            lblGo.BringToFront();

            Reset();
        }
        public void Reset()
        {
            anim = true;

            dCount = 0;


            foreach ( PictureBox temp in enemys.ToList())
            {
                temp.Dispose();
            }
            enemys.Clear();

            foreach (List<PictureBox> temp in eLists.ToList())
            {
                foreach(PictureBox temp2 in temp.ToList())
                {
                    temp2.Dispose();
                }
                temp.Clear();
            }

            foreach(Tuple< Panel,int,int> temp in projs.ToList())
            {
                temp.Item1.Dispose();
            }
            projs.Clear();

            foreach (Panel temp in enProj.ToList())
            {
                temp.Dispose();
            }

            enProj.Clear();

            dirsL.Clear();

            scripts.Clear();

            hpShip.Clear();

            tmarg = 0;
            tmargc = false;
            lr = true;
            lrtic = 0;
            lor = 1;
            lorstart = true;
            rndenProj = true;

            lblscore.Dispose();
            lblscore = new Label();

            score = 0;

            right = false; left = false; up = false; down = false; cshoot = false; burshoot = false; lastd = false; firstshoot = false; anim = false;
            interval = 0; totX = 0; totY = 0;
            limit = 0;
            tic = 0;

            
            ship[0].Dispose();
            ship = new PictureBox[1];

            if (amS) { ammo.Dispose(); }
            amS = false;
            ammoP = false;
            ammoTic = 0;

            tmrAnim.Stop();
            twizMove.Stop();
            tmrProj.Stop();
            enProjTic.Stop();

            tmrGo.Start();
            

        }
        private void tmrProj_Tick(object sender, EventArgs e)
        {
            interval++;
            if (ammoP) { ammoTic++; }
            int projspeed = 7;
            bool brek = false;

            if (interval % 150 == 0)
            {
                int temp = rnd.Next(0,100) + 1;
                if (temp <= 25 && !amS)
                {
                    ammoS();
                    amS = true;
                }

            }

            if (amS)
            {
                ammo.Top += 2;
                if (ammo.Location.Y > this.ClientSize.Height)
                {
                    ammo.Dispose();
                    amS=false;
                }
                if (ship[0].Bounds.IntersectsWith(ammo.Bounds))
                {
                    ammo.Dispose();
                    amS = false;
                    ammoP = true;
                }

                if (ammoTic % 75 == 0 && ammoTic != 0)
                {
                    ammoP = false;
                    ammoTic = 0;
                }
            }

            

            foreach (Tuple<Panel,int,int> proj in projs.ToList())
            {
                for (int i = 0;i < enemys.Count; i++)
                {
                    if (proj.Item1.Bounds.IntersectsWith(enemys[i].Bounds))
                    {
                        if (anim) { return; }
                        if (Convert.ToInt32(enemys[i].Tag) == 1)
                        {
                            PictureBox pic = enemys[i];
                            enemys.Remove(pic);

                            for (int j = 0; j < eLists.Count; j++)
                            {
                                if (eLists[j].Contains(pic)) { eLists[j].Remove(pic); }
                            }

                            pic.Dispose();

                            UpdScore(100);

                            if (enemys.Count == 0)
                            {
                                Espawn();
                                lrtic = tmarg;
                            }

                            brek = true;
                        }
                        else
                        {
                            enemys[i].Tag = Convert.ToInt32(enemys[i].Tag) - 1;
                        }

                        projs.Remove(proj);
                        proj.Item1.Dispose();

                        if (brek) { break; }
                    }
                }

                if (proj.Item3 == 0) { proj.Item1.Top -= projspeed; }
                else
                {
                    double longS = Math.Ceiling(projspeed* Math.Cos(6 * Math.PI / 180));
                    double shrS = Math.Ceiling(projspeed* Math.Sin(6 * Math.PI / 180));
                    int x = (Convert.ToInt32(shrS) * proj.Item2) * proj.Item3;
                    int y = Convert.ToInt32(longS) ;

                    proj.Item1.Location = new Point(proj.Item1.Location.X + x, proj.Item1.Location.Y - y);
                }
                if ( proj.Item1.Location.X > this.ClientSize.Width || proj.Item1.Location.X + proj.Item1.Width < 0 || proj.Item1.Location.Y  > this.ClientSize.Height || proj.Item1.Location.Y+ proj.Item1.Height < 0)
                {
                    projs.Remove(proj);
                    proj.Item1.Dispose();
                }

                
            }

            
            if (firstshoot || interval % 45 == 0)
            {
                if (firstshoot) { firstshoot = false; }
                if (anim) { return; }
                if (shoot)
                {
                    int lor;
                    if (lastd == false) { lor = 1; }
                    else { lor = -1; }
                    if (!ammoP)
                    {
                        var proj = Tuple.Create(projS(Color.White,0,0), lor, 0);
                        projs.Add(proj);
                    }
                    else
                    {
                        int j = 1;
                        for (int i = 0; i < 3; i++)
                        {
                            var proj = Tuple.Create(projS(Color.White, 0, 0), lor, j);
                            projs.Add(proj);
                            j--;
                        }
                    }
                }
                else if(firstshoot == false)
                {
                    firstshoot = true;
                }
            }
            
        }


        public void UpdScore(int temp)
        {
            score += temp;
            lblscore.Text = "SCORE: " + score.ToString();
        }

        private void tmrGo_Tick(object sender, EventArgs e)
        {
            goTic++;
            if (goTic == 5)
            {
                goTic = 0;
                foreach (string temp in Resources.script1.Split('/'))
                {
                    dirsL.Add(temp);
                }
                foreach (string dir in dirsL)
                {
                    string[] dirtemp = dir.Split(',');

                    if (dirtemp[0] != "") { totX += Convert.ToInt32(dirtemp[0]); totY += Convert.ToInt32(dirtemp[1]); }

                }
                for (int i = 0; i < 3; i++) { eLists.Add(new List<PictureBox>()); }

                ship[0] = shipspawn();
                Espawn();

                lrtic = tmarg;
                tmrAnim.Start();
                twizMove.Start();
                tmrProj.Start();
                enProjTic.Start();
                lblGo.Dispose();
                pnlGo.Dispose();
                lblGo = new Label();
                pnlGo = new Panel();
                
                tmrGo.Stop();
            }
        }

        public Panel projS(Color col, int x, int y)
        {
            if (x == 0)
            {
                x = ship[0].Location.X + (ship[0].Width / 2);
                y = ship[0].Location.Y;
            }

            Panel pnl = new Panel();
            pnl.Location = new Point(x, y);
            pnl.BackColor = col;
            pnl.Name = "pnl" + projs.Count.ToString();
            pnl.Size = new System.Drawing.Size(2, 6);
            pnl.TabIndex = 0;
            pnl.TabStop = true;
            Controls.Add(pnl);
            pnl.BringToFront();
            return pnl;
        }

        public PictureBox shipspawn()
        {
            PictureBox pic = new PictureBox();
            pic.Image = Resources.ship;
            pic.Location = new Point((this.ClientSize.Width / 2) - 18, 460);
            pic.Name = "ship";
            pic.Size = new Size(36, 36);
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.TabIndex = 1;
            pic.Tag = 3;
            pic.TabStop = false;
            Controls.Add(pic);
            pic.BringToFront();

            hpSpawn();
            scorespawn();
            return pic;
        }
        public void scorespawn()
        {
            lblscore.AutoSize = true;
            lblscore.Font = new Font(pfc.Families[0], 8);
            lblscore.ForeColor = System.Drawing.Color.White;
            lblscore.BackColor = Color.Transparent;
            lblscore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblscore.Location = new System.Drawing.Point(4, 4);
            lblscore.Name = "lblscore";
            lblscore.UseCompatibleTextRendering = true;
            lblscore.Size = new System.Drawing.Size(74, 18);
            lblscore.TabIndex = 0;
            lblscore.Text = "SCORE: 0";
            Controls.Add(lblscore);
            lblscore.SendToBack();
        }

        public void ammoS()
        {
            ammo = new PictureBox();
            ammo.Image = Resources.ammosprite;
            int tempx = rnd.Next(0, this.Width - 31);
            ammo.Location = new Point(tempx, -26);
            ammo.Name = "pictureBox1";
            ammo.Size = new Size(31, 26);
            ammo.SizeMode = PictureBoxSizeMode.Zoom;
            ammo.TabIndex = 0;
            ammo.TabStop = false;
            Controls.Add(ammo);
            ammo.SendToBack();
        }


        public void hpSpawn()
        {
            int size = 20;
            for (int i = 1; i<=3; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Image = Resources.ship;
                pic.Location = new Point(0+(size*(i-1)), this.ClientSize.Height - size);
                pic.Name = "ship" + i.ToString();
                pic.Size = new Size(size, size);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.TabIndex = 1;
                pic.TabStop = false;
                Controls.Add(pic);
                hpShip.Add(pic);
            }
        }
        public void Espawn()
        {
            int[] eCounts = { 10, 8, 6  };
            string[] eNames = {"enemy1","enemy2","enemy3" };
            int[] eHealths = {1,2,3 };
            
            for(int i = 0; i < eCounts.Length; i++)
            {
                SpawnLayer(eCounts[i], eHealths[i], eNames[i], i);
            }


            limit = dirsL.Count;
            tmrAnim.Start();
            anim = true;
        }

        public void SpawnLayer(int eCount, int eHealth, string eName, int ind)
        {
            Image[] eImgs = { Resources.enemy1, Resources.enemy2, Resources.enemy3 };
            int genSize = 25 ;
            int genSpcing = 7;
            int layerSpc = 3;
            int margin = 0;
            int startH = 30;
            
            int listIn = eHealth - 1;

            int dyntotX = Convert.ToInt32(totX * Math.Pow(-1.00, eHealth));

            for (int i = 0; i < eCount; i++)
            {
                margin += genSize;
                if (i != 0) { margin += genSpcing; }
            }
            margin = (this.ClientSize.Width - margin) / 2;

            if (!tmargc) { tmargc = true; tmarg = margin; }

            for (int i = 0; i < eCount; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Image = eImgs[ind];
                pic.Location = new Point((margin + (i * (genSize + genSpcing))) - dyntotX, (startH + ((genSize+layerSpc)*(ind))) - totY) ;
                pic.Name = eName + eLists[listIn].Count.ToString();
                pic.Size = new Size(genSize, genSize);
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.TabIndex = 1;
                pic.TabStop = true;
                pic.Tag = eHealth;
                eLists[listIn].Add(pic);
                enemys.Add(pic); 
                pic.BringToFront();
                Controls.Add(pic);
            }
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void twizMove_Tick(object sender, EventArgs e)
        {
            int twSpeed = 5;

            if (mods[0]) {
                if ((ship[0].Location.X+ship[0].Width) + twSpeed > this.ClientSize.Width) { ship[0].Left += this.ClientSize.Width - (ship[0].Location.X + ship[0].Width); lastd = false; }
                else { ship[0].Left += twSpeed; lastd = false; }
            }
            if (mods[1]) {
                if (ship[0].Location.X - twSpeed <= 0) { ship[0].Left -= ship[0].Location.X; lastd = true; }
                else { ship[0].Left -= twSpeed; lastd = true; }
            }
            if (mods[2]) {
                if(ship[0].Top - twSpeed <= 360) { ship[0].Top -= ship[0].Location.Y - 360; }
                else { ship[0].Top -= twSpeed; }
            }

            if (mods[3]) { 
                if(ship[0].Location.Y + ship[0].Height + twSpeed >= this.ClientSize.Height) { ship[0].Top += this.ClientSize.Height - (ship[0].Location.Y + ship[0].Height) ; }
                else { ship[0].Top += twSpeed; }

                
            }


            if (!anim)
            {
                if (lrtic == 0)
                {
                    lrtic = tmarg * 2;
                    if (lor == 1) { lor = -1; }
                    else { lor = 1; }
                    lr = false;
                }
                else if(lr)
                {
                    lorstart = true;
                    foreach (PictureBox enemy in enemys.ToList())
                    {
                        if (lorstart) { lrtic -= 5; lorstart = false; }
                        enemy.Left += 5 * lor;
                    }
                }
                else
                {
                    foreach (PictureBox enemy in enemys.ToList())
                    {
                        enemy.Top += 3 ;
                        dCount += 1;
                        if (dCount == 2800)
                        {
                            GameOver();
                            
                        }
                        else { lr = true; }
                    }
                }
            }
        }

       

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keys.Contains(e.KeyCode))
            {
                mods[keys.IndexOf(e.KeyCode)] = true;
            }

            if (anim) { return; }

            if (e.KeyCode == Keys.Space)
            {
                shoot = true;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keys.Contains(e.KeyCode))
            {
                mods[keys.IndexOf(e.KeyCode)] = false;
            }
            else if (e.KeyCode == Keys.Space) { shoot = false;}
        }
    }
}
