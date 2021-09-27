using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PassKeeperEFWeb
{
    public partial class MyPasswordsWindow : Form
    {
        private int CategoryID;
        private bool mouseDown;
        private Point lastLocation;

        public MyPasswordsWindow(int CategoryID = -1)
        {
            InitializeComponent();
            this.CategoryID = CategoryID;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Passwords_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.CategoryID == -1)
                {
                    (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords();

                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    listBox1.DataSource = passwords;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "PasswordID";
                }
                else
                {
                    (Categories category, Exception error) = WebUtils.GetCategory(categoryID: CategoryID);
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords(categoryName: category.name);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    listBox1.DataSource = passwords;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "PasswordID";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.CategoryID == -1)
                {
                    if (!string.IsNullOrWhiteSpace(nameBox.Text))
                    {
                        (List<Passwords> mypasswords, Exception err) = WebUtils.GetAllPasswords(like: nameBox.Text);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = mypasswords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }

                    else
                    {
                        (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords();
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = passwords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }
                }
                else
                {
                    (Categories category, Exception error) = WebUtils.GetCategory(categoryID: CategoryID);
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(nameBox.Text))
                    {
                        (List<Passwords> mypasswords, Exception err) = WebUtils.GetAllPasswords(category.name, nameBox.Text);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = mypasswords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }

                    else
                    {
                        (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords(categoryName: category.name);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = passwords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            try
            {
                Passwords myitem = (Passwords)listBox1.SelectedItem;
                ShowPass passWindow = new ShowPass(myitem.name, Utils.Decrypt(myitem.password));
                this.Hide();
                passWindow.ShowDialog();
                this.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Passwords myitem = (Passwords)listBox1.SelectedItem;
                Exception error = WebUtils.DeletePassword(myitem.name);
                if (error != null)
                {
                    MessageBox.Show(error.Message);
                    return;
                }

                if (this.CategoryID == -1)
                {
                    if (!string.IsNullOrWhiteSpace(nameBox.Text))
                    {
                        (List<Passwords> mypasswords, Exception err) = WebUtils.GetAllPasswords(like: nameBox.Text);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = mypasswords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }

                    else
                    {
                        (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords();
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = passwords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }
                }
                else
                {
                    (Categories category, Exception err2) = WebUtils.GetCategory(categoryID: CategoryID);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(nameBox.Text))
                    {
                        (List<Passwords> mypasswords, Exception err) = WebUtils.GetAllPasswords(category.name, nameBox.Text);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = mypasswords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }

                    else
                    {
                        (List<Passwords> passwords, Exception err) = WebUtils.GetAllPasswords(categoryName: category.name);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        listBox1.DataSource = passwords;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "PasswordID";
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Passwords myitem = (Passwords)listBox1.SelectedItem;
            (Passwords pass, Exception err) = WebUtils.GetPassword(PasswordName: myitem.name);

            if(err != null)
            {
                MessageBox.Show(err.Message);
                return;
            }

            MainMenu editMenu = new MainMenu(pass.PasswordID);
            this.Hide();
            editMenu.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu editMenu = new MainMenu();
            this.Hide();
            editMenu.ShowDialog();
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Passwords myitem = (Passwords)listBox1.SelectedItem;
                ShowPass passWindow = new ShowPass(myitem.name, Utils.Decrypt(myitem.password));
                this.Hide();
                passWindow.ShowDialog();
                this.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Passwords_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Passwords_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Passwords_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void newPassword_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
            this.Hide();
            menu.ShowDialog();
            this.Close();
        }
    }
}

