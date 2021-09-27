using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public partial class AddEditCat : Form
    {
        private string user;
        private string category;
        private bool mouseDown;
        private Point lastLocation;
        public AddEditCat(string user, string category = null)
        {
            InitializeComponent();
            this.user = user;
            this.category = category;
        }

        private void AddEditCat_Load(object sender, EventArgs e)
        {
            if (this.category == null)
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
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (this.category == null)
                {
                    string isExists = string.Format("SELECT * FROM {0}C WHERE categories = @category", this.user);
                    SqlCommand isExistsCmd = new SqlCommand(isExists, connection);
                    isExistsCmd.Parameters.AddWithValue("@category", textBox1.Text);
                    SqlDataReader reader = isExistsCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Böyle bir kategori zaten var!");
                        reader.Close();
                        return;
                    }
                    reader.Close();
                    if (textBox1.Text.Replace(" ", "") != "")
                    {
                        string operation = string.Format("INSERT INTO {0}C VALUES(@category)", this.user);
                        SqlCommand operationCmd = new SqlCommand(operation, connection);
                        operationCmd.Parameters.AddWithValue("@category", textBox1.Text);
                        operationCmd.ExecuteNonQuery();

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
                    if (textBox1.Text.Replace(" ", "") != "")
                    {
                        string updateCategory = string.Format("UPDATE {0}C SET categories = @category WHERE categories = @oldCat", this.user);
                        SqlCommand updateCategoryCmd = new SqlCommand(updateCategory, connection);
                        updateCategoryCmd.Parameters.AddWithValue("@category", textBox1.Text);
                        updateCategoryCmd.Parameters.AddWithValue("@oldCat", this.category);
                        updateCategoryCmd.ExecuteNonQuery();

                        string updatePasswords = string.Format("UPDATE {0} SET category = @category WHERE category = @oldCat", this.user);
                        SqlCommand updatePasswordsCmd = new SqlCommand(updatePasswords, connection);
                        updatePasswordsCmd.Parameters.AddWithValue("@category", textBox1.Text);
                        updatePasswordsCmd.Parameters.AddWithValue("@oldCat", this.category);
                        updatePasswordsCmd.ExecuteNonQuery();

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
