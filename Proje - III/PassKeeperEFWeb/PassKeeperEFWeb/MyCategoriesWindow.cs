using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PassKeeperEFWeb
{
    public partial class MyCategoriesWindow : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public MyCategoriesWindow()
        {
            InitializeComponent();
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            try
            {
                (List<Categories> categories, Exception err) = WebUtils.GetAllCategories();
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                listBox1.DataSource = categories;
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
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories(categorySearchBox.Text);
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
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories();
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

            (Categories category, Exception err) = WebUtils.GetCategory(categoryName: myitem.name);
            if (err != null)
            {
                MessageBox.Show(err.Message);
                return;
            }

            MyPasswordsWindow passWindow = new MyPasswordsWindow(category.CategoryID);
            this.Hide();
            passWindow.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu();
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
                List<Categories> mycategories = new List<Categories>();
                Categories myitem = (Categories)listBox1.SelectedItem;
                if (myitem.name == "Uncategorized")
                {
                    MessageBox.Show("Bu kategori default olduğu için silinemez.");
                    return;
                }

                Exception err = WebUtils.DeleteCategory(myitem.name);
                if (err != null)
                {
                    MessageBox.Show(err.Message);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (mycategories, err) = WebUtils.GetAllCategories(categorySearchBox.Text);
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
                    (mycategories, err) = WebUtils.GetAllCategories();
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

        private void editButton_Click(object sender, EventArgs e)
        {
            Categories myitem = (Categories)listBox1.SelectedItem;
            if (myitem.name == "Uncategorized")
            {
                MessageBox.Show("Bu kategori default olduğu için düzenlenemez.");
                return;
            }

            (Categories category, Exception error) = WebUtils.GetCategory(categoryName: myitem.name);
            if (error != null)
            {
                MessageBox.Show(error.Message);
                return;
            }

            AddEditCat Window = new AddEditCat(category.CategoryID);
            this.Hide();
            Window.ShowDialog();
            try
            {
                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories(categorySearchBox.Text);
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
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories();
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
            catch (Exception error2)
            {
                MessageBox.Show(error2.Message);
            }
            this.Show();
        }

        private void addCategory_Click(object sender, EventArgs e)
        {
            AddEditCat Window = new AddEditCat();
            this.Hide();
            Window.ShowDialog();
            try
            {
                if (!string.IsNullOrWhiteSpace(categorySearchBox.Text))
                {
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories(categorySearchBox.Text);
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
                    (List<Categories> mycategories, Exception err) = WebUtils.GetAllCategories();
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
            this.Show();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Categories myitem = (Categories)listBox1.SelectedItem;
            (Categories category, Exception err) = WebUtils.GetCategory(categoryName: myitem.name);
            if(err != null)
            {
                MessageBox.Show(err.Message);
                return;
            }

            MyPasswordsWindow passWindow = new MyPasswordsWindow(category.CategoryID);
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
