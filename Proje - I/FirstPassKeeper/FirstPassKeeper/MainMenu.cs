using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public partial class MainMenu : Form
    {
        private string user;
        private string progName;
        private bool mouseDown;
        private Point lastLocation;


        public MainMenu(string user, string progName = null)
        {
            InitializeComponent();
            this.user = user;
            this.progName = progName;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ActiveControl = name;
            if (this.progName == null)
            {
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                try
                {
                    SqlConnection connection = new SqlConnection(source);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string getCategoriesSql = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategoriesSql, connection);
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Uncategorized";
                    comboBox1.ValueMember = "categories";
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                try
                {
                    SqlConnection connection = new SqlConnection(source);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string getDetails = string.Format("SELECT * FROM {0} WHERE name = @progName", this.user);
                    SqlCommand getDetailsCmd = new SqlCommand(getDetails, connection);
                    getDetailsCmd.Parameters.AddWithValue("@progName", this.progName);
                    SqlDataReader reader = getDetailsCmd.ExecuteReader();
                    reader.Read();
                    string progPass = Utils.Decrypt(reader.GetString(1));
                    string progCategory = reader.GetString(2);
                    reader.Close();

                    nameBox.Text = this.progName;
                    passwordBox.Text = progPass;

                    string getCategoriesSql = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategoriesSql, connection);
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = progCategory;
                    List<string> items = new List<string>();
                    foreach (DataRowView obj in comboBox1.Items.Cast<DataRowView>().ToList())
                    {
                        items.Add(obj.Row.ItemArray[0].ToString());
                    }
                    comboBox1.SelectedIndex = items.IndexOf(progCategory);
                    comboBox1.ValueMember = "categories";
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.progName == null)
            {
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                string programName = nameBox.Text;
                string programPassword = passwordBox.Text;
                string categoryName = comboBox1.Text;
                if (programName.Replace(" ", "") == "" || programPassword.Replace(" ", "") == "" || categoryName.Replace(" ", "") == "")
                {
                    MessageBox.Show("Program adı, şifre ve kategori boş bırakılamaz!");
                    return;
                }

                try
                {
                    SqlConnection connection = new SqlConnection(source);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    string isExists = string.Format("SELECT * FROM {0} WHERE name = @name", this.user);
                    SqlCommand isExistsCmd = new SqlCommand(isExists, connection);
                    isExistsCmd.Parameters.AddWithValue("@name", programName);

                    SqlDataReader existsReader = isExistsCmd.ExecuteReader();
                    if (existsReader.Read())
                    {
                        MessageBox.Show("Böyle bir program zaten kaydedilmiş.");
                        return;
                    }

                    existsReader.Close();

                    string saveSql = string.Format("INSERT INTO {0} (name, password, category) VALUES(@progName, @progPass, @catName)", this.user);
                    SqlCommand saveCmd = new SqlCommand(saveSql, connection);
                    saveCmd.Parameters.AddWithValue("@progName", programName);
                    saveCmd.Parameters.AddWithValue("@progPass", Utils.Encrypt(programPassword));
                    saveCmd.Parameters.AddWithValue("@catName", categoryName);

                    saveCmd.ExecuteNonQuery();

                    string saveCategorySql = string.Format("SELECT * FROM {0}C WHERE categories = @category", this.user);
                    SqlCommand saveCategoryCmd = new SqlCommand(saveCategorySql, connection);
                    saveCategoryCmd.Parameters.AddWithValue("@category", categoryName);
                    SqlDataReader reader = saveCategoryCmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        reader.Close();
                        string savingSql = string.Format("INSERT INTO {0}C VALUES(@category)", this.user);
                        SqlCommand savingCmd = new SqlCommand(savingSql, connection);
                        savingCmd.Parameters.AddWithValue("@category", categoryName);
                        savingCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Şifre Başarıyla Kaydedildi!");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                try
                {
                    string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                    string programName = nameBox.Text;
                    string programPassword = passwordBox.Text;
                    string categoryName = comboBox1.Text;

                    SqlConnection connection = new SqlConnection(source);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    if (this.progName != programName)
                    {
                        string deleteSql = string.Format("DELETE FROM {0} WHERE name = @progName", this.user);
                        SqlCommand deleteCmd = new SqlCommand(deleteSql, connection);
                        deleteCmd.Parameters.AddWithValue("@progName", this.progName);
                        deleteCmd.ExecuteNonQuery();

                        string saveSql = string.Format("INSERT INTO {0} (name, password, category) VALUES(@name, @password, @category)", this.user);
                        SqlCommand saveCmd = new SqlCommand(saveSql, connection);
                        saveCmd.Parameters.AddWithValue("@name", programName);
                        saveCmd.Parameters.AddWithValue("@password", Utils.Encrypt(programPassword));
                        saveCmd.Parameters.AddWithValue("@category", categoryName);

                        saveCmd.ExecuteNonQuery();
                        MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    }
                    else
                    {
                        string updateSql = string.Format("UPDATE {0} SET password = @password, category = @category WHERE name = @name", this.user);
                        SqlCommand updateCmd = new SqlCommand(updateSql, connection);
                        updateCmd.Parameters.AddWithValue("@password", Utils.Encrypt(programPassword));
                        updateCmd.Parameters.AddWithValue("@category", categoryName);
                        updateCmd.Parameters.AddWithValue("@name", this.progName);

                        updateCmd.ExecuteNonQuery();
                        MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void allPasswordsButton_Click(object sender, EventArgs e)
        {
            Passwords PasswordsWindow = new Passwords(this.user);
            this.Hide();
            PasswordsWindow.ShowDialog();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passwordBox.PasswordChar = '\0';
            }
            else
            {
                passwordBox.PasswordChar = '\u25CF';
            }
        }

        private void categoriesButton_Click(object sender, EventArgs e)
        {
            Categories categoryWindow = new Categories(this.user);
            this.Hide();
            categoryWindow.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            passwordBox.Text = Utils.GetRandomPassword(12);
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON File (*.json)|*.json";
            saveFileDialog1.Title = "JSON Olarak Kaydet";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                Utils.ExportAsJSON(saveFileDialog1.FileName);
            }

        }

        private void MainMenu_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void MainMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void MainMenu_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}