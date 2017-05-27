namespace GamePrototype.Forms
{
    partial class MapForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.mapBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ownColorBox = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnMakeStep = new System.Windows.Forms.Button();
            this.btnGetReserve = new System.Windows.Forms.Button();
            this.btnSplitArmy = new System.Windows.Forms.Button();
            this.btnJoinArmies = new System.Windows.Forms.Button();
            this.btnMoveArmy = new System.Windows.Forms.Button();
            this.btnDefendRegion = new System.Windows.Forms.Button();
            this.btnAttackNearRegion = new System.Windows.Forms.Button();
            this.btnCopyText = new System.Windows.Forms.Button();
            this.hHeaderBox = new System.Windows.Forms.PictureBox();
            this.vHeaderBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ownColorBox)).BeginInit();
            this.actionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hHeaderBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vHeaderBox)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(1036, 12);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(1036, 42);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Выход";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // mapBox
            // 
            this.mapBox.Location = new System.Drawing.Point(42, 42);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size(640, 640);
            this.mapBox.TabIndex = 0;
            this.mapBox.TabStop = false;
            this.mapBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mapBox_Paint);
            this.mapBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapBox_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ownColorBox);
            this.groupBox1.Location = new System.Drawing.Point(715, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(75, 89);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ваш цвет";
            // 
            // ownColorBox
            // 
            this.ownColorBox.Location = new System.Drawing.Point(6, 19);
            this.ownColorBox.Name = "ownColorBox";
            this.ownColorBox.Size = new System.Drawing.Size(63, 63);
            this.ownColorBox.TabIndex = 0;
            this.ownColorBox.TabStop = false;
            this.ownColorBox.Paint += new System.Windows.Forms.PaintEventHandler(this.ownColorBox_Paint);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(715, 108);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(396, 303);
            this.listBox1.TabIndex = 5;
            // 
            // actionGroupBox
            // 
            this.actionGroupBox.Controls.Add(this.btnCancel);
            this.actionGroupBox.Controls.Add(this.btnMakeStep);
            this.actionGroupBox.Controls.Add(this.btnGetReserve);
            this.actionGroupBox.Controls.Add(this.btnSplitArmy);
            this.actionGroupBox.Controls.Add(this.btnJoinArmies);
            this.actionGroupBox.Controls.Add(this.btnMoveArmy);
            this.actionGroupBox.Controls.Add(this.btnDefendRegion);
            this.actionGroupBox.Controls.Add(this.btnAttackNearRegion);
            this.actionGroupBox.Location = new System.Drawing.Point(715, 417);
            this.actionGroupBox.Name = "actionGroupBox";
            this.actionGroupBox.Size = new System.Drawing.Size(396, 236);
            this.actionGroupBox.TabIndex = 6;
            this.actionGroupBox.TabStop = false;
            this.actionGroupBox.Text = "Действия";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(286, 94);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 63);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отменить";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnMakeStep
            // 
            this.btnMakeStep.Location = new System.Drawing.Point(286, 25);
            this.btnMakeStep.Name = "btnMakeStep";
            this.btnMakeStep.Size = new System.Drawing.Size(92, 63);
            this.btnMakeStep.TabIndex = 7;
            this.btnMakeStep.Text = "Сделать ход";
            this.btnMakeStep.UseVisualStyleBackColor = true;
            this.btnMakeStep.Click += new System.EventHandler(this.btnMakeStep_Click);
            // 
            // btnGetReserve
            // 
            this.btnGetReserve.Location = new System.Drawing.Point(22, 170);
            this.btnGetReserve.Name = "btnGetReserve";
            this.btnGetReserve.Size = new System.Drawing.Size(178, 23);
            this.btnGetReserve.TabIndex = 1;
            this.btnGetReserve.Text = "Пополнить армию из резерва";
            this.btnGetReserve.UseVisualStyleBackColor = true;
            this.btnGetReserve.Visible = false;
            this.btnGetReserve.Click += new System.EventHandler(this.btnGetReserve_Click);
            // 
            // btnSplitArmy
            // 
            this.btnSplitArmy.Location = new System.Drawing.Point(22, 141);
            this.btnSplitArmy.Name = "btnSplitArmy";
            this.btnSplitArmy.Size = new System.Drawing.Size(178, 23);
            this.btnSplitArmy.TabIndex = 1;
            this.btnSplitArmy.Text = "Разделить армию";
            this.btnSplitArmy.UseVisualStyleBackColor = true;
            this.btnSplitArmy.Visible = false;
            this.btnSplitArmy.Click += new System.EventHandler(this.btnSplitArmy_Click);
            // 
            // btnJoinArmies
            // 
            this.btnJoinArmies.Location = new System.Drawing.Point(22, 112);
            this.btnJoinArmies.Name = "btnJoinArmies";
            this.btnJoinArmies.Size = new System.Drawing.Size(178, 23);
            this.btnJoinArmies.TabIndex = 1;
            this.btnJoinArmies.Text = "Объединить армии";
            this.btnJoinArmies.UseVisualStyleBackColor = true;
            this.btnJoinArmies.Visible = false;
            this.btnJoinArmies.Click += new System.EventHandler(this.btnJoinArmies_Click);
            // 
            // btnMoveArmy
            // 
            this.btnMoveArmy.Location = new System.Drawing.Point(22, 54);
            this.btnMoveArmy.Name = "btnMoveArmy";
            this.btnMoveArmy.Size = new System.Drawing.Size(178, 23);
            this.btnMoveArmy.TabIndex = 1;
            this.btnMoveArmy.Text = "Переместить армию";
            this.btnMoveArmy.UseVisualStyleBackColor = true;
            this.btnMoveArmy.Click += new System.EventHandler(this.btnMoveArmy_Click);
            // 
            // btnDefendRegion
            // 
            this.btnDefendRegion.Location = new System.Drawing.Point(22, 83);
            this.btnDefendRegion.Name = "btnDefendRegion";
            this.btnDefendRegion.Size = new System.Drawing.Size(178, 23);
            this.btnDefendRegion.TabIndex = 1;
            this.btnDefendRegion.Text = "Защитить регион";
            this.btnDefendRegion.UseVisualStyleBackColor = true;
            this.btnDefendRegion.Visible = false;
            this.btnDefendRegion.Click += new System.EventHandler(this.btnDefendRegion_Click);
            // 
            // btnAttackNearRegion
            // 
            this.btnAttackNearRegion.Location = new System.Drawing.Point(22, 25);
            this.btnAttackNearRegion.Name = "btnAttackNearRegion";
            this.btnAttackNearRegion.Size = new System.Drawing.Size(178, 23);
            this.btnAttackNearRegion.TabIndex = 1;
            this.btnAttackNearRegion.Text = "Атаковать соседний регион";
            this.btnAttackNearRegion.UseVisualStyleBackColor = true;
            this.btnAttackNearRegion.Click += new System.EventHandler(this.btnAttack_Click);
            // 
            // btnCopyText
            // 
            this.btnCopyText.Location = new System.Drawing.Point(803, 72);
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.Size = new System.Drawing.Size(112, 23);
            this.btnCopyText.TabIndex = 8;
            this.btnCopyText.Text = "Копировать текст";
            this.btnCopyText.UseVisualStyleBackColor = true;
            this.btnCopyText.Click += new System.EventHandler(this.btnCopyText_Click);
            // 
            // hHeaderBox
            // 
            this.hHeaderBox.Location = new System.Drawing.Point(42, 5);
            this.hHeaderBox.Name = "hHeaderBox";
            this.hHeaderBox.Size = new System.Drawing.Size(640, 32);
            this.hHeaderBox.TabIndex = 9;
            this.hHeaderBox.TabStop = false;
            this.hHeaderBox.Paint += new System.Windows.Forms.PaintEventHandler(this.hHeaderBox_Paint);
            // 
            // hHeaderBox
            // 
            this.vHeaderBox.Location = new System.Drawing.Point(5, 42);
            this.vHeaderBox.Name = "hHeaderBox";
            this.vHeaderBox.Size = new System.Drawing.Size(32, 640);
            this.vHeaderBox.TabIndex = 10;
            this.vHeaderBox.TabStop = false;
            this.vHeaderBox.Paint += new System.Windows.Forms.PaintEventHandler(this.vHeaderBox_Paint);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 695);
            this.Controls.Add(this.vHeaderBox);
            this.Controls.Add(this.hHeaderBox);
            this.Controls.Add(this.btnCopyText);
            this.Controls.Add(this.actionGroupBox);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.mapBox);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Карта";
            this.Load += new System.EventHandler(this.MapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ownColorBox)).EndInit();
            this.actionGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hHeaderBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vHeaderBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mapBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox ownColorBox;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox actionGroupBox;
        private System.Windows.Forms.Button btnGetReserve;
        private System.Windows.Forms.Button btnSplitArmy;
        private System.Windows.Forms.Button btnJoinArmies;
        private System.Windows.Forms.Button btnMoveArmy;
        private System.Windows.Forms.Button btnDefendRegion;
        private System.Windows.Forms.Button btnAttackNearRegion;
        private System.Windows.Forms.Button btnMakeStep;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCopyText;
        private System.Windows.Forms.PictureBox hHeaderBox;
        private System.Windows.Forms.PictureBox vHeaderBox;
    }
}

