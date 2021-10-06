using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Newtonsoft.Json;
using Minecraft2D;
using System.Text.RegularExpressions;

internal static partial class FormJSON
{
    public static JsonForm Parse(string str)
    {
        return JsonConvert.DeserializeObject<JsonForm>(str);
    }
}

public partial class JsonControl
{
    public string Type { get; set; }
    public Point Position { get; set; }
    public Size Size { get; set; }
    public bool Enabled { get; set; }
    public bool Visible { get; set; }
    public string Text { get; set; }
    public string Name { get; set; }
    public Color ForeColor { get; set; }
    public Color BackColor { get; set; }
    public bool DisableFlatStyle { get; set; } = true;
    public FlatStyle FlatStyle { get; set; }
    public FlatButtonAppearance FlatAppearance { get; set; }
    public string LoadScript { get; set; }
    public Dictionary<string, string> EventHandlers { get; set; }
    public BorderStyle BorderStyle { get; set; } = BorderStyle.FixedSingle;
    public DockStyle Dock { get; set; } = DockStyle.Top;
}

public partial class JsonForm
{
    private Random rnd = new Random();
    public static Dictionary<string, object[]> Objects = new Dictionary<string, object[]>();

    public List<JsonControl> Controls { get; set; } = new List<JsonControl>();
    public string Text { get; set; }
    public Size Size { get; set; }
    public string LoadScript { get; set; }
    public FormBorderStyle BorderStyle { get; set; } = FormBorderStyle.Sizable;

    public static MsgBoxResult MsgBox(string Prompt, MsgBoxStyle Style = MsgBoxStyle.ApplicationModal, string Title = null)
    {
        return Interaction.MsgBox(Prompt, Style, Title);
    }

