using TestTop.Core;

namespace TestTop.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.desktopButton = new System.Windows.Forms.Button();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.desktopControl1 = new TestTop.Core.DesktopControl();
            this.SuspendLayout();
            // 
            // desktopButton
            // 
            this.desktopButton.Location = new System.Drawing.Point(159, 10);
            this.desktopButton.Name = "desktopButton";
            this.desktopButton.Size = new System.Drawing.Size(122, 23);
            this.desktopButton.TabIndex = 1;
            this.desktopButton.Text = "Switch Desktop";
            this.desktopButton.UseVisualStyleBackColor = true;
            this.desktopButton.Click += new System.EventHandler(this.DesktopButton_Click);
            // 
            // comboBox
            // 
            this.comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(12, 12);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(121, 21);
            this.comboBox.TabIndex = 2;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(297, 10);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(142, 23);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // desktopControl1
            // 
            this.desktopControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.desktopControl1.Location = new System.Drawing.Point(0, 66);
            this.desktopControl1.Name = "desktopControl1";
            this.desktopControl1.Size = new System.Drawing.Size(1046, 461);
            this.desktopControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1046, 527);
            this.Controls.Add(this.desktopControl1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.desktopButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button desktopButton;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button SaveButton;
        private DesktopControl desktopControl1;
        //private DesktopControl userControl11;
    }
}

