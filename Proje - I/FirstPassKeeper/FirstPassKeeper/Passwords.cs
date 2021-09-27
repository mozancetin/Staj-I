using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public partial class Passwords : Form
    {
        private string user;
        private string category;
        private bool mouseDown;
        private Point lastLocation;

        public Passwords(string user, string category = null)
        {
            InitializeComponent();
            this.user = user;
            this.category = category;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Passwords_Load(object sender, EventArgs e)
        {
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                if (this.category == null)
                {
                    string getPasswordsSql = string.Format("SELECT * FROM {0}", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getPasswordsSql, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "name";
                    connection.Close();
                }
                else
                {
                    string getPasswordsSql = string.Format("SELECT * FROM {0} WHERE category = @category", this.user);
                    SqlDataAdapter da = new SqlDataAdapter(getPasswordsSql, connection);
                    da.SelectCommand.Parameters.AddWithValue("@category", this.category);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "name";
                    listBox1.ValueMember = "name";
                    connection.Close();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (this.category == null)
                {
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE name LIKE @text", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@text", "%" + nameBox.Text + "%");
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }

                    else
                    {
                        string getPassSql = string.Format("SELECT * FROM {0}", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }
                }
                else
                {
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE name LIKE @text AND category = @category", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@text", "%" + nameBox.Text + "%");
                        da.SelectCommand.Parameters.AddWithValue("@category", this.category);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }

                    else
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE category = @category", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@category", this.category);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
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
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
                string getPass = string.Format("SELECT * FROM {0} WHERE name = @item", this.user);
                SqlCommand getPassCmd = new SqlCommand(getPass, connection);
                getPassCmd.Parameters.AddWithValue("@item", myitem);
                SqlDataReader reader = getPassCmd.ExecuteReader();
                reader.Read();
                string pass = Utils.Decrypt(reader.GetString(1));
                reader.Close();
                ShowPass passWindow = new ShowPass(myitem, pass);
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
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
                string deleteSql = string.Format("DELETE FROM {0} WHERE name = @progName", this.user);
                SqlCommand deleteCmd = new SqlCommand(deleteSql, connection);
                deleteCmd.Parameters.AddWithValue("@progName", myitem);
                deleteCmd.ExecuteNonQuery();

                if (this.category == null)
                {
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE name LIKE @text", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@text", "%" + nameBox.Text + "%");
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }

                    else
                    {
                        string getPassSql = string.Format("SELECT * FROM {0}", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }
                }
                else
                {
                    if (nameBox.Text.Replace(" ", "") != "")
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE name LIKE @text AND category = @category", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@text", "%" + nameBox.Text + "%");
                        da.SelectCommand.Parameters.AddWithValue("@category", this.category);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
                    }

                    else
                    {
                        string getPassSql = string.Format("SELECT * FROM {0} WHERE category = @category", this.user);
                        SqlDataAdapter da = new SqlDataAdapter(getPassSql, connection);
                        da.SelectCommand.Parameters.AddWithValue("@category", this.category);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        listBox1.DataSource = dt;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "name";
                        connection.Close();
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
            string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
            MainMenu editMenu = new MainMenu(this.user, myitem);
            this.Hide();
            editMenu.ShowDialog();
            this.Close();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            MainMenu editMenu = new MainMenu(this.user);
            this.Hide();
            editMenu.ShowDialog();
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string source = "Data Source=DESKTOP-VR7FKVJ;Initial Catalog=users;Integrated Security=True";
            try
            {
                SqlConnection connection = new SqlConnection(source);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myitem = ((DataRowView)listBox1.SelectedItem).Row.ItemArray[0].ToString();
                string getPass = string.Format("SELECT * FROM {0} WHERE name = @item", this.user);
                SqlCommand getPassCmd = new SqlCommand(getPass, connection);
                getPassCmd.Parameters.AddWithValue("@item", myitem);
                SqlDataReader reader = getPassCmd.ExecuteReader();
                reader.Read();
                string pass = Utils.Decrypt(reader.GetString(1));
                reader.Close();
                ShowPass passWindow = new ShowPass(myitem, pass);
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
            MainMenu menu = new MainMenu(this.user);
            this.Hide();
            menu.ShowDialog();
            this.Close();
        }
    }
}
