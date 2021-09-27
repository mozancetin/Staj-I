
namespace FirstPassKeeper
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.closeButton = new System.Windows.Forms.Button();
            this.allPasswordsButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.categoriesButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.name = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.category = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(23)))), ((int)(((byte)(23)))));
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.closeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.closeButton.Location = new System.Drawing.Point(258, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(29, 25);
            this.closeButton.TabIndex = 8;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // allPasswordsButton
            // 
            this.allPasswordsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.allPasswordsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.allPasswordsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.allPasswordsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.allPasswordsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.allPasswordsButton.Location = new System.Drawing.Point(12, 322);
            this.allPasswordsButton.Name = "allPasswordsButton";
            this.allPasswordsButton.Size = new System.Drawing.Size(275, 28);
            this.allPasswordsButton.TabIndex = 7;
            this.allPasswordsButton.Text = "Şifrelerim";
            this.allPasswordsButton.UseVisualStyleBackColor = false;
            this.allPasswordsButton.Click += new System.EventHandler(this.allPasswordsButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.saveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.saveButton.Location = new System.Drawing.Point(12, 288);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(275, 28);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Kaydet";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // categoriesButton
            // 
            this.categoriesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.categoriesButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.categoriesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categoriesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.categoriesButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.categoriesButton.Location = new System.Drawing.Point(13, 356);
            this.categoriesButton.MaximumSize = new System.Drawing.Size(275, 28);
            this.categoriesButton.MinimumSize = new System.Drawing.Size(275, 28);
            this.categoriesButton.Name = "categoriesButton";
            this.categoriesButton.Size = new System.Drawing.Size(275, 28);
            this.categoriesButton.TabIndex = 6;
            this.categoriesButton.Text = "Kategorilerim";
            this.categoriesButton.UseVisualStyleBackColor = false;
            this.categoriesButton.Click += new System.EventHandler(this.categoriesButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.ForeColor = System.Drawing.Color.Black;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 243);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(271, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.name.Location = new System.Drawing.Point(12, 45);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(116, 19);
            this.name.TabIndex = 8;
            this.name.Text = "Programın Adı";
            // 
            // nameBox
            // 
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nameBox.Location = new System.Drawing.Point(12, 76);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(271, 22);
            this.nameBox.TabIndex = 0;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.password.Location = new System.Drawing.Point(12, 118);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(44, 19);
            this.password.TabIndex = 8;
            this.password.Text = "Şifre";
            // 
            // category
            // 
            this.category.AutoSize = true;
            this.category.Font = new System.Drawing.Font("Calibri Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.category.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.category.Location = new System.Drawing.Point(12, 211);
            this.category.Name = "category";
            this.category.Size = new System.Drawing.Size(84, 19);
            this.category.TabIndex = 8;
            this.category.Text = "Kategorisi";
            // 
            // passwordBox
            // 
            this.passwordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.passwordBox.Location = new System.Drawing.Point(12, 150);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '●';
            this.passwordBox.Size = new System.Drawing.Size(271, 22);
            this.passwordBox.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.checkBox1.Location = new System.Drawing.Point(12, 178);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(88, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Şifreyi Göster";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(62, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Rastgele";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.exportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.exportButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(197)))), ((int)(((byte)(255)))));
            this.exportButton.Location = new System.Drawing.Point(12, 390);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(275, 28);
            this.exportButton.TabIndex = 7;
            this.exportButton.Text = "Dışarı Aktar";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(300, 430);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.category);
            this.Controls.Add(this.password);
            this.Controls.Add(this.name);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.categoriesButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.allPasswordsButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 430);
            this.MinimumSize = new System.Drawing.Size(300, 430);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainMenu_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainMenu_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainMenu_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button allPasswordsButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button categoriesButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Label category;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button exportButton;
    }
}