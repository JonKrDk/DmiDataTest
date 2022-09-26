namespace TestBench
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTest = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.btnObservations = new System.Windows.Forms.Button();
            this.btnGetTemps = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(10, 10);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(90, 27);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(10, 42);
            this.tbResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResult.Size = new System.Drawing.Size(622, 310);
            this.tbResult.TabIndex = 1;
            this.tbResult.WordWrap = false;
            // 
            // btnObservations
            // 
            this.btnObservations.Location = new System.Drawing.Point(104, 10);
            this.btnObservations.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnObservations.Name = "btnObservations";
            this.btnObservations.Size = new System.Drawing.Size(159, 27);
            this.btnObservations.TabIndex = 2;
            this.btnObservations.Text = "Get Observations";
            this.btnObservations.UseVisualStyleBackColor = true;
            this.btnObservations.Click += new System.EventHandler(this.btnObservations_Click);
            // 
            // btnGetTemps
            // 
            this.btnGetTemps.Location = new System.Drawing.Point(268, 8);
            this.btnGetTemps.Name = "btnGetTemps";
            this.btnGetTemps.Size = new System.Drawing.Size(151, 29);
            this.btnGetTemps.TabIndex = 3;
            this.btnGetTemps.Text = "Hent Temp Hist";
            this.btnGetTemps.UseVisualStyleBackColor = true;
            this.btnGetTemps.Click += new System.EventHandler(this.btnGetTemps_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.btnGetTemps);
            this.Controls.Add(this.btnObservations);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.btnTest);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnTest;
        private TextBox tbResult;
        private Button btnObservations;
        private Button btnGetTemps;
    }
}