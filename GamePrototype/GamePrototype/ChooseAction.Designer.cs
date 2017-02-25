namespace GamePrototype
{
    partial class ChooseAction
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
            this.attackNearRegion = new System.Windows.Forms.RadioButton();
            this.moveArmy = new System.Windows.Forms.RadioButton();
            this.joinArmies = new System.Windows.Forms.RadioButton();
            this.splitArmy = new System.Windows.Forms.RadioButton();
            this.getReserve = new System.Windows.Forms.RadioButton();
            this.defendRegion = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.regionInfo = new System.Windows.Forms.RadioButton();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.actionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // attackNearRegion
            // 
            this.attackNearRegion.AutoSize = true;
            this.attackNearRegion.Location = new System.Drawing.Point(6, 17);
            this.attackNearRegion.Name = "attackNearRegion";
            this.attackNearRegion.Size = new System.Drawing.Size(167, 17);
            this.attackNearRegion.TabIndex = 0;
            this.attackNearRegion.TabStop = true;
            this.attackNearRegion.Text = "Атаковать соседний регион";
            this.attackNearRegion.UseVisualStyleBackColor = true;
            // 
            // moveArmy
            // 
            this.moveArmy.AutoSize = true;
            this.moveArmy.Location = new System.Drawing.Point(6, 80);
            this.moveArmy.Name = "moveArmy";
            this.moveArmy.Size = new System.Drawing.Size(130, 17);
            this.moveArmy.TabIndex = 0;
            this.moveArmy.TabStop = true;
            this.moveArmy.Text = "Переместить армию";
            this.moveArmy.UseVisualStyleBackColor = true;
            // 
            // joinArmies
            // 
            this.joinArmies.AutoSize = true;
            this.joinArmies.Location = new System.Drawing.Point(6, 103);
            this.joinArmies.Name = "joinArmies";
            this.joinArmies.Size = new System.Drawing.Size(122, 17);
            this.joinArmies.TabIndex = 0;
            this.joinArmies.TabStop = true;
            this.joinArmies.Text = "Объединить армии";
            this.joinArmies.UseVisualStyleBackColor = true;
            // 
            // splitArmy
            // 
            this.splitArmy.AutoSize = true;
            this.splitArmy.Location = new System.Drawing.Point(6, 126);
            this.splitArmy.Name = "splitArmy";
            this.splitArmy.Size = new System.Drawing.Size(116, 17);
            this.splitArmy.TabIndex = 0;
            this.splitArmy.TabStop = true;
            this.splitArmy.Text = "Разделить армию";
            this.splitArmy.UseVisualStyleBackColor = true;
            // 
            // getReserve
            // 
            this.getReserve.AutoSize = true;
            this.getReserve.Location = new System.Drawing.Point(6, 149);
            this.getReserve.Name = "getReserve";
            this.getReserve.Size = new System.Drawing.Size(177, 17);
            this.getReserve.TabIndex = 0;
            this.getReserve.TabStop = true;
            this.getReserve.Text = "Пополнить армию из резерва";
            this.getReserve.UseVisualStyleBackColor = true;
            // 
            // defendRegion
            // 
            this.defendRegion.AutoSize = true;
            this.defendRegion.Location = new System.Drawing.Point(6, 40);
            this.defendRegion.Name = "defendRegion";
            this.defendRegion.Size = new System.Drawing.Size(113, 17);
            this.defendRegion.TabIndex = 0;
            this.defendRegion.TabStop = true;
            this.defendRegion.Text = "Защитить регион";
            this.defendRegion.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(372, 12);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(372, 41);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // regionInfo
            // 
            this.regionInfo.AutoSize = true;
            this.regionInfo.Checked = true;
            this.regionInfo.Location = new System.Drawing.Point(6, 187);
            this.regionInfo.Name = "regionInfo";
            this.regionInfo.Size = new System.Drawing.Size(144, 17);
            this.regionInfo.TabIndex = 0;
            this.regionInfo.TabStop = true;
            this.regionInfo.Text = "Информация о регионе";
            this.regionInfo.UseVisualStyleBackColor = true;
            // 
            // actionGroupBox
            // 
            this.actionGroupBox.Controls.Add(this.moveArmy);
            this.actionGroupBox.Controls.Add(this.attackNearRegion);
            this.actionGroupBox.Controls.Add(this.defendRegion);
            this.actionGroupBox.Controls.Add(this.joinArmies);
            this.actionGroupBox.Controls.Add(this.regionInfo);
            this.actionGroupBox.Controls.Add(this.splitArmy);
            this.actionGroupBox.Controls.Add(this.getReserve);
            this.actionGroupBox.Location = new System.Drawing.Point(12, 12);
            this.actionGroupBox.Name = "actionGroupBox";
            this.actionGroupBox.Size = new System.Drawing.Size(348, 218);
            this.actionGroupBox.TabIndex = 2;
            this.actionGroupBox.TabStop = false;
            this.actionGroupBox.Text = "Действия";
            // 
            // ChooseAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 241);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.actionGroupBox);
            this.Name = "ChooseAction";
            this.Text = "Выберите действие";
            this.actionGroupBox.ResumeLayout(false);
            this.actionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton attackNearRegion;
        private System.Windows.Forms.RadioButton moveArmy;
        private System.Windows.Forms.RadioButton joinArmies;
        private System.Windows.Forms.RadioButton splitArmy;
        private System.Windows.Forms.RadioButton getReserve;
        private System.Windows.Forms.RadioButton defendRegion;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton regionInfo;
        private System.Windows.Forms.GroupBox actionGroupBox;
    }
}