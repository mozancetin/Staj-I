using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PassKeeperEFWeb
{
    public partial class Login : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!");
                    return;
                }

                Exception err = WebUtils.Login(textBox1.Text, textBox2.Text);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                MainMenu Menu = new MainMenu();
                this.Hide();
                Menu.ShowDialog();
                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!");
                return;
            }

            if (textBox1.Text.Contains(" ") || textBox2.Text.Contains(" "))
            {
                MessageBox.Show("Kullanıcı adı ve şifre içerisinde boşluk karakteri bulunamaz!");
                return;
            }
            try
            {
                Exception err = WebUtils.SignIn(textBox1.Text, textBox2.Text);
                if(err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(103, 197, 255);
            closeButton.ForeColor = Color.FromArgb(103, 197, 255);
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            closeButton.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '\u25CF';
            }
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "JSON File (*.json)|*.json";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            if (filePath != string.Empty)
            {
                Exception err = WebUtils.Import(filePath);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}