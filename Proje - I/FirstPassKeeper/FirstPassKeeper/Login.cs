using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FirstPassKeeper
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

            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string openUsers = string.Format("IF OBJECT_ID('users', 'U') IS NULL CREATE TABLE users (id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, username nvarchar(255), password nvarchar(255))");
                SqlCommand openUsersCmd = new SqlCommand(openUsers, connection);
                openUsersCmd.ExecuteNonQuery();

                string loginSql = "SELECT * FROM users WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(loginSql, connection);
                cmd.Parameters.AddWithValue("@username", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", Utils.Encrypt(textBox2.Text));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    MainMenu Menu = new MainMenu(textBox1.Text);
                    this.Hide();
                    Menu.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Verilen Bilgiler Veri Tabanındakiler ile Eşleşmiyor.");
                }

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

            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string openUsers = string.Format("IF OBJECT_ID('users', 'U') IS NULL CREATE TABLE users (id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, username nvarchar(255), password nvarchar(255))");
                SqlCommand openUsersCmd = new SqlCommand(openUsers, connection);
                openUsersCmd.ExecuteNonQuery();

                string loginSql = "SELECT * FROM users WHERE username = @username";
                SqlCommand cmd = new SqlCommand(loginSql, connection);
                cmd.Parameters.AddWithValue("@username", textBox1.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Bu kullanıcı adı alınmış");
                }
                else
                {
                    reader.Close();
                    string registerSql = "INSERT INTO users (username, password) VALUES(@username, @password)";
                    SqlCommand registerCmd = new SqlCommand(registerSql, connection);
                    registerCmd.Parameters.AddWithValue("@username", textBox1.Text);
                    registerCmd.Parameters.AddWithValue("@password", Utils.Encrypt(textBox2.Text));
                    registerCmd.ExecuteNonQuery();

                    string openTable = string.Format("IF OBJECT_ID('{0}', 'U') IS NULL CREATE TABLE {0} (name nvarchar(255), password nvarchar(255), category nvarchar(255))", textBox1.Text);
                    SqlCommand openTableCmd = new SqlCommand(openTable, connection);
                    openTableCmd.ExecuteNonQuery();

                    string categoryTable = string.Format("IF OBJECT_ID('{0}C', 'U') IS NULL CREATE TABLE {0}C (categories nvarchar(255))", textBox1.Text);
                    SqlCommand categoryTableCmd = new SqlCommand(categoryTable, connection);
                    categoryTableCmd.ExecuteNonQuery();

                    string addUncategorized = string.Format("INSERT INTO {0}C VALUES('Uncategorized')", textBox1.Text);
                    SqlCommand addUncategorizedCmd = new SqlCommand(addUncategorized, connection);
                    addUncategorizedCmd.ExecuteNonQuery();

                    MessageBox.Show("Başarıyla kayıt oldun!");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
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
                Utils.ImportFromJSON(filePath);
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