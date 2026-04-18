using System;
using System.Drawing;
using System.Windows.Forms;

namespace SafeBox.Presentation.Popup
{
    public class DownloadOptionForm : Form
    {
        private Button btnOriginal;
        private Button btnEncrypted;
        private Button btnCancel;
        private Label lblMessage;

        public DownloadOptionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnOriginal = new System.Windows.Forms.Button();
            this.btnEncrypted = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(20, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(260, 19);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Please select a download format:";
            
            // 
            // btnOriginal
            // 
            this.btnOriginal.Location = new System.Drawing.Point(24, 60);
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(120, 40);
            this.btnOriginal.TabIndex = 1;
            this.btnOriginal.Text = "Original File\n(Decrypted)";
            this.btnOriginal.UseVisualStyleBackColor = true;
            this.btnOriginal.DialogResult = DialogResult.Yes;
            
            // 
            // btnEncrypted
            // 
            this.btnEncrypted.Location = new System.Drawing.Point(160, 60);
            this.btnEncrypted.Name = "btnEncrypted";
            this.btnEncrypted.Size = new System.Drawing.Size(120, 40);
            this.btnEncrypted.TabIndex = 2;
            this.btnEncrypted.Text = "Encrypted File\n(Base64 .txt)";
            this.btnEncrypted.UseVisualStyleBackColor = true;
            this.btnEncrypted.DialogResult = DialogResult.No;
            
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(296, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;

            // 
            // DownloadOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 130);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEncrypted);
            this.Controls.Add(this.btnOriginal);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadOptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Options";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
