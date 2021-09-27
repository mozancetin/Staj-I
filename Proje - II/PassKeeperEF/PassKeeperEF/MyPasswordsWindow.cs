using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace PassKeeperEF
{
    public partial class MyPasswordsWindow : Form
    {
        private int UserID;
        private int CategoryID;
        private bool mouseDown;
        private Point lastLocation;

        public MyPasswordsWindow(int UserID, int CategoryID = -1)
        {
            InitializeComponent();
            this.UserID = UserID;
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
                    (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByUserID(UserID);
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
                    (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByCategoryID(CategoryID, UserID);
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
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        (List<Passwords> mypasswords, Exception err) = DBUtils.GetAllPasswordsByUserID(UserID, nameBox.Text);
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
                        (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByUserID(UserID);
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
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        (List<Passwords> mypasswords, Exception err) = DBUtils.GetAllPasswordsByCategoryID(CategoryID, UserID, nameBox.Text);
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
                        (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByCategoryID(CategoryID, UserID);
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
                Exception error = DBUtils.DeletePasswordByID(myitem.PasswordID);
                if (error != null)
                {
                    MessageBox.Show(error.Message);
                    return;
                }

                if (this.CategoryID == -1)
                {
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        (List<Passwords> mypasswords, Exception err) = DBUtils.GetAllPasswordsByUserID(UserID, nameBox.Text);
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
                        (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByUserID(UserID);
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
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        (List<Passwords> mypasswords, Exception err) = DBUtils.GetAllPasswordsByCategoryID(CategoryID, UserID, nameBox.Text);
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
                        (List<Passwords> passwords, Exception err) = DBUtils.GetAllPasswordsByCategoryID(CategoryID, UserID);
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
            MainMenu editMenu = new MainMenu(UserID, myitem.PasswordID);
            this.Hide();
            editMenu.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu editMenu = new MainMenu(UserID);
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
            MainMenu menu = new MainMenu(UserID);
            this.Hide();
            menu.ShowDialog();
            this.Close();
        }
    }
}

