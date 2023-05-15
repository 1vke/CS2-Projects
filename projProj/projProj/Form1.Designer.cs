namespace projProj
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.twizMove = new System.Windows.Forms.Timer(this.components);
            this.tmrProj = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tmrAnim = new System.Windows.Forms.Timer(this.components);
            this.enProjTic = new System.Windows.Forms.Timer(this.components);
            this.tmrGo = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // twizMove
            // 
            this.twizMove.Enabled = true;
            this.twizMove.Interval = 10;
            this.twizMove.Tick += new System.EventHandler(this.twizMove_Tick);
            // 
            // tmrProj
            // 
            this.tmrProj.Enabled = true;
            this.tmrProj.Interval = 5;
            this.tmrProj.Tick += new System.EventHandler(this.tmrProj_Tick);
            // 
            // tmrAnim
            // 
            this.tmrAnim.Interval = 1;
            this.tmrAnim.Tick += new System.EventHandler(this.tmrAnim_Tick);
            // 
            // enProjTic
            // 
            this.enProjTic.Enabled = true;
            this.enProjTic.Interval = 5;
            this.enProjTic.Tick += new System.EventHandler(this.enProjTic_Tick);
            // 
            // tmrGo
            // 
            this.tmrGo.Interval = 1000;
            this.tmrGo.Tick += new System.EventHandler(this.tmrGo_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(414, 524);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Galaga";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer twizMove;
        private System.Windows.Forms.Timer tmrProj;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tmrAnim;
        private System.Windows.Forms.Timer enProjTic;
        private System.Windows.Forms.Timer tmrGo;
    }
}

