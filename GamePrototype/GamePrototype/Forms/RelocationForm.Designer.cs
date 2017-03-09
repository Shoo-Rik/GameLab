namespace GamePrototype.Forms
{
    partial class RelocationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fromCountBox = new System.Windows.Forms.NumericUpDown();
            this.toCountBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromCountBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toCountBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Из";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "в";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(28, 58);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(322, 45);
            this.trackBar1.TabIndex = 5;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(392, 18);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "ОК";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(392, 47);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // fromCountBox
            // 
            this.fromCountBox.Location = new System.Drawing.Point(66, 21);
            this.fromCountBox.Name = "fromCountBox";
            this.fromCountBox.Size = new System.Drawing.Size(120, 20);
            this.fromCountBox.TabIndex = 8;
            this.fromCountBox.ValueChanged += new System.EventHandler(this.fromCountBox_ValueChanged);
            this.fromCountBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fromCountBox_KeyUp);
            // 
            // toCountBox
            // 
            this.toCountBox.Location = new System.Drawing.Point(230, 21);
            this.toCountBox.Name = "toCountBox";
            this.toCountBox.Size = new System.Drawing.Size(120, 20);
            this.toCountBox.TabIndex = 9;
            this.toCountBox.ValueChanged += new System.EventHandler(this.toCountBox_ValueChanged);
            this.toCountBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toCountBox_KeyUp);
            // 
            // RelocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 107);
            this.Controls.Add(this.toCountBox);
            this.Controls.Add(this.fromCountBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RelocationForm";
            this.Text = "Перемещение армии";
            this.Load += new System.EventHandler(this.RelocationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fromCountBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toCountBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown fromCountBox;
        private System.Windows.Forms.NumericUpDown toCountBox;
    }
}