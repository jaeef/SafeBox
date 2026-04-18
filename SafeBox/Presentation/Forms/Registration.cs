using System;
using System.Windows.Forms;
using SafeBox.Application.Interfaces;
using SafeBox.Presentation;
using SafeBox.Domain.Exceptions;

namespace SafeBox.Presentation.Forms
{
    public partial class Registration : Form
    {
        private readonly IAuthService _authService;

        public Registration(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (txtPass.Content != txtConfirmPass.Content)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _authService.Register(txtName.Content.Trim(), txtEmail.Content.Trim(), txtPass.Content);

                MessageBox.Show("Account Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open Login form
                Login login = new Login(_authService);
                login.Show();
                this.Hide();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (BusinessRuleException ex)
            {
                MessageBox.Show(ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login login = new Login(_authService);
            login.Show();
            this.Hide();
        }
    }
}
