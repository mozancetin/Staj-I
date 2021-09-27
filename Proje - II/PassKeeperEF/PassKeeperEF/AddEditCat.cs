using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PassKeeperEF
{
    public partial class AddEditCat : Form
    {
        private int UserID;
        private int CategoryID;
        private bool mouseDown;
        private Point lastLocation;
        public AddEditCat(int UserID, int CategoryID = -1)
        {
            InitializeComponent();
            this.UserID = UserID;
            this.CategoryID = CategoryID;
        }

        private void AddEditCat_Load(object sender, EventArgs e)
        {
            if (CategoryID == -1)
            {
                label1.Text = "Yeni Kategori İsmi";
                Button1.Text = "Kategori Ekle";
            }
            else
            {
                label1.Text = "Yeni Kategori İsmi";
                Button1.Text = "Yeniden Adlandır";
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (CategoryID == -1)
                {
                    (_, Exception err) = DBUtils.GetCategoryByName(textBox1.Text, UserID);
                    if (err == null)
                    {
                        MessageBox.Show("Böyle bir kategori zaten var!");
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        err = DBUtils.AddCategory(textBox1.Text, UserID);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        MessageBox.Show("Kategori Eklendi!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kategori Adı Boş Bırakılamaz!");
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        Exception err = DBUtils.UpdateCategoryByID(CategoryID, textBox1.Text);
                        if (err != null)
                        {
                            MessageBox.Show(err.Message);
                            return;
                        }

                        MessageBox.Show("Kategori Yeniden Adlandırıldı!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kategori Adı Boş Bırakılamaz!");
                        return;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void AddEditCat_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void AddEditCat_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void AddEditCat_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
