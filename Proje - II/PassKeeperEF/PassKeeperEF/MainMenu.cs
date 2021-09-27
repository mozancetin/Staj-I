using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PassKeeperEF
{
    public partial class MainMenu : Form
    {
        private int userid;
        private int PasswordID;
        private bool mouseDown;
        private Point lastLocation;


        public MainMenu(int UserID, int PasswordID = -1)
        {
            InitializeComponent();
            this.userid = UserID;
            this.PasswordID = PasswordID;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ActiveControl = name;
            if (PasswordID == -1)
            {
                try
                {
                    (List<Categories> categories, Exception err) = DBUtils.GetAllCategoriesByUserID(userid);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    comboBox1.DataSource = categories;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "CategoryID";
                    comboBox1.SelectedIndex = 0;
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
                    (Passwords pass, Exception err) = DBUtils.GetPasswordByID(PasswordID);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                    }

                    
                    nameBox.Text = pass.name;
                    passwordBox.Text = Utils.Decrypt(pass.password);

                    (List<Categories> categories, Exception err2) = DBUtils.GetAllCategoriesByUserID(userid);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    (Categories category, Exception err3) = DBUtils.GetCategoryByCategoryID(pass.CategoryID);
                    if (err3 != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    comboBox1.DataSource = categories;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "CategoryID";
                    comboBox1.SelectedIndex = categories.IndexOf(category);
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
            if (PasswordID == -1)
            {
                string programName = nameBox.Text;
                string programPassword = passwordBox.Text;
                string categoryName = comboBox1.Text;
                if (string.IsNullOrEmpty(programName) || string.IsNullOrEmpty(programPassword) || string.IsNullOrEmpty(categoryName))
                {
                    MessageBox.Show("Program adı, şifre ve kategori boş bırakılamaz!");
                    return;
                }

                try
                {
                    Exception err3 = new Exception();
                    (Categories category, Exception err) = DBUtils.GetCategoryByName(categoryName, userid);
                    if (err != null)
                    {
                        Exception err2 = DBUtils.AddCategory(categoryName, userid);
                        if (err2 != null)
                        {
                            MessageBox.Show(err2.Message);
                            return;
                        }
                        (category, err3) = DBUtils.GetCategoryByName(categoryName, userid);
                        if (err3 != null)
                        {
                            MessageBox.Show(err3.Message);
                            return;
                        }
                    }

                    err = DBUtils.AddPassword(programName, programPassword, userid, category.CategoryID);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    MessageBox.Show("Şifre Başarıyla Kaydedildi!");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    (Passwords pass, Exception error) = DBUtils.GetPasswordByID(PasswordID);
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    string programName = nameBox.Text;
                    string programPassword = passwordBox.Text;
                    (Categories newCategory, Exception err) = DBUtils.GetCategoryByName(comboBox1.Text, userid);
                    Exception err2 = new Exception();
                    if (err != null)
                    {
                        err2 = DBUtils.AddCategory(comboBox1.Text, userid);
                        if (err2 != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        (newCategory, err2) = DBUtils.GetCategoryByName(comboBox1.Text, userid);
                        if (err2 != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }
                    }

                    if (pass.name != programName)
                    {
                        err = DBUtils.UpdatePasswordByID(PasswordID, programName, programPassword, newCategory.CategoryID);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    }
                    else
                    {
                        err = DBUtils.UpdatePasswordByID(PasswordID, null, programPassword, newCategory.CategoryID);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }
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
            MyPasswordsWindow PasswordsWindow = new MyPasswordsWindow(userid);
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
            MyCategoriesWindow categoryWindow = new MyCategoriesWindow(userid);
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