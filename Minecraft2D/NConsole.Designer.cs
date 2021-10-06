namespace Minecraft2D
{
    partial class NConsole
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
            this.winFormsConsole1 = new Minecraft2D.WinFormsConsole();
            this.SuspendLayout();
            // 
            // winFormsConsole1
            // 
            this.winFormsConsole1.BackColor = System.Drawing.Color.White;
            this.winFormsConsole1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winFormsConsole1.Font = new System.Drawing.Font("Video Terminal Screen", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.winFormsConsole1.Location = new System.Drawing.Point(0, 0);
            this.winFormsConsole1.Name = "winFormsConsole1";
            this.winFormsConsole1.PromptString = ">";
            this.winFormsConsole1.Size = new System.Drawing.Size(659, 302);
            this.winFormsConsole1.TabIndex = 0;
            // 
            // NConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 302);
            this.Controls.Add(this.winFormsConsole1);
            this.Font = new System.Drawing.Font("Consolas", 11.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NConsole";
            this.ShowIcon = false;
            this.Text = " ";
            this.Load += new System.EventHandler(this.NConsole_Load);
            this.Shown += new System.EventHandler(this.NConsole_Shown);
            this.ResumeLayout(false);

        }

        public WinFormsConsole winFormsConsole1;

        #endregion

        //internal DarkCoderConsoleBox.ConsoleBox consoleBox1;
    }
}