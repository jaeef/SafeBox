using System;
using System.Windows.Forms;
using SafeBox.Application.DTOs;
using SafeBox.Application.Interfaces;
using SafeBox.Application.Services;
using SafeBox.Presentation.Admin;
using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Repositories;
using SafeBox.Infrastructure.Services;
using SafeBox.Domain.Exceptions;

namespace SafeBox.Presentation.Forms
{
    public partial class Login : Form
    {
        private readonly IAuthService _authService;

        public Login(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            this.txtLoginEmail.KeyDown += InputFields_KeyDown;
            this.txtLoginPass.KeyDown += InputFields_KeyDown;
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string username = txtLoginEmail.Text.Trim();
            string password = txtLoginPass.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter Username and Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Try Admin Login first
                try 
                {
                    Domain.Entities.Admin admin = _authService.AdminLogin(username, password);
                    if (admin != null)
                    {
                        SessionManager.CurrentAdmin = admin;
                        MessageBox.Show($"Login Successful! Welcome Admin {admin.AdminUsername}.", "Admin Access", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new AdminDashboard().Show();
                        this.Hide();
                        return;
                    }
                }
                catch (UnauthorizedException) 
                { 
                    // Fallthrough to User Login if admin login fails (or handle distinct if username prefix known)
                    // For now, we assume if AdminLogin fails (auth), it might be a regular user.
                }

                UserDto user = _authService.Login(username, password);

                SessionManager.CurrentUser = new User
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Status = user.Status,
                    RoleId = user.RoleId,
                    PasswordHash = user.PasswordHash
                };

                MessageBox.Show($"Login Successful! Welcome {user.Username}.", "User Access", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var vaultService = new VaultService();
                var fileService = new FileService();
                var activityService = new ActivityService();
                var userService = new UserService();
                var cryptoService = new CryptoService();

                DashBoardMain dashboard = new DashBoardMain(
                    _authService, vaultService, fileService,
                    activityService, userService, cryptoService);
                dashboard.Show();
                this.Hide();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedException ex)
            {
                MessageBox.Show(ex.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (BusinessRuleException ex)
            {
                MessageBox.Show(ex.Message, "Account Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblGoToRegister_Click(object sender, EventArgs e)
        {
            Registration regForm = new Registration(_authService);
            regForm.Show();
            this.Hide();
        }

        private void lblForgetPass_Click(object sender, EventArgs e)
        {
            ResetPass resetPass = new ResetPass();
            resetPass.Owner = this;
            resetPass.Show();
            this.Hide();
        }

        private void txtLoginEmail_Click(object sender, EventArgs e)
        {
        }

        private void InputFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSignIn_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
