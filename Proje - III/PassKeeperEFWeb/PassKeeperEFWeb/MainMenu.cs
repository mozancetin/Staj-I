using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PassKeeperEFWeb
{
    public partial class MainMenu : Form
    {
        private int PasswordID;
        private bool mouseDown;
        private Point lastLocation;

        public MainMenu(int PasswordID = -1)
        {
            InitializeComponent();
            this.PasswordID = PasswordID;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ActiveControl = name;
            if (PasswordID == -1)
            {
                try
                {
                    (List<Categories> categories, Exception err) = WebUtils.GetAllCategories();
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

                    (Passwords pass, Exception err) = WebUtils.GetPassword(PasswordID: PasswordID);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                    }

                    nameBox.Text = pass.name;
                    passwordBox.Text = Utils.Decrypt(pass.password);
                    
                    (List<Categories> categories, Exception err2) = WebUtils.GetAllCategories();
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message + "2");
                        return;
                    }
                    (Categories category, Exception err3) = WebUtils.GetCategory(categoryID: pass.CategoryID);
                    if (err3 != null)
                    {
                        MessageBox.Show(err3.Message + "3");
                        return;
                    }
                    /*
                    categories.ForEach(c => { Console.WriteLine(c.CategoryID); });
                    Console.WriteLine("---------------" + "\n" + category.CategoryID);*/
                    comboBox1.DataSource = categories;
                    comboBox1.DisplayMember = "name";
                    comboBox1.ValueMember = "CategoryID";
                    comboBox1.SelectedIndex = categories.IndexOf(categories.Where(c => c.name == category.name && c.UserID == category.UserID).First());
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message + "4");
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
                    (Categories category, Exception err) = WebUtils.GetCategory(categoryName: categoryName);
                    if (err != null)
                    {
                        Exception err2 = WebUtils.AddCategory(categoryName);
                        if (err2 != null)
                        {
                            MessageBox.Show(err2.Message);
                            return;
                        }
                        (List<Categories> categories, Exception err3) = WebUtils.GetAllCategories();
                        if (err3 != null)
                        {
                            MessageBox.Show(err3.Message);
                            return;
                        }
                        (category, err) = WebUtils.GetCategory(categoryName: categoryName);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        comboBox1.DataSource = categories;
                        comboBox1.DisplayMember = "name";
                        comboBox1.ValueMember = "CategoryID";
                        comboBox1.SelectedIndex = categories.IndexOf(categories.Where(c => c.name == category.name && c.UserID == category.UserID).First());
                    }
                    
                    err = WebUtils.AddPassword(programName, programPassword, categoryName);
                    if (err != null)
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
            else
            {
                try
                {
                    (Passwords pass, Exception error) = WebUtils.GetPassword(PasswordID: PasswordID);
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    string programName = nameBox.Text;
                    string programPassword = passwordBox.Text;
                    
                    (Categories newCategory, Exception err) = WebUtils.GetCategory(categoryName: comboBox1.Text);
                    Exception err2 = new Exception();
                    if (err != null)
                    {
                        err2 = WebUtils.AddCategory(comboBox1.Text);
                        if (err2 != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        (newCategory, err2) = WebUtils.GetCategory(categoryName: comboBox1.Text);
                        if (err2 != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }
                    }
                    
                    if (pass.name != programName)
                    {
                        err = WebUtils.UpdatePassword(pass.name, programName, programPassword, newCategory.name);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        MessageBox.Show("Bilgiler başarıyla güncellendi.");
                    }
                    else
                    {
                        err = WebUtils.UpdatePassword(pass.name, null, programPassword, newCategory.name);
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
            MyPasswordsWindow PasswordsWindow = new MyPasswordsWindow();
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
            MyCategoriesWindow categoryWindow = new MyCategoriesWindow();
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
                WebUtils.Export(saveFileDialog1.FileName);
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