
namespace PassKeeperEFWeb
{
    partial class ShowPass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowPass));
            this.name = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.copyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.passLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.name.Location = new System.Drawing.Point(146, 13);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(116, 19);
            this.name.TabIndex = 10;
            this.name.Text = "Programın Adı";
            // 
            // nameLabel
            // 
            this.nameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.nameLabel.Font = new System.Drawing.Font("Calibri Light", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.nameLabel.Location = new System.Drawing.Point(0, 41);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(415, 35);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.Text = "Programın Adı";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // copyButton
            // 
            this.copyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.copyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.copyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.copyButton.Location = new System.Drawing.Point(41, 166);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(327, 28);
            this.copyButton.TabIndex = 0;
            this.copyButton.Text = "Şifreyi Kopyala";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(175, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "Şifresi";
            // 
            // passLabel
            // 
            this.passLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.passLabel.Font = new System.Drawing.Font("Calibri Light", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.passLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.passLabel.Location = new System.Drawing.Point(-5, 116);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(420, 35);
            this.passLabel.TabIndex = 10;
            this.passLabel.Text = "Programın Şifresi";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeButton
            // 
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.closeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.closeButton.Location = new System.Drawing.Point(367, 9);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(29, 25);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "<";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // ShowPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(408, 210);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(408, 210);
            this.MinimumSize = new System.Drawing.Size(408, 210);
            this.Name = "ShowPass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowPass";
            this.Load += new System.EventHandler(this.ShowPass_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShowPass_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowPass_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShowPass_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.Button closeButton;
    }
}