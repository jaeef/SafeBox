using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SafeBox.Presentation.Admin
{
    public partial class ListItem_UserRow : UserControl
    {
        // Events
        // Events
        public event Action<int> UserActivated; // userId
        public event Action<int> UserDeactivated; // userId

        private static readonly Random r = new Random();
        private int _userId;
        private string _status;

        public ListItem_UserRow()
        {
            InitializeComponent();
            // Delete/Deactivate button
            // Delete/Deactivate button
            btnDelet.Click += btnDelet_Click;
            
            // Activate/Restore button
            btnReset.Click += btnActivate_Click;
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
             if(MessageBox.Show("Are you sure you want to deactivate this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                UserDeactivated?.Invoke(_userId);
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
             if(MessageBox.Show("Are you sure you want to activate/restore this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                UserActivated?.Invoke(_userId);
        }

        // ???? ????? ???? ?????
        public void SetData(string initial, string name, string email, string role, string status, string lastLogin, string created, int userId = 0)
        {
            _userId = userId;
            _status = status;

            lblName.Content = name;
            lblEmail.Content = email;
            lblRole.Content = role;
            lblStatus.Content = status;
            lblLogin.Content = lastLogin;
            lblCreated.Content = created;

            // ?. ??? ???? ??? ???
            Color bg = GetRandomColor();
            picAvatar.Image = GenerateAvatar(initial, bg);

            // ?. ????????? ????? ??? ???? ??????????
            // Hide buttons for admin users
            if (role.ToUpper() == "ADMIN")
            {
                btnDelet.Visible = false;
                btnReset.Visible = false;
                lblStatus.ForeColor = status.ToLower() == "inactive" ? Color.Salmon : Color.SpringGreen;
            }
            else if (status.ToLower() == "inactive")
            {
                lblStatus.ForeColor = Color.Salmon;
                btnDelet.Visible = false;   // Hide Deactivate
                btnReset.Visible = true;    // Show Activate
            }
            else
            {
                lblStatus.ForeColor = Color.SpringGreen;
                btnDelet.Visible = true;    // Show Deactivate
                btnReset.Visible = false;   // Hide Activate
            }
        }

        // --- ??? ???? ???????? (???? ????) ---
        private Bitmap GenerateAvatar(string text, Color bgColor)
        {
            if (picAvatar.Width == 0) return null;
            Bitmap bmp = new Bitmap(picAvatar.Width, picAvatar.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                using (Brush brush = new SolidBrush(bgColor))
                {
                    g.FillEllipse(brush, 0, 0, picAvatar.Width - 1, picAvatar.Height - 1);
                }
                using (Font font = new Font("Segoe UI", 10, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, Brushes.White, (picAvatar.Width - textSize.Width) / 2, (picAvatar.Height - textSize.Height) / 2);
                }
            }
            return bmp;
        }

        private Color GetRandomColor()
        {
            Color[] colors = { Color.Orange, Color.SeaGreen, Color.DodgerBlue, Color.Purple, Color.Crimson };
            return colors[r.Next(colors.Length)];
        }
    }
}
