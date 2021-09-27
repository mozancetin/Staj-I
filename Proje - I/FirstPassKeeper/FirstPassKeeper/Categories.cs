using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public partial class Categories : Form
    {
        private string user;
        private bool mouseDown;
        private Point lastLocation;
        public Categories(string user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            try
            {
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string getCategories = string.Format("SELECT * FROM {0}C", this.user);
                SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                listBox1.DataSource = dt;
                listBox1.DisplayMember = "categories";
                listBox1.ValueMember = "categories";
                connection.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void categorySearchBox_TextChanged(object sender, EventArgs e)
        {
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (categorySearchBox.Text.Replace(" ", "") != "")
                {
                    string getCategories = string.Format("SELECT * FROM {0}C WHERE categories LIKE @text", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    da.SelectCommand.Parameters.AddWithValue("@text", "%" + categorySearchBox.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }
                else
                {
                    string getCategories = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
            Passwords passWindow = new Passwords(this.user, myitem);
            this.Hide();
            passWindow.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu menu = new MainMenu(this.user);
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
                string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
                if (myitem == "Uncategorized")
                {
                    MessageBox.Show("Bu kategori default olduğu için silinemez.");
                    return;
                }
                string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string updatePass = string.Format("UPDATE {0} SET category = 'Uncategorized' WHERE category = @category", this.user);
                SqlCommand updatePassCmd = new SqlCommand(updatePass, connection);
                updatePassCmd.Parameters.AddWithValue("@category", myitem);
                updatePassCmd.ExecuteNonQuery();

                string deleteCategory = string.Format("DELETE FROM {0}C WHERE categories = @category", this.user);
                SqlCommand deleteCategoryCmd = new SqlCommand(deleteCategory, connection);
                deleteCategoryCmd.Parameters.AddWithValue("@category", myitem);
                deleteCategoryCmd.ExecuteNonQuery();

                if (categorySearchBox.Text.Replace(" ", "") != "")
                {
                    string getCategories = string.Format("SELECT * FROM {0}C WHERE categories LIKE @text", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    da.SelectCommand.Parameters.AddWithValue("@text", "%" + categorySearchBox.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }
                else
                {
                    string getCategories = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void editButton_Click(object sender, EventArgs e)
        {
            string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
            if (myitem == "Uncategorized")
            {
                MessageBox.Show("Bu kategori default olduğu için düzenlenemez.");
                return;
            }

            AddEditCat Window = new AddEditCat(this.user, myitem);
            this.Hide();
            Window.ShowDialog();
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (categorySearchBox.Text.Replace(" ", "") != "")
                {
                    string getCategories = string.Format("SELECT * FROM {0}C WHERE categories LIKE @text", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    da.SelectCommand.Parameters.AddWithValue("@text", "%" + categorySearchBox.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }
                else
                {
                    string getCategories = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
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
            AddEditCat Window = new AddEditCat(this.user);
            this.Hide();
            Window.ShowDialog();
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (categorySearchBox.Text.Replace(" ", "") != "")
                {
                    string getCategories = string.Format("SELECT * FROM {0}C WHERE categories LIKE @text", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    da.SelectCommand.Parameters.AddWithValue("@text", "%" + categorySearchBox.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
                }
                else
                {
                    string getCategories = string.Format("SELECT * FROM {0}C", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getCategories, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "categories";
                    listBox1.ValueMember = "categories";
                    connection.Close();
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
            string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
            Passwords passWindow = new Passwords(this.user, myitem);
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
