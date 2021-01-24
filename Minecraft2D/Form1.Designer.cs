using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Minecraft2D
{
    [DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Форма переопределяет dispose для очистки списка компонентов.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Является обязательной для конструктора форм Windows Forms
        private System.ComponentModel.IContainer components;

        // Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
        // Для ее изменения используйте конструктор форм Windows Form.  
        // Не изменяйте ее в редакторе исходного кода.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this._Ticker = new System.Windows.Forms.Timer(this.components);
            this.InventoryButton = new System.Windows.Forms.Button();
            this._ListBox1 = new System.Windows.Forms.ListBox();
            this.ChatButton = new System.Windows.Forms.Button();
            this._Timer3 = new System.Windows.Forms.Timer(this.components);
            this._ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._localPlayer = new Minecraft2D.TransparentPicBox();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.пасхалкажмиСюдаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._Warning = new System.Windows.Forms.Label();
            this._ButtonLeft = new System.Windows.Forms.Button();
            this._ButtonJump = new System.Windows.Forms.Button();
            this._ButtonRight = new System.Windows.Forms.Button();
            this.MenuButton = new System.Windows.Forms.Button();
            this._ButtonAttack = new System.Windows.Forms.Button();
            this._ListBox2 = new System.Windows.Forms.ListBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.makeItDark = new System.Windows.Forms.PictureBox();
            this.invPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.invClose1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chatPanel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.debuginfo = new Minecraft2D.OpaqueLabel();
            this.R1 = new Minecraft2D.TransparentPicBox();
            ((System.ComponentModel.ISupportInitialize)(this._localPlayer)).BeginInit();
            this.ContextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.makeItDark)).BeginInit();
            this.invPanel.SuspendLayout();
            this.chatPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.R1)).BeginInit();
            this.SuspendLayout();
            // 
            // _Ticker
            // 
            this._Ticker.Enabled = true;
            this._Ticker.Interval = 15000;
            this._Ticker.Tick += new System.EventHandler(this.Ticker_Tick);
            // 
            // InventoryButton
            // 
            this.InventoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InventoryButton.BackColor = System.Drawing.SystemColors.Control;
            this.InventoryButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("InventoryButton.BackgroundImage")));
            this.InventoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InventoryButton.ForeColor = System.Drawing.Color.White;
            this.InventoryButton.Location = new System.Drawing.Point(1088, 0);
            this.InventoryButton.Name = "InventoryButton";
            this.InventoryButton.Size = new System.Drawing.Size(75, 23);
            this.InventoryButton.TabIndex = 2;
            this.InventoryButton.Text = "game.button.inventory";
            this.InventoryButton.UseVisualStyleBackColor = false;
            this.InventoryButton.Click += new System.EventHandler(this.Button1_Click);
            this.InventoryButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.InventoryButton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.InventoryButton.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.InventoryButton.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // _ListBox1
            // 
            this._ListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this._ListBox1.ForeColor = System.Drawing.Color.White;
            this._ListBox1.FormattingEnabled = true;
            this._ListBox1.Location = new System.Drawing.Point(364, 36);
            this._ListBox1.Name = "_ListBox1";
            this._ListBox1.Size = new System.Drawing.Size(467, 368);
            this._ListBox1.TabIndex = 3;
            this._ListBox1.UseTabStops = false;
            this._ListBox1.SelectedIndexChanged += new System.EventHandler(this._ListBox1_SelectedIndexChanged);
            this._ListBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ListBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDoubleClick);
            this._ListBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1_MouseDown);
            // 
            // ChatButton
            // 
            this.ChatButton.BackColor = System.Drawing.SystemColors.Control;
            this.ChatButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ChatButton.BackgroundImage")));
            this.ChatButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChatButton.ForeColor = System.Drawing.Color.White;
            this.ChatButton.Location = new System.Drawing.Point(3, 0);
            this.ChatButton.Name = "ChatButton";
            this.ChatButton.Size = new System.Drawing.Size(75, 48);
            this.ChatButton.TabIndex = 4;
            this.ChatButton.Text = "game.button.chat";
            this.ChatButton.UseVisualStyleBackColor = false;
            this.ChatButton.Click += new System.EventHandler(this.Button2_Click);
            this.ChatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ChatButton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ChatButton.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.ChatButton.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // _Timer3
            // 
            this._Timer3.Enabled = true;
            this._Timer3.Interval = 15000;
            this._Timer3.Tick += new System.EventHandler(this.Timer3_Tick);
            // 
            // _ProgressBar1
            // 
            this._ProgressBar1.BackColor = System.Drawing.SystemColors.Control;
            this._ProgressBar1.ForeColor = System.Drawing.Color.Red;
            this._ProgressBar1.Location = new System.Drawing.Point(84, 0);
            this._ProgressBar1.Name = "_ProgressBar1";
            this._ProgressBar1.Size = new System.Drawing.Size(149, 23);
            this._ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._ProgressBar1.TabIndex = 5;
            this._ProgressBar1.Value = 100;
            this._ProgressBar1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ProgressBar1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // ToolTip1
            // 
            this.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToolTip1.ToolTipTitle = "Информация";
            // 
            // _localPlayer
            // 
            this._localPlayer.BackColor = System.Drawing.Color.Transparent;
            this._localPlayer.ContextMenuStrip = this.ContextMenuStrip1;
            this._localPlayer.Image = ((System.Drawing.Image)(resources.GetObject("_localPlayer.Image")));
            this._localPlayer.Location = new System.Drawing.Point(0, 0);
            this._localPlayer.Name = "_localPlayer";
            this._localPlayer.Size = new System.Drawing.Size(37, 72);
            this._localPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._localPlayer.TabIndex = 14;
            this._localPlayer.TabStop = false;
            this.ToolTip1.SetToolTip(this._localPlayer, "Локальный игрок");
            this._localPlayer.Move += new System.EventHandler(this._localPlayer_Move);
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пасхалкажмиСюдаToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(191, 26);
            // 
            // пасхалкажмиСюдаToolStripMenuItem
            // 
            this.пасхалкажмиСюдаToolStripMenuItem.Name = "пасхалкажмиСюдаToolStripMenuItem";
            this.пасхалкажмиСюдаToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.пасхалкажмиСюдаToolStripMenuItem.Text = "пасхалка (жми сюда)";
            this.пасхалкажмиСюдаToolStripMenuItem.Click += new System.EventHandler(this.пасхалкажмиСюдаToolStripMenuItem_Click);
            // 
            // _Warning
            // 
            this._Warning.AutoSize = true;
            this._Warning.BackColor = System.Drawing.Color.Transparent;
            this._Warning.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._Warning.ForeColor = System.Drawing.Color.Red;
            this._Warning.Location = new System.Drawing.Point(239, 0);
            this._Warning.Name = "_Warning";
            this._Warning.Size = new System.Drawing.Size(0, 18);
            this._Warning.TabIndex = 8;
            this._Warning.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._Warning.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            // 
            // _ButtonLeft
            // 
            this._ButtonLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ButtonLeft.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ButtonLeft.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonLeft.Location = new System.Drawing.Point(8, 503);
            this._ButtonLeft.Name = "_ButtonLeft";
            this._ButtonLeft.Size = new System.Drawing.Size(70, 61);
            this._ButtonLeft.TabIndex = 9;
            this._ButtonLeft.Text = "ß";
            this._ButtonLeft.UseCompatibleTextRendering = true;
            this._ButtonLeft.UseVisualStyleBackColor = false;
            this._ButtonLeft.Visible = false;
            this._ButtonLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonLeft.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button3_MouseDown);
            this._ButtonLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button3_MouseUp);
            // 
            // _ButtonJump
            // 
            this._ButtonJump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ButtonJump.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonJump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ButtonJump.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonJump.Location = new System.Drawing.Point(1084, 503);
            this._ButtonJump.Name = "_ButtonJump";
            this._ButtonJump.Size = new System.Drawing.Size(70, 61);
            this._ButtonJump.TabIndex = 10;
            this._ButtonJump.Text = "ñ";
            this._ButtonJump.UseCompatibleTextRendering = true;
            this._ButtonJump.UseVisualStyleBackColor = false;
            this._ButtonJump.Visible = false;
            this._ButtonJump.Click += new System.EventHandler(this.Button5_Click);
            this._ButtonJump.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonJump.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonJump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonJump_MouseDown);
            // 
            // _ButtonRight
            // 
            this._ButtonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ButtonRight.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ButtonRight.Font = new System.Drawing.Font("Wingdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonRight.Location = new System.Drawing.Point(84, 503);
            this._ButtonRight.Name = "_ButtonRight";
            this._ButtonRight.Size = new System.Drawing.Size(70, 61);
            this._ButtonRight.TabIndex = 11;
            this._ButtonRight.Text = "à";
            this._ButtonRight.UseCompatibleTextRendering = true;
            this._ButtonRight.UseVisualStyleBackColor = false;
            this._ButtonRight.Visible = false;
            this._ButtonRight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonRight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this._ButtonRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button6_MouseDown);
            this._ButtonRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button6_MouseUp);
            // 
            // MenuButton
            // 
            this.MenuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MenuButton.BackColor = System.Drawing.SystemColors.Control;
            this.MenuButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MenuButton.BackgroundImage")));
            this.MenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MenuButton.ForeColor = System.Drawing.Color.White;
            this.MenuButton.Location = new System.Drawing.Point(1088, 29);
            this.MenuButton.Name = "MenuButton";
            this.MenuButton.Size = new System.Drawing.Size(75, 23);
            this.MenuButton.TabIndex = 12;
            this.MenuButton.Text = "game.button.pause";
            this.MenuButton.UseVisualStyleBackColor = false;
            this.MenuButton.Click += new System.EventHandler(this.Button3_Click_1);
            this.MenuButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MenuButton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MenuButton.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.MenuButton.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // _ButtonAttack
            // 
            this._ButtonAttack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ButtonAttack.BackColor = System.Drawing.SystemColors.Control;
            this._ButtonAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._ButtonAttack.Font = new System.Drawing.Font("Webdings", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this._ButtonAttack.ForeColor = System.Drawing.Color.Red;
            this._ButtonAttack.Location = new System.Drawing.Point(1008, 503);
            this._ButtonAttack.Name = "_ButtonAttack";
            this._ButtonAttack.Size = new System.Drawing.Size(70, 61);
            this._ButtonAttack.TabIndex = 13;
            this._ButtonAttack.Text = "r";
            this._ButtonAttack.UseVisualStyleBackColor = false;
            this._ButtonAttack.Visible = false;
            this._ButtonAttack.Click += new System.EventHandler(this.ButtonAttack_Click);
            this._ButtonAttack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this._ButtonAttack.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // _ListBox2
            // 
            this._ListBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this._ListBox2.ForeColor = System.Drawing.Color.White;
            this._ListBox2.FormattingEnabled = true;
            this._ListBox2.Location = new System.Drawing.Point(0, 36);
            this._ListBox2.Name = "_ListBox2";
            this._ListBox2.Size = new System.Drawing.Size(365, 368);
            this._ListBox2.TabIndex = 17;
            this._ListBox2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox2_MouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bedrock.png");
            this.imageList1.Images.SetKeyName(1, "BLOCK.png");
            this.imageList1.Images.SetKeyName(2, "bucket.png");
            this.imageList1.Images.SetKeyName(3, "c-edition.png");
            this.imageList1.Images.SetKeyName(4, "coal.png");
            this.imageList1.Images.SetKeyName(5, "coal_ore.png");
            this.imageList1.Images.SetKeyName(6, "cobblestone.png");
            this.imageList1.Images.SetKeyName(7, "cobblestone4.png");
            this.imageList1.Images.SetKeyName(8, "diamond.png");
            this.imageList1.Images.SetKeyName(9, "DIAMOND_AXE.png");
            this.imageList1.Images.SetKeyName(10, "DIAMOND_AXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(11, "diamond_block.png");
            this.imageList1.Images.SetKeyName(12, "diamond_ore.png");
            this.imageList1.Images.SetKeyName(13, "DIAMOND_PICKAXE.png");
            this.imageList1.Images.SetKeyName(14, "DIAMOND_PICKAXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(15, "DIAMOND_SHOVEL.png");
            this.imageList1.Images.SetKeyName(16, "DIAMOND_SHOVEL_FLIPPED.png");
            this.imageList1.Images.SetKeyName(17, "DIAMOND_SWORD.png");
            this.imageList1.Images.SetKeyName(18, "DIAMOND_SWORD_FLIPPED.png");
            this.imageList1.Images.SetKeyName(19, "dirt.png");
            this.imageList1.Images.SetKeyName(20, "dirt1.png");
            this.imageList1.Images.SetKeyName(21, "furnace_front.png");
            this.imageList1.Images.SetKeyName(22, "furnace_front_off.png");
            this.imageList1.Images.SetKeyName(23, "furnace_front_on.png");
            this.imageList1.Images.SetKeyName(24, "glass.png");
            this.imageList1.Images.SetKeyName(25, "GLASSBLOCK.png");
            this.imageList1.Images.SetKeyName(26, "GOLD.png");
            this.imageList1.Images.SetKeyName(27, "gold_block.png");
            this.imageList1.Images.SetKeyName(28, "gold_ore.png");
            this.imageList1.Images.SetKeyName(29, "grass_block_side.png");
            this.imageList1.Images.SetKeyName(30, "grass_side.png");
            this.imageList1.Images.SetKeyName(31, "Grid_Sapling.png");
            this.imageList1.Images.SetKeyName(32, "IRON.png");
            this.imageList1.Images.SetKeyName(33, "IRON_AXE.png");
            this.imageList1.Images.SetKeyName(34, "IRON_AXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(35, "iron_block.png");
            this.imageList1.Images.SetKeyName(36, "iron_ore.png");
            this.imageList1.Images.SetKeyName(37, "IRON_PICKAXE.png");
            this.imageList1.Images.SetKeyName(38, "IRON_PICKAXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(39, "IRON_SHOVEL.png");
            this.imageList1.Images.SetKeyName(40, "IRON_SHOVEL_FLIPPED.png");
            this.imageList1.Images.SetKeyName(41, "IRON_SWORD.png");
            this.imageList1.Images.SetKeyName(42, "IRON_SWORD_FLIPPED.png");
            this.imageList1.Images.SetKeyName(43, "lava_bucket.png");
            this.imageList1.Images.SetKeyName(44, "lava_still.png");
            this.imageList1.Images.SetKeyName(45, "leaves.jpg");
            this.imageList1.Images.SetKeyName(46, "log_oak.png");
            this.imageList1.Images.SetKeyName(47, "NCLogo.png");
            this.imageList1.Images.SetKeyName(48, "netcraft.png");
            this.imageList1.Images.SetKeyName(49, "oak_log.png");
            this.imageList1.Images.SetKeyName(50, "oak_sapling.png");
            this.imageList1.Images.SetKeyName(51, "OBSIDIAN.png");
            this.imageList1.Images.SetKeyName(52, "obsidian1.png");
            this.imageList1.Images.SetKeyName(53, "planks.png");
            this.imageList1.Images.SetKeyName(54, "planks_oak.png");
            this.imageList1.Images.SetKeyName(55, "platformPack_tile001.png");
            this.imageList1.Images.SetKeyName(56, "platformPack_tile004.png");
            this.imageList1.Images.SetKeyName(57, "platformPack_tile016.png");
            this.imageList1.Images.SetKeyName(58, "platformPack_tile034.png");
            this.imageList1.Images.SetKeyName(59, "Player1Texture.png");
            this.imageList1.Images.SetKeyName(60, "Player2Texture.png");
            this.imageList1.Images.SetKeyName(61, "sand.png");
            this.imageList1.Images.SetKeyName(62, "SANDBLOCK.png");
            this.imageList1.Images.SetKeyName(63, "sprite.png");
            this.imageList1.Images.SetKeyName(64, "STICK.png");
            this.imageList1.Images.SetKeyName(65, "STONE.png");
            this.imageList1.Images.SetKeyName(66, "STONE_AXE.png");
            this.imageList1.Images.SetKeyName(67, "STONE_AXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(68, "STONE_PICKAXE.png");
            this.imageList1.Images.SetKeyName(69, "STONE_PICKAXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(70, "STONE_SHOVEL.png");
            this.imageList1.Images.SetKeyName(71, "STONE_SHOVEL_FLIPPED.png");
            this.imageList1.Images.SetKeyName(72, "STONE_SWORD.png");
            this.imageList1.Images.SetKeyName(73, "STONE_SWORD_FLIPPED.png");
            this.imageList1.Images.SetKeyName(74, "stone1.png");
            this.imageList1.Images.SetKeyName(75, "tnt_side.png");
            this.imageList1.Images.SetKeyName(76, "vbnet-edition.png");
            this.imageList1.Images.SetKeyName(77, "water_bucket.png");
            this.imageList1.Images.SetKeyName(78, "water_still.png");
            this.imageList1.Images.SetKeyName(79, "WOOD.png");
            this.imageList1.Images.SetKeyName(80, "WOODEN_AXE.png");
            this.imageList1.Images.SetKeyName(81, "WOODEN_AXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(82, "WOODEN_PICKAXE.png");
            this.imageList1.Images.SetKeyName(83, "WOODEN_PICKAXE_FLIPPED.png");
            this.imageList1.Images.SetKeyName(84, "WOODEN_SHOVEL.png");
            this.imageList1.Images.SetKeyName(85, "WOODEN_SHOVEL_FLIPPED.png");
            this.imageList1.Images.SetKeyName(86, "WOODEN_SWORD.png");
            this.imageList1.Images.SetKeyName(87, "WOODEN_SWORD_FLIPPED.png");
            this.imageList1.Images.SetKeyName(88, "cancel.png");
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.ForeColor = System.Drawing.Color.Gold;
            this.progressBar1.Location = new System.Drawing.Point(84, 25);
            this.progressBar1.Maximum = 20;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(149, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 18;
            this.progressBar1.Value = 20;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // makeItDark
            // 
            this.makeItDark.Location = new System.Drawing.Point(460, -16);
            this.makeItDark.Name = "makeItDark";
            this.makeItDark.Size = new System.Drawing.Size(100, 13);
            this.makeItDark.TabIndex = 20;
            this.makeItDark.TabStop = false;
            this.makeItDark.Visible = false;
            // 
            // invPanel
            // 
            this.invPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.invPanel.Controls.Add(this.label1);
            this.invPanel.Controls.Add(this._ListBox2);
            this.invPanel.Controls.Add(this.invClose1);
            this.invPanel.Controls.Add(this._ListBox1);
            this.invPanel.ForeColor = System.Drawing.Color.White;
            this.invPanel.Location = new System.Drawing.Point(172, 54);
            this.invPanel.Name = "invPanel";
            this.invPanel.Size = new System.Drawing.Size(831, 408);
            this.invPanel.TabIndex = 21;
            this.invPanel.Visible = false;
            this.invPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.invPanel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Inventory";
            // 
            // invClose1
            // 
            this.invClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.invClose1.BackColor = System.Drawing.SystemColors.Control;
            this.invClose1.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this.invClose1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.invClose1.ImageKey = "cancel.png";
            this.invClose1.ImageList = this.imageList1;
            this.invClose1.Location = new System.Drawing.Point(770, 3);
            this.invClose1.Name = "invClose1";
            this.invClose1.Size = new System.Drawing.Size(58, 29);
            this.invClose1.TabIndex = 22;
            this.invClose1.UseVisualStyleBackColor = false;
            this.invClose1.Click += new System.EventHandler(this.button1_Click_1);
            this.invClose1.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.invClose1.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.textBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(0, 348);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(556, 25);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImage = global::Minecraft2D.My.Resources.Resources.buttonbg;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.ImageKey = "cancel.png";
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(492, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 26);
            this.button1.TabIndex = 23;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            this.button1.MouseEnter += new System.EventHandler(this.MenuButton_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.MenuButton_MouseLeave);
            // 
            // chatPanel1
            // 
            this.chatPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.chatPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.chatPanel1.Controls.Add(this.label2);
            this.chatPanel1.Controls.Add(this.button1);
            this.chatPanel1.Controls.Add(this.textBox1);
            this.chatPanel1.Controls.Add(this.richTextBox1);
            this.chatPanel1.Location = new System.Drawing.Point(5, 124);
            this.chatPanel1.Name = "chatPanel1";
            this.chatPanel1.Size = new System.Drawing.Size(556, 373);
            this.chatPanel1.TabIndex = 22;
            this.chatPanel1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Chat";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(0, 35);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(556, 314);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // debuginfo
            // 
            this.debuginfo.AutoSize = true;
            this.debuginfo.BackColor = System.Drawing.Color.Transparent;
            this.debuginfo.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.debuginfo.ForeColor = System.Drawing.Color.White;
            this.debuginfo.Location = new System.Drawing.Point(5, 51);
            this.debuginfo.Name = "debuginfo";
            this.debuginfo.Size = new System.Drawing.Size(133, 68);
            this.debuginfo.TabIndex = 19;
            this.debuginfo.Text = "Netcraft {0}\r\nServer: {1}\r\nPlayer position: x, y\r\nOnline players: a";
            this.debuginfo.Visible = false;
            this.debuginfo.Click += new System.EventHandler(this.debuginfo_Click);
            // 
            // R1
            // 
            this.R1.BackColor = System.Drawing.Color.Transparent;
            this.R1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.R1.Location = new System.Drawing.Point(54, 146);
            this.R1.Name = "R1";
            this.R1.Size = new System.Drawing.Size(32, 32);
            this.R1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.R1.TabIndex = 15;
            this.R1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(1166, 576);
            this.Controls.Add(this.chatPanel1);
            this.Controls.Add(this.invPanel);
            this.Controls.Add(this.makeItDark);
            this.Controls.Add(this.debuginfo);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.R1);
            this.Controls.Add(this._localPlayer);
            this.Controls.Add(this._ButtonAttack);
            this.Controls.Add(this.MenuButton);
            this.Controls.Add(this._ButtonRight);
            this.Controls.Add(this._ButtonJump);
            this.Controls.Add(this._ButtonLeft);
            this.Controls.Add(this._Warning);
            this.Controls.Add(this._ProgressBar1);
            this.Controls.Add(this.ChatButton);
            this.Controls.Add(this.InventoryButton);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2140, 2060);
            this.Name = "Form1";
            this.Text = " ";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Form1_Scroll);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Move += new System.EventHandler(this.Form1_Move);
            ((System.ComponentModel.ISupportInitialize)(this._localPlayer)).EndInit();
            this.ContextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.makeItDark)).EndInit();
            this.invPanel.ResumeLayout(false);
            this.invPanel.PerformLayout();
            this.chatPanel1.ResumeLayout(false);
            this.chatPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.R1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Timer _Ticker;

        internal Timer Ticker
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Ticker;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Ticker != null)
                {
                    _Ticker.Tick -= Ticker_Tick;
                }

                _Ticker = value;
                if (_Ticker != null)
                {
                    _Ticker.Tick += Ticker_Tick;
                }
            }
        }

        private Button InventoryButton;

        internal Button Button1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return InventoryButton;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (InventoryButton != null)
                {
                    InventoryButton.KeyDown -= Form1_KeyDown;
                    InventoryButton.KeyUp -= Form1_KeyUp;
                    InventoryButton.Click -= Button1_Click;
                }

                InventoryButton = value;
                if (InventoryButton != null)
                {
                    InventoryButton.KeyDown += Form1_KeyDown;
                    InventoryButton.KeyUp += Form1_KeyUp;
                    InventoryButton.Click += Button1_Click;
                }
            }
        }

        private ListBox _ListBox1;

        internal ListBox ListBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ListBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ListBox1 != null)
                {
                    _ListBox1.KeyDown -= Form1_KeyDown;
                    _ListBox1.KeyUp -= Form1_KeyUp;
                    _ListBox1.MouseDown -= ListBox1_MouseDown;
                    _ListBox1.MouseDoubleClick -= ListBox1_MouseDoubleClick;
                }

                _ListBox1 = value;
                if (_ListBox1 != null)
                {
                    _ListBox1.KeyDown += Form1_KeyDown;
                    _ListBox1.KeyUp += Form1_KeyUp;
                    _ListBox1.MouseDown += ListBox1_MouseDown;
                    _ListBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
                }
            }
        }

        

        private Button ChatButton;

        internal Button Button2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return ChatButton;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (ChatButton != null)
                {
                    ChatButton.KeyDown -= Form1_KeyDown;
                    ChatButton.KeyUp -= Form1_KeyUp;
                    ChatButton.Click -= Button2_Click;
                }

                ChatButton = value;
                if (ChatButton != null)
                {
                    ChatButton.KeyDown += Form1_KeyDown;
                    ChatButton.KeyUp += Form1_KeyUp;
                    ChatButton.Click += Button2_Click;
                }
            }
        }

        private Timer _Timer3;

        internal Timer Timer3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Timer3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Timer3 != null)
                {
                    _Timer3.Tick -= Timer3_Tick;
                }

                _Timer3 = value;
                if (_Timer3 != null)
                {
                    _Timer3.Tick += Timer3_Tick;
                }
            }
        }

        private ProgressBar _ProgressBar1;

        internal ProgressBar ProgressBar1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ProgressBar1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ProgressBar1 != null)
                {
                    _ProgressBar1.KeyDown -= Form1_KeyDown;
                    _ProgressBar1.KeyUp -= Form1_KeyUp;
                }

                _ProgressBar1 = value;
                if (_ProgressBar1 != null)
                {
                    _ProgressBar1.KeyDown += Form1_KeyDown;
                    _ProgressBar1.KeyUp += Form1_KeyUp;
                }
            }
        }

        internal ToolTip ToolTip1;
        internal ContextMenuStrip ContextMenuStrip1;
        private Label _Warning;

        internal Label Warning
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Warning;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Warning != null)
                {
                    _Warning.KeyDown -= Form1_KeyDown;
                    _Warning.KeyUp -= Form1_KeyUp;
                }

                _Warning = value;
                if (_Warning != null)
                {
                    _Warning.KeyDown += Form1_KeyDown;
                    _Warning.KeyUp += Form1_KeyUp;
                }
            }
        }

        private Button _ButtonLeft;

        internal Button ButtonLeft
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonLeft;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonLeft != null)
                {
                    _ButtonLeft.KeyDown -= Form1_KeyDown;
                    _ButtonLeft.KeyUp -= Form1_KeyUp;
                    _ButtonLeft.MouseDown -= Button3_MouseDown;
                    _ButtonLeft.MouseUp -= Button3_MouseUp;
                }

                _ButtonLeft = value;
                if (_ButtonLeft != null)
                {
                    _ButtonLeft.KeyDown += Form1_KeyDown;
                    _ButtonLeft.KeyUp += Form1_KeyUp;
                    _ButtonLeft.MouseDown += Button3_MouseDown;
                    _ButtonLeft.MouseUp += Button3_MouseUp;
                }
            }
        }

        private Button _ButtonJump;

        internal Button ButtonJump
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonJump;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonJump != null)
                {
                    _ButtonJump.KeyDown -= Form1_KeyDown;
                    _ButtonJump.KeyUp -= Form1_KeyUp;
                    _ButtonJump.Click -= Button5_Click;
                    _ButtonJump.MouseDown -= ButtonJump_MouseDown;
                }

                _ButtonJump = value;
                if (_ButtonJump != null)
                {
                    _ButtonJump.KeyDown += Form1_KeyDown;
                    _ButtonJump.KeyUp += Form1_KeyUp;
                    _ButtonJump.Click += Button5_Click;
                    _ButtonJump.MouseDown += ButtonJump_MouseDown;
                }
            }
        }

        private Button _ButtonRight;

        internal Button ButtonRight
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonRight;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonRight != null)
                {
                    _ButtonRight.KeyDown -= Form1_KeyDown;
                    _ButtonRight.KeyUp -= Form1_KeyUp;
                    _ButtonRight.MouseDown -= Button6_MouseDown;
                    _ButtonRight.MouseUp -= Button6_MouseUp;
                }

                _ButtonRight = value;
                if (_ButtonRight != null)
                {
                    _ButtonRight.KeyDown += Form1_KeyDown;
                    _ButtonRight.KeyUp += Form1_KeyUp;
                    _ButtonRight.MouseDown += Button6_MouseDown;
                    _ButtonRight.MouseUp += Button6_MouseUp;
                }
            }
        }

        private Button MenuButton;

        internal Button Button3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return MenuButton;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (MenuButton != null)
                {
                    MenuButton.KeyDown -= Form1_KeyDown;
                    MenuButton.KeyUp -= Form1_KeyUp;
                    MenuButton.Click -= Button3_Click_1;
                }

                MenuButton = value;
                if (MenuButton != null)
                {
                    MenuButton.KeyDown += Form1_KeyDown;
                    MenuButton.KeyUp += Form1_KeyUp;
                    MenuButton.Click += Button3_Click_1;
                }
            }
        }

        private Button _ButtonAttack;

        internal Button ButtonAttack
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ButtonAttack;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ButtonAttack != null)
                {
                    _ButtonAttack.KeyDown -= Form1_KeyDown;
                    _ButtonAttack.KeyUp -= Form1_KeyUp;
                    _ButtonAttack.Click -= ButtonAttack_Click;
                }

                _ButtonAttack = value;
                if (_ButtonAttack != null)
                {
                    _ButtonAttack.KeyDown += Form1_KeyDown;
                    _ButtonAttack.KeyUp += Form1_KeyUp;
                    _ButtonAttack.Click += ButtonAttack_Click;
                }
            }
        }

        internal TransparentPicBox localPlayer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _localPlayer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_localPlayer != null)
                {
                    _localPlayer.LocationChanged -= (_, __) => Test();
                    //_localPlayer.LocationChanged -= Test;
                }

                _localPlayer = value;
                if (_localPlayer != null)
                {
                    _localPlayer.LocationChanged += (_, __) => Test();
                    //_localPlayer.LocationChanged += Test;
                }
            }
        }

        internal TransparentPicBox R1;

        private ListBox _ListBox2;
        private ImageList imageList1;
        private ProgressBar progressBar1;
        private OpaqueLabel debuginfo;
        private Timer timer2;
        private PictureBox makeItDark;
        private Panel invPanel;
        private Button invClose1;
        private TextBox textBox1;
        private Button button1;
        private Panel chatPanel1;
        private RichTextBox richTextBox1;
        public TransparentPicBox _localPlayer;
        private ToolStripMenuItem пасхалкажмиСюдаToolStripMenuItem;
        private Label label1;
        private Label label2;

        internal ListBox ListBox2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ListBox2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ListBox2 != null)
                {
                    _ListBox2.MouseDoubleClick -= ListBox2_MouseDoubleClick;
                }

                _ListBox2 = value;
                if (_ListBox2 != null)
                {
                    _ListBox2.MouseDoubleClick += ListBox2_MouseDoubleClick;
                }
            }
        }
    }
}