using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PassKeeperEF
{
    public partial class MyCategoriesWindow : Form
    {
        private int UserID;
        private bool mouseDown;
        private Point lastLocation;
        public MyCategoriesWindow(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            try
            {
                (List<Categories> mycategories, Exception err) = DBUtils.GetAllCategoriesByUserID(this.UserID);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                listBox1.DataSource = mycategories;
                listBox1.DisplayMember = "name";
                listBox1.ValueMember = "CategoryID";
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void categorySearchBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err) = DBUtils.GetAllCategoriesByUserID(this.UserID, categorySearchBox.Text);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
                else
                {
                    (List<Categories> mycategories, Exception err) = DBUtils.GetAllCategoriesByUserID(this.UserID);
                    if (err != null)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            Categories myitem = (Categories)listBox1.SelectedItem;

            MyPasswordsWindow passWindow = new MyPasswordsWindow(UserID, myitem.CategoryID);
            this.Hide();
            passWindow.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu(UserID);
            this.Hide();
            menu.ShowDialog();
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Categories myitem = (Categories)listBox1.SelectedItem;
                if (myitem.name == "Uncategorized")
                {
                    MessageBox.Show("Bu kategori default olduğu için silinemez.");
                    return;
                }

                Exception err = DBUtils.DeleteCategoryByID(myitem.CategoryID);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID, categorySearchBox.Text);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
                else
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Categories myitem = (Categories)listBox1.SelectedItem;
            if (myitem.name == "Uncategorized")
            {
                MessageBox.Show("Bu kategori default olduğu için düzenlenemez.");
                return;
            }

            AddEditCat Window = new AddEditCat(UserID, myitem.CategoryID);
            this.Hide();
            Window.ShowDialog();
            try
            {
                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID, categorySearchBox.Text);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
                else
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            this.Show();
        }

        private void addCategory_Click(object sender, EventArgs e)
        {
            AddEditCat Window = new AddEditCat(UserID);
            this.Hide();
            Window.ShowDialog();
            try
            {
                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID, categorySearchBox.Text);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
                else
                {
                    (List<Categories> mycategories, Exception err2) = DBUtils.GetAllCategoriesByUserID(UserID);
                    if (err2 != null)
                    {
                        MessageBox.Show(err2.Message);
                        return;
                    }

                    listBox1.DataSource = mycategories;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "CategoryID";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            this.Show();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Categories myitem = (Categories)listBox1.SelectedItem;
            MyPasswordsWindow passWindow = new MyPasswordsWindow(UserID, myitem.CategoryID);
            this.Hide();
            passWindow.ShowDialog();
            this.Close();
        }

        private void Categories_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Categories_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Categories_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
