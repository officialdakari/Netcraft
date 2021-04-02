using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Minecraft2D
{
    public class NCResources
    {
        Dictionary<string, Bitmap> res_bmp = new Dictionary<string, Bitmap>();
        Dictionary<string, string> res_txt = new Dictionary<string, string>();
        Dictionary<string, Stream> res_stream = new Dictionary<string, Stream>();

        Bitmap nullBmp;

        public static NCResources Load(string path)
        {
            NCResources ncr = new NCResources();
            Bitmap n = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(n))
            {
                //g.DrawString("NU", new Font("Lucida Console", 24, FontStyle.Regular), new SolidBrush(Color.Red), 1, 1, StringFormat.GenericTypographic);
                //g.DrawString("LL", new Font("Lucida Console", 24, FontStyle.Regular), new SolidBrush(Color.Red), 1, 32, StringFormat.GenericTypographic);
                g.FillRectangle(new SolidBrush(Color.Magenta), 0, 0, 32, 32);
                g.FillRectangle(new SolidBrush(Color.Magenta), 32, 32, 32, 32);
                g.FillRectangle(new SolidBrush(Color.Black), 0, 32, 32, 32);
                g.FillRectangle(new SolidBrush(Color.Black), 32, 0, 32, 32);
            }
            ncr.nullBmp = n;
                foreach (string f in Directory.GetFiles(path))
                {
                    string ext = f.Split(Path.AltDirectorySeparatorChar).Last().Split('.')[1];

                    switch (ext)
                    {
                        case "png":
                            ncr.res_bmp.Add(f.Split('\\').Last().Split('.')[0], (Bitmap)Image.FromFile(f));
                            break;
                        case "jpg":
                            ncr.res_bmp.Add(f.Split('\\').Last().Split('.')[0], (Bitmap)Image.FromFile(f));
                            break;
                        case "bmp":
                            ncr.res_bmp.Add(f.Split('\\').Last().Split('.')[0], (Bitmap)Image.FromFile(f));
                            break;
                        case "txt":
                            ncr.res_txt.Add(f.Split('\\').Last().Split('.')[0], File.ReadAllText(f));
                            break;
                        case "mp3":
                            ncr.res_stream.Add(f.Split('\\').Last().Split('.')[0], File.OpenRead(f));
                            break;
                        case "mp4":
                            ncr.res_stream.Add(f.Split('\\').Last().Split('.')[0], File.OpenRead(f));
                            break;
                        case "wav":
                            ncr.res_stream.Add(f.Split('\\').Last().Split('.')[0], File.OpenRead(f));
                            break;
                    }
                }

            return ncr;
        }

        public Bitmap GetBitmap(string key)
        {
            return res_bmp.ContainsKey(key) ? (Bitmap)res_bmp[key].Clone (): nullBmp;
        }

        public Bitmap GetBitmapFlipped(string key)
        {
            Bitmap bmp = res_bmp.ContainsKey(key) ? (Bitmap)res_bmp[key].Clone() : null ;
            if (bmp == null) return nullBmp;
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return bmp;
        }

        public string GetString(string key)
        {
            return res_txt.ContainsKey(key) ? res_txt[key] : null;
        }

        public Stream GetStream(string key)
        {
            return res_stream.ContainsKey(key) ? res_stream[key] : null;
        }
    }
}
