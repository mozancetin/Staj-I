using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstPassKeeper
{
    public partial class ShowPass : Form
    {
        private string progName;
        private string pass;
        private bool mouseDown;
        private Point lastLocation;
        public ShowPass(string progName, string progPass)
        {
            InitializeComponent();
            this.progName = progName;
            this.pass = progPass;
        }

        private void ShowPass_Load(object sender, EventArgs e)
        {
            nameLabel.Text = this.progName;
            passLabel.Text = this.pass;
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.pass);
            MessageBox.Show("Şifre başarıyla kopyalandı!");
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowPass_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void ShowPass_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void ShowPass_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
