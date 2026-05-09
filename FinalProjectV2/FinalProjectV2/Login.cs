using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FinalProjectV2
{
    public partial class Login : Form
    {
        private readonly Color darkBlue = Color.FromArgb(13, 33, 62);
        private readonly Color deepBlue = Color.FromArgb(24, 54, 99);
        private readonly Color accentGold = Color.FromArgb(201, 158, 42);
        private readonly Color textDark = Color.FromArgb(24, 34, 56);
        private readonly Color textMuted = Color.FromArgb(111, 123, 146);
        private readonly Color placeholderColor = Color.FromArgb(154, 163, 178);
        private readonly Color inputBackground = Color.FromArgb(246, 248, 252);

        private const string IdentifierPlaceholder = "Email, phone number, or school ID";
        private const string PasswordPlaceholder = "Password";

        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public Login()
        {
            InitializeComponent();
            InitializeTheme();
            InitializePlaceholders();
            RegisterDragTargets();
        }

        private void InitializeTheme()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            AcceptButton = ButtonLogin;

            PanelRight.BackColor = Color.White;
            PanelIdentifier.BackColor = inputBackground;
            PanelPassword.BackColor = inputBackground;

            TextBoxIdentifier.BackColor = inputBackground;
            TextBoxPassword.BackColor = inputBackground;

            LabelWelcome.ForeColor = textDark;
            LabelSubtitle.ForeColor = textMuted;
            LabelIdentifier.ForeColor = textDark;
            LabelPassword.ForeColor = textDark;
            LabelSecurityNote.ForeColor = textMuted;
            LabelNoAccount.ForeColor = textMuted;

            ButtonLogin.BackColor = darkBlue;
            ButtonLogin.ForeColor = Color.White;

            ButtonMinimize.ForeColor = textDark;
            ButtonClose.ForeColor = textDark;
            CheckBoxShowPassword.ForeColor = textMuted;

            LabelAppName.BackColor = Color.Transparent;
            LabelTagline.BackColor = Color.Transparent;
            LabelFeatureTitle.BackColor = Color.Transparent;
            LabelFeature1.BackColor = Color.Transparent;
            LabelFeature2.BackColor = Color.Transparent;
            LabelFeature3.BackColor = Color.Transparent;
            LabelFooterLeft.BackColor = Color.Transparent;

            ApplyRoundedLayout();
        }

        private void InitializePlaceholders()
        {
            TextBoxIdentifier.Tag = IdentifierPlaceholder;
            TextBoxPassword.Tag = PasswordPlaceholder;

            SetPlaceholder(TextBoxIdentifier);
            SetPlaceholder(TextBoxPassword);
        }

        private void RegisterDragTargets()
        {
            RegisterDrag(this);
            RegisterDrag(PanelLeft);
            RegisterDrag(PanelRight);
            RegisterDrag(LabelAppName);
            RegisterDrag(LabelTagline);
            RegisterDrag(LabelPortal);
            RegisterDrag(LabelWelcome);
            RegisterDrag(LabelSubtitle);
        }

        private void RegisterDrag(Control control)
        {
            if (control != null)
            {
                control.MouseDown += DragWindow;
            }
        }

        private void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WmNclButtonDown, HtCaption, 0);
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            ApplyRoundedLayout();
        }

        private void Login_Resize(object sender, EventArgs e)
        {
            ApplyRoundedLayout();
        }

        private void ApplyRoundedLayout()
        {
            ApplyRoundedForm(26);
            ApplyRoundedRegion(PanelIdentifier, 18);
            ApplyRoundedRegion(PanelPassword, 18);
            ApplyRoundedRegion(ButtonLogin, 18);
        }

        private void ApplyRoundedForm(int radius)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, Width, Height), radius))
            {
                Region = new Region(path);
            }
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region = new Region(path);
            }
        }

        private GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            Rectangle arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);

            path.StartFigure();
            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void SetPlaceholder(TextBox textBox)
        {
            string placeholder = Convert.ToString(textBox.Tag);

            textBox.ForeColor = placeholderColor;
            textBox.Text = placeholder;

            if (textBox == TextBoxPassword)
            {
                textBox.UseSystemPasswordChar = false;
            }
        }

        private bool IsPlaceholderActive(TextBox textBox)
        {
            string placeholder = Convert.ToString(textBox.Tag);
            return string.Equals(textBox.Text, placeholder, StringComparison.Ordinal);
        }

        private void InputTextBoxEnter(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && IsPlaceholderActive(textBox))
            {
                textBox.Text = string.Empty;
                textBox.ForeColor = textDark;

                if (textBox == TextBoxPassword)
                {
                    textBox.UseSystemPasswordChar = !CheckBoxShowPassword.Checked;
                }
            }
        }

        private void InputTextBoxLeave(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetPlaceholder(textBox);
            }
        }

        private void CheckBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (IsPlaceholderActive(TextBoxPassword))
            {
                TextBoxPassword.UseSystemPasswordChar = false;
                return;
            }

            TextBoxPassword.UseSystemPasswordChar = !CheckBoxShowPassword.Checked;
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string userInput = IsPlaceholderActive(TextBoxIdentifier)
                ? string.Empty
                : TextBoxIdentifier.Text.Trim();

            string password = IsPlaceholderActive(TextBoxPassword)
                ? string.Empty
                : TextBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(userInput) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter your email, phone number, or school ID, and your password.",
                    "SchoolarLink",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            MessageBox.Show(
                "Login page is ready.\n\nConnect this button to your scholarship account database or API.\n\nUser: " + userInput,
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void LinkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "Connect this link to your password recovery flow.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void LinkLabelSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "Connecting to Sign Up Form.",
                "SchoolarLink",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            {
                using (CreateAcc createAcc = new CreateAcc())
                {
                    createAcc.ShowDialog(this);
                }
            }
        }


        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PanelLeft_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = PanelLeft.ClientRectangle;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                darkBlue,
                deepBlue,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(glowBrush, 15, 8, 110, 110);
                e.Graphics.FillEllipse(glowBrush, 220, 18, 145, 160);
                e.Graphics.FillEllipse(glowBrush, 205, 315, 150, 150);
                e.Graphics.FillEllipse(glowBrush, -90, 462, 220, 220);
            }
        }
    }
}