    public Form GetWinForm()
    {
        var frm = new Form();
        frm.Text = Text;
        frm.Size = Size;
        frm.FormBorderStyle = BorderStyle;
        foreach (var ct in Controls)
        {
            switch (ct.Type)
            {
                case "BUTTON":
                    {
                        var btn = new Button();
                        btn.Name = ct.Name;
                        btn.Text = ct.Text;
                        if (!ct.DisableFlatStyle)
                        {
                            btn.FlatStyle = ct.FlatStyle;
                            {
                                var withBlock = btn.FlatAppearance;
                                withBlock.BorderColor = ct.FlatAppearance.BorderColor;
                                withBlock.BorderSize = ct.FlatAppearance.BorderSize;
                                withBlock.CheckedBackColor = ct.FlatAppearance.CheckedBackColor;
                                withBlock.MouseDownBackColor = ct.FlatAppearance.MouseDownBackColor;
                                withBlock.MouseOverBackColor = ct.FlatAppearance.MouseOverBackColor;
                            }
                        }

                        if (btn.Dock == DockStyle.None)
                        {
                            btn.Location = ct.Position;
                            btn.Size = ct.Size;
                        }

                        btn.Enabled = ct.Enabled;
                        btn.Visible = ct.Visible;
                        btn.Dock = ct.Dock;
                        if (!Information.IsNothing(ct.ForeColor))
                        {
                            btn.ForeColor = ct.ForeColor;
                        }

                        if (!Information.IsNothing(ct.BackColor))
                        {
                            btn.BackColor = ct.BackColor;
                        }

                        frm.Controls.Add(btn);
                        // Dim opt = ScriptOptions.Default
                        // opt.AddReferences({"System", "System.Core", "System.IO", "System.Linq", "System.Text", "System.Net", "System.Windows.Forms", "System.Diagnostics", "System.Windows"})
                        // opt.AddImports({"System", "System.IO", "System.Linq", "System.Text", "System.Net", "System.Windows.Forms", "System.Diagnostics", "System.Windows"})
                        // CSharpScript.RunAsync(File.ReadAllText(Command), opt)
                        string id = Guid.NewGuid().ToString();
                        Objects.Add(id, new object[] { frm, ct, btn });
                        if (!Information.IsNothing(ct.LoadScript))
                        {
                            btn.Click += (a, b) => {
                                string s = ct.LoadScript;
                                foreach (Control f in btn.FindForm().Controls)
                                {
                                    if (f.GetType() == typeof(TextBox))
                                    {
                                        s = s.Replace($"%TEXT.{f.Name}%", f.Text);
                                    }
                                    if (f.GetType() == typeof(CheckBox))
                                    {
                                        s = s.Replace($"%STATE.{f.Name}%", ((CheckBox)f).Checked ? "CHECKED" : "UNCHECKED");
                                    }
                                }
                                Form1.GetInstance().Send("menucommand?" + s);
                            };
                        }

                        break;
                    }

                case "TEXTBOX":
                    {
                        var tb = new TextBox();
                        tb.Name = ct.Name;
                        tb.Text = ct.Text;
                        tb.BorderStyle = ct.BorderStyle;
                        if (!Information.IsNothing(ct.FlatStyle))
                        {
                            throw new ArgumentException("FlatStyle is not accepted for TEXTBOX");
                        }

                        if (!Information.IsNothing(ct.FlatAppearance))
                        {
                            throw new ArgumentException("FlatAppearance is not accepted for TEXTBOX");
                        }

                        if (tb.Dock == DockStyle.None)
                        {
                            tb.Location = ct.Position;
                            tb.Size = ct.Size;
                        }

                        tb.Dock = ct.Dock;
                        tb.Enabled = ct.Enabled;
                        if (!Information.IsNothing(ct.ForeColor))
                        {
                            tb.ForeColor = ct.ForeColor;
                        }

                        if (!Information.IsNothing(ct.BackColor))
                        {
                            tb.BackColor = ct.BackColor;
                        }

                        tb.Visible = ct.Visible;
                        frm.Controls.Add(tb);
                        string id = Guid.NewGuid().ToString();
                        Objects.Add(id, new object[] { frm, ct, tb });
                        if (!Information.IsNothing(ct.LoadScript))
                        {
                            
                        }

                        break;
                    }

                case "PANEL":
                    {
                        var tb = new Panel();
                        tb.Name = ct.Name;
                        tb.Text = ct.Text;
                        tb.BorderStyle = ct.BorderStyle;
                        tb.Dock = ct.Dock;
                        if (!Information.IsNothing(ct.FlatStyle))
                        {
                            throw new ArgumentException("FlatStyle is not accepted for PANEL");
                        }

                        if (!Information.IsNothing(ct.FlatAppearance))
                        {
                            throw new ArgumentException("FlatAppearance is not accepted for PANEL");
                        }

                        if (tb.Dock == DockStyle.None)
                        {
                            tb.Location = ct.Position;
                            tb.Size = ct.Size;
                        }

                        tb.Enabled = ct.Enabled;
                        if (!Information.IsNothing(ct.ForeColor))
                        {
                            tb.ForeColor = ct.ForeColor;
                        }

                        if (!Information.IsNothing(ct.BackColor))
                        {
                            tb.BackColor = ct.BackColor;
                        }

                        tb.Visible = ct.Visible;
                        frm.Controls.Add(tb);
                        string id = Guid.NewGuid().ToString();
                        Objects.Add(id, new object[] { frm, ct, tb });
                        if (!Information.IsNothing(ct.LoadScript))
                        {
                            
                        }

                        break;
                    }
                case "LABEL":
                    {
                        var tb = new Label();
                        tb.Name = ct.Name;
                        tb.Text = ct.Text;
                        tb.BorderStyle = ct.BorderStyle;
                        if (!Information.IsNothing(ct.FlatStyle))
                        {
                            throw new ArgumentException("FlatStyle is not accepted for LABEL");
                        }

                        if (!Information.IsNothing(ct.FlatAppearance))
                        {
                            throw new ArgumentException("FlatAppearance is not accepted for LABEL");
                        }

                        if (tb.Dock == DockStyle.None)
                        {
                            tb.Location = ct.Position;
                            tb.Size = ct.Size;
                        }

                        tb.Enabled = ct.Enabled;
                        if (!Information.IsNothing(ct.ForeColor))
                        {
                            tb.ForeColor = ct.ForeColor;
                        }

                        if (!Information.IsNothing(ct.BackColor))
                        {
                            tb.BackColor = ct.BackColor;
                        }

                        tb.Visible = ct.Visible;
                        tb.Dock = ct.Dock;
                        frm.Controls.Add(tb);
                        string id = Guid.NewGuid().ToString();
                        Objects.Add(id, new object[] { frm, ct, tb });
                        if (!Information.IsNothing(ct.LoadScript))
                        {
                            tb.Click += (a, b) => {
                                string s = ct.LoadScript;
                                foreach (Control f in tb.FindForm().Controls)
                                {
                                    if (f.GetType() == typeof(TextBox))
                                    {
                                        s = s.Replace($"%TEXT.{f.Name}%", f.Text);
                                    }
                                    if (f.GetType() == typeof(CheckBox))
                                    {
                                        s = s.Replace($"%STATE.{f.Name}%", ((CheckBox)f).Checked ? "CHECKED" : "UNCHECKED");
                                    }
                                }
                                Form1.GetInstance().Send("menucommand?" + s);
                            };
                        }

                        break;
                    }

                case "CHECKBOX":
                    {
                        var cb = new CheckBox();
                        cb.Name = ct.Name;
                        cb.Text = ct.Text;
                        if (!ct.DisableFlatStyle)
                        {
                            cb.FlatStyle = ct.FlatStyle;
                            {
                                var withBlock1 = cb.FlatAppearance;
                                withBlock1.BorderColor = ct.FlatAppearance.BorderColor;
                                withBlock1.BorderSize = ct.FlatAppearance.BorderSize;
                                withBlock1.CheckedBackColor = ct.FlatAppearance.CheckedBackColor;
                                withBlock1.MouseDownBackColor = ct.FlatAppearance.MouseDownBackColor;
                                withBlock1.MouseOverBackColor = ct.FlatAppearance.MouseOverBackColor;
                            }
                        }

                        if (!Information.IsNothing(ct.ForeColor))
                        {
                            cb.ForeColor = ct.ForeColor;
                        }

                        if (!Information.IsNothing(ct.BackColor))
                        {
                            cb.BackColor = ct.BackColor;
                        }

                        if (cb.Dock == DockStyle.None)
                        {
                            cb.Location = ct.Position;
                            cb.Size = ct.Size;
                        }

                        cb.Enabled = ct.Enabled;
                        cb.Visible = ct.Visible;
                        cb.Dock = ct.Dock;
                        frm.Controls.Add(cb);
                        string id = Guid.NewGuid().ToString();
                        Objects.Add(id, new object[] { frm, ct, cb });
                        if (!Information.IsNothing(ct.LoadScript))
                        {
                            cb.Click += (a, b) => {
                                string s = ct.LoadScript;
                                foreach (Control f in cb.FindForm().Controls)
                                {
                                    if (f.GetType() == typeof(TextBox))
                                    {
                                        s = s.Replace($"%TEXT.{f.Name}%", f.Text);
                                    }
                                    if (f.GetType() == typeof(CheckBox))
                                    {
                                        s = s.Replace($"%STATE.{f.Name}%", ((CheckBox)f).Checked ? "CHECKED" : "UNCHECKED");
                                    }
                                }
                                Form1.GetInstance().Send("menucommand?" + s);
                            };
                        }

                        break;
                    }
            }
        }

        return frm;
    }
}