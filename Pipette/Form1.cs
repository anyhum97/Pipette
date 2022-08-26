using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace Pipette
{
	public partial class Form1 : Form
	{
		Bitmap Canvas = new Bitmap(256, 256);
		Bitmap Source = new Bitmap(32, 32);

		private int dx = 0;
		private int dy = 0;

		private int xPos = 0;
		private int yPos = 0;

		public Form1()
		{
			InitializeComponent();

			KeyPreview = true;
			TopMost = true;

			KeyDown += Form1_KeyDown;
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.ControlKey:
				{
					timer1.Enabled = !timer1.Enabled;
					dx = 0;
					dy = 0;
					break;
				}

				case Keys.Left:
				{
					if(dx > -14)
					{
						--dx;
					}
					
					DrawFrame();
					break;
				}

				case Keys.Right:
				{
					if(dx < 14)
					{
						++dx;
					}
					
					DrawFrame();
					break;
				}

				case Keys.Up:
				{
					if(dy > -14)
					{
						--dy;
					}

					DrawFrame();
					break;
				}

				case Keys.Down:
				{
					if(dy < 15)
					{
						++dy;
					}

					DrawFrame();
					break;
				}

				case Keys.E:
				{
					Export();
					break;
				}

				case Keys.Escape:
				{
					Close();
					break;
				}
			}
		}

		private void DrawFrame()
		{
			Bitmap bitmap2 = (Bitmap)Canvas.Clone();

			for(int i=128-10; i<128+2; ++i)
			{
				bitmap2.SetPixel(128-09+dx*8, i+dy*8, Color.Green);
				bitmap2.SetPixel(128-10+dx*8, i+dy*8, Color.Green);
				bitmap2.SetPixel(128-11+dx*8, i+dy*8, Color.Green);
			}

			for(int i=128-10; i<128+2; ++i)
			{
				bitmap2.SetPixel(128+0+dx*8, i+dy*8, Color.Green);
				bitmap2.SetPixel(128+1+dx*8, i+dy*8, Color.Green);
				bitmap2.SetPixel(128+2+dx*8, i+dy*8, Color.Green);
			}

			for(int i=128-10; i<128+2; ++i)
			{
				bitmap2.SetPixel(i+dx*8, 128-09+dy*8, Color.Green);
				bitmap2.SetPixel(i+dx*8, 128-10+dy*8, Color.Green);
				bitmap2.SetPixel(i+dx*8, 128-11+dy*8, Color.Green);
			}

			for(int i=128-10; i<128+2; ++i)
			{
				bitmap2.SetPixel(i+dx*8, 128+0+dy*8, Color.Green);
				bitmap2.SetPixel(i+dx*8, 128+1+dy*8, Color.Green);
				bitmap2.SetPixel(i+dx*8, 128+2+dy*8, Color.Green);
			}

			pictureBox1.Image = bitmap2;

			Color color1 = Source.GetPixel(15+dx, 15+dy);
			Color color2 = Source.GetPixel(16+dx, 15+dy);

			textBox1.Text = color1.R.ToString();
			textBox2.Text = color1.G.ToString();
			textBox3.Text = color1.B.ToString();

			textBox4.Text = (xPos + dx).ToString();
			textBox5.Text = (yPos + dy).ToString();

			textBox6.Text = color2.R.ToString();
			textBox7.Text = color2.G.ToString();
			textBox8.Text = color2.B.ToString();

			textBox9.Text = (xPos + 1 + dx).ToString();
			textBox10.Text = (yPos + dy).ToString();

			GC.Collect();
		}

		private void UpdateImage()
		{
			int x = Cursor.Position.X;
			int y = Cursor.Position.Y;

			int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
			int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height;

			using(Graphics graphics = Graphics.FromImage(Source))
			{
				if(x - 16 > 0 && x + 16 < width && y - 16 > 0 && y + 16 < height)
				{
					graphics.CopyFromScreen(x - 15, y - 15, 0, 0, Source.Size);
				}

				for(int i=0; i<32; ++i)
				{
					for(int j=0; j<32; ++j)
					{
						Color color = Source.GetPixel(i, j);

						for(int z=0; z<8; ++z)
						{
							for(int t=0; t<8; ++t)
							{
								Canvas.SetPixel(i*8+z, j*8+t, color);
							}
						}
					}
				}

				xPos = x;
				yPos = y;

				DrawFrame();

				GC.Collect();
			}
		}

		private void Export()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append("\t\tprivate static bool Check()\r\n\t\t{\r\n");

			stringBuilder.Append("\t\t\tPixels pixel = new Pixels\r\n\t\t\t{\r\n");
			
			stringBuilder.Append("\t\t\t\tColor1 = Color.FromArgb(");

			stringBuilder.Append(textBox1.Text.ToString() + ", ");
			stringBuilder.Append(textBox2.Text.ToString() + ", ");
			stringBuilder.Append(textBox3.Text.ToString() + "),\r\n");

			stringBuilder.Append("\t\t\t\tColor2 = Color.FromArgb(");

			stringBuilder.Append(textBox6.Text.ToString() + ", ");
			stringBuilder.Append(textBox7.Text.ToString() + ", ");
			stringBuilder.Append(textBox8.Text.ToString() + ")\r\n");

			stringBuilder.Append("\t\t\t};\r\n\r\n\t\t\treturn CheckSum.Check(");

			stringBuilder.Append(textBox4.Text + ", " + textBox5.Text + ", ");

			stringBuilder.Append("pixel, 1, 5);\r\n\t\t}\r\n");

			Clipboard.SetText(stringBuilder.ToString());
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateImage();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Export();
		}
	}
}

