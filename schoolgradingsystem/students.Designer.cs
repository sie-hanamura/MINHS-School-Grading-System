
namespace schoolgradingsystem
{
    partial class Students
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Students));
            this.StudentPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.sectionPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.roundButton2 = new introduction.RoundButton();
            this.btnAddSection = new introduction.RoundButton();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.StudentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StudentPanel
            // 
            this.StudentPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("StudentPanel.BackgroundImage")));
            this.StudentPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StudentPanel.Controls.Add(this.button1);
            this.StudentPanel.Controls.Add(this.sectionPanel);
            this.StudentPanel.Controls.Add(this.label1);
            this.StudentPanel.Controls.Add(this.roundButton2);
            this.StudentPanel.Controls.Add(this.btnAddSection);
            this.StudentPanel.Controls.Add(this.lblWelcome);
            this.StudentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StudentPanel.Location = new System.Drawing.Point(0, 0);
            this.StudentPanel.Name = "StudentPanel";
            this.StudentPanel.Size = new System.Drawing.Size(774, 498);
            this.StudentPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Nirmala UI", 7.841584F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(608, 69);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 28);
            this.button1.TabIndex = 16;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sectionPanel
            // 
            this.sectionPanel.AutoScroll = true;
            this.sectionPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.sectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sectionPanel.Font = new System.Drawing.Font("Times New Roman", 12.83168F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionPanel.Location = new System.Drawing.Point(27, 106);
            this.sectionPanel.Name = "sectionPanel";
            this.sectionPanel.Size = new System.Drawing.Size(720, 299);
            this.sectionPanel.TabIndex = 14;
            this.sectionPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.sectionPanel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(23, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 22);
            this.label1.TabIndex = 13;
            this.label1.Text = "Sections and Grade Levels";
            // 
            // roundButton2
            // 
            this.roundButton2.BackColor = System.Drawing.Color.Black;
            this.roundButton2.FlatAppearance.BorderSize = 0;
            this.roundButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundButton2.ForeColor = System.Drawing.Color.White;
            this.roundButton2.Location = new System.Drawing.Point(588, 422);
            this.roundButton2.Name = "roundButton2";
            this.roundButton2.Size = new System.Drawing.Size(159, 38);
            this.roundButton2.TabIndex = 12;
            this.roundButton2.Text = "Edit Section";
            this.roundButton2.UseVisualStyleBackColor = false;
            this.roundButton2.Click += new System.EventHandler(this.roundButton2_Click);
            // 
            // btnAddSection
            // 
            this.btnAddSection.BackColor = System.Drawing.Color.Black;
            this.btnAddSection.FlatAppearance.BorderSize = 0;
            this.btnAddSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSection.ForeColor = System.Drawing.Color.White;
            this.btnAddSection.Location = new System.Drawing.Point(27, 422);
            this.btnAddSection.Name = "btnAddSection";
            this.btnAddSection.Size = new System.Drawing.Size(159, 38);
            this.btnAddSection.TabIndex = 11;
            this.btnAddSection.Text = "Add Section";
            this.btnAddSection.UseVisualStyleBackColor = false;
            this.btnAddSection.Click += new System.EventHandler(this.btnAddSection_Click);
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblWelcome.Font = new System.Drawing.Font("Impact", 19.9604F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.ForeColor = System.Drawing.Color.Black;
            this.lblWelcome.Location = new System.Drawing.Point(21, 18);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(122, 36);
            this.lblWelcome.TabIndex = 6;
            this.lblWelcome.Text = "Students";
            // 
            // Students
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 498);
            this.Controls.Add(this.StudentPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Students";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Students";
            this.StudentPanel.ResumeLayout(false);
            this.StudentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel StudentPanel;
        private System.Windows.Forms.Label lblWelcome;
        private introduction.RoundButton roundButton2;
        private introduction.RoundButton btnAddSection;
        private System.Windows.Forms.Panel sectionPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}