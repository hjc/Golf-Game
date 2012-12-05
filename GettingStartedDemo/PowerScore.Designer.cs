namespace GettingStartedDemo
{
    partial class PowerScore
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
            this.lblpower = new System.Windows.Forms.Label();
            this.lblstroke = new System.Windows.Forms.Label();
            this.parLabel = new System.Windows.Forms.Label();
            this.currentStrokeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblpower
            // 
            this.lblpower.Font = new System.Drawing.Font("Quartz MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpower.Location = new System.Drawing.Point(12, 9);
            this.lblpower.Name = "lblpower";
            this.lblpower.Size = new System.Drawing.Size(222, 53);
            this.lblpower.TabIndex = 0;
            this.lblpower.Text = "score";
            // 
            // lblstroke
            // 
            this.lblstroke.Font = new System.Drawing.Font("Quartz MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstroke.Location = new System.Drawing.Point(15, 62);
            this.lblstroke.Name = "lblstroke";
            this.lblstroke.Size = new System.Drawing.Size(219, 52);
            this.lblstroke.TabIndex = 1;
            this.lblstroke.Text = "score";
            // 
            // parLabel
            // 
            this.parLabel.Font = new System.Drawing.Font("Quartz MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parLabel.Location = new System.Drawing.Point(12, 126);
            this.parLabel.Name = "parLabel";
            this.parLabel.Size = new System.Drawing.Size(219, 52);
            this.parLabel.TabIndex = 2;
            this.parLabel.Text = "score";
            // 
            // currentStrokeLabel
            // 
            this.currentStrokeLabel.Font = new System.Drawing.Font("Quartz MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentStrokeLabel.Location = new System.Drawing.Point(12, 190);
            this.currentStrokeLabel.Name = "currentStrokeLabel";
            this.currentStrokeLabel.Size = new System.Drawing.Size(219, 52);
            this.currentStrokeLabel.TabIndex = 3;
            this.currentStrokeLabel.Text = "Strokes:";
            // 
            // PowerScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 272);
            this.Controls.Add(this.currentStrokeLabel);
            this.Controls.Add(this.parLabel);
            this.Controls.Add(this.lblstroke);
            this.Controls.Add(this.lblpower);
            this.Name = "PowerScore";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblpower;
        private System.Windows.Forms.Label lblstroke;
        private System.Windows.Forms.Label parLabel;
        private System.Windows.Forms.Label currentStrokeLabel;
    }
}