namespace BSS.LineUpFour
{
    partial class FormLineUpFour
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
            System.Windows.Forms.ToolStripMenuItem miLevel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLineUpFour));
            this.miLevel0 = new System.Windows.Forms.ToolStripMenuItem();
            this.miLevel1 = new System.Windows.Forms.ToolStripMenuItem();
            this.miLevel2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miLevel3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miSwap = new System.Windows.Forms.ToolStripMenuItem();
            miLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // miLevel
            // 
            miLevel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLevel0,
            this.miLevel1,
            this.miLevel2,
            this.miLevel3});
            miLevel.Name = "miLevel";
            miLevel.Size = new System.Drawing.Size(46, 19);
            miLevel.Text = "&Level";
            // 
            // miLevel0
            // 
            this.miLevel0.Checked = true;
            this.miLevel0.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miLevel0.Name = "miLevel0";
            this.miLevel0.Size = new System.Drawing.Size(127, 22);
            this.miLevel0.Text = "&Easy";
            this.miLevel0.Click += new System.EventHandler(this.OnClickMILevel);
            // 
            // miLevel1
            // 
            this.miLevel1.Name = "miLevel1";
            this.miLevel1.Size = new System.Drawing.Size(127, 22);
            this.miLevel1.Text = "&Normal";
            this.miLevel1.Click += new System.EventHandler(this.OnClickMILevel);
            // 
            // miLevel2
            // 
            this.miLevel2.Name = "miLevel2";
            this.miLevel2.Size = new System.Drawing.Size(127, 22);
            this.miLevel2.Text = "&Advanced";
            this.miLevel2.Click += new System.EventHandler(this.OnClickMILevel);
            // 
            // miLevel3
            // 
            this.miLevel3.Name = "miLevel3";
            this.miLevel3.Size = new System.Drawing.Size(127, 22);
            this.miLevel3.Text = "&Hard";
            this.miLevel3.Click += new System.EventHandler(this.OnClickMILevel);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStart,
            this.miSwap,
            miLevel});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(19, 6, 0, 6);
            this.menuStrip.Size = new System.Drawing.Size(442, 31);
            this.menuStrip.TabIndex = 0;
            // 
            // miStart
            // 
            this.miStart.Name = "miStart";
            this.miStart.Size = new System.Drawing.Size(46, 19);
            this.miStart.Text = "&Start!";
            this.miStart.Click += new System.EventHandler(this.OnClickMIStart);
            // 
            // miSwap
            // 
            this.miSwap.Name = "miSwap";
            this.miSwap.Size = new System.Drawing.Size(47, 19);
            this.miSwap.Text = "S&wap";
            this.miSwap.Click += new System.EventHandler(this.OnClickMISwap);
            // 
            // FormLineUpFour
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.SlateBlue;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(442, 404);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Book Antiqua", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Fuchsia;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.MaximizeBox = false;
            this.Name = "FormLineUpFour";
            this.Text = "Line up four";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miStart;
        private System.Windows.Forms.ToolStripMenuItem miSwap;
        private System.Windows.Forms.ToolStripMenuItem miLevel2;
        private System.Windows.Forms.ToolStripMenuItem miLevel3;
        private System.Windows.Forms.ToolStripMenuItem miLevel0;
        private System.Windows.Forms.ToolStripMenuItem miLevel1;
    }
}

