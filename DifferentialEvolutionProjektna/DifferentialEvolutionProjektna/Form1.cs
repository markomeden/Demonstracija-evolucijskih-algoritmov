using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DifferentialEvolutionProjektna
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            for (int i = 0; i < 144; i++)
                tabela[i] = 0;

            InitializeComponent();
        }

        public int[] tabela = new int[144];

        private void button1_Click(object sender, EventArgs e)
        {
            int pop_size = Convert.ToInt32(textBox1.Text);
            int D = 144;
            int max_eval = Convert.ToInt32(textBox2.Text);
            double CR = Convert.ToDouble(textBox3.Text);
            double F = Convert.ToDouble(textBox4.Text);

            DE(pop_size, max_eval, CR, F, D);
        }

        private void narisi(int[] tabela2)
        {
            for (int i = 0; i < 144; i++)
            {
                PictureBox pb = this.Controls.Find("b" + (i+1), true).FirstOrDefault() as PictureBox;
                if (tabela2[i] == 0) pb.BackColor = System.Drawing.Color.Black; else pb.BackColor = System.Drawing.Color.White;
            }
        }

        public void DE(int pop_size, int max_eval, double CR, double F, int D)
        {

         int counter = 0; //za vsakih toliko generacij

        int[] tabela2 = new int[D];

		int eval = 0;
		Random generator = new Random();
		double ri, rez = 0, rez2 = 0;
		int R;
		double best = Double.MaxValue;
		double[,]agenti = new double[pop_size,D];
		double[] x;
		int a = 0 , b = 0, c = 0;
		double[] y;
		double[] bestX = new double[D];
		
		for(int i=0; i<pop_size; i++)
		{
			x = new double[D];
			
			for(int j=0; j<D; j++)
                agenti[i, j] = generator.NextDouble(); //od 0 do 0.5 je črno, od 0.5 do 1 pa belo
		}
		
		while(eval<max_eval)
		{
			for(int i=0; i<pop_size; i++)
			{
				do
				{
					a = generator.Next(pop_size);
				}
				while (a == i);
				
				do
				{
					b = generator.Next(pop_size);
				}
				while (b == i || b==a);
				
				do
				{
					c = generator.Next(pop_size);
				}
				while (c == i || c==a || c==b);
				
				R = generator.Next(D);
				y = new double[D];
				
				for(int j=0; j<D;j++)
				{
					ri = generator.NextDouble();

                    if (ri < CR || i == R)
                    {
                        y[j] = (agenti[a, j] + F * generator.NextDouble() * (agenti[b, j] - agenti[c, j]));
                        if (y[j] < 0) y[j] = 0;
                        if (y[j] > 1) y[j] = 1;
                    }
                    else
                        y[j] = agenti[i, j];
				}

                rez=0;
                rez2 = 0;
                double rez3 = 0;
                for (int z = 0; z < D; z++)
                {
                    rez3+=tabela[z];
                        rez+=Math.Abs(tabela[z]-y[z]);
                        rez2 += Math.Abs(tabela[z] - agenti[i,z]);

                }
               // Console.WriteLine(rez + " ," + rez2);
                if (rez < rez2)
                {
                
                    for (int ii = 0; ii < D; ii++)
                    {
                        agenti[i,ii] = y[ii];
                    }}
                    if (rez < best)
                    {
                        //Console.WriteLine(best + "  RISI TABLE" + rez);
                       // Console.WriteLine(eval + ": " + rez + " ," + rez2+" "+rez3);
                        best = rez;
                        for (int ii = 0; ii < D; ii++)
                        {
                            bestX[ii] = y[ii];
                        }
                        for (int z = 0; z < D; z++)
                        {
                            if (bestX[z] < 0.5)
                                tabela2[z] = 0;
                            else
                                tabela2[z] = 1;
                        }
                        narisi(tabela2);
                    }
			}

            //shrani sliko vsakih x generacij
            if (checkBox1.Checked)
            {
                if (textBox5.Text != "")
                    counter = Convert.ToInt32(textBox5.Text);
                if (eval % counter == 0)
                {
                    Bitmap img = new Bitmap(@"C:\SLIKICE\original.bmp");
                    Bitmap bitmap = new Bitmap(img.Clone(new Rectangle(0, 0, 240, 240), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                    int stevec = 0;
                    for (int yyy = 0; yyy < 240; yyy += 20)
                    {
                        for (int xxx = 0; xxx < 240; xxx += 20)
                        {
                            PictureBox pb = this.Controls.Find("b" + (stevec + 1), true).FirstOrDefault() as PictureBox;

                            if (pb.BackColor == System.Drawing.Color.Black)
                            {
                                for (int i = xxx; i < xxx + 20; i++)
                                    for (int j = yyy; j < yyy + 20; j++)
                                        bitmap.SetPixel(i, j, Color.Black);
                            }
                            stevec++;
                        }
                    }
                    bitmap.Save(@"C:\Rezultat\" + textBox6.Text + ""+ eval + ".bmp");
                }
            }

			eval++;
			rez=0;
			rez2=0;

           
		}
	}

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    Bitmap img = new Bitmap(file);
                    int stevec = 0;
                    for (int y = 0; y < 12; y++)
                    {
                        for (int x = 0; x < 12; x++)
                        {
                            Color pixelColor = img.GetPixel(x, y);
                            if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0)
                            {
                                PictureBox pb = this.Controls.Find("a" + (stevec + 1), true).FirstOrDefault() as PictureBox;
                                pb.BackColor = System.Drawing.Color.Black;
                                tabela[stevec] = 0;
                            }
                            else
                            {
                                PictureBox pb = this.Controls.Find("a" + (stevec + 1), true).FirstOrDefault() as PictureBox;
                                pb.BackColor = System.Drawing.Color.White;
                                tabela[stevec] = 1;
                            }
                            richTextBox1.Text += tabela[stevec];
                            stevec++;
                        }
                        richTextBox1.Text += "\n";
                    }
                }
                catch (IOException)
                {
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(@"C:\SLIKICE\original.bmp");
            Bitmap bitmap = new Bitmap(img.Clone(new Rectangle(0, 0, 240, 240), System.Drawing.Imaging.PixelFormat.Format32bppArgb));

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp";
            saveFileDialog1.Title = "Shrani Sliko";
            saveFileDialog1.InitialDirectory = @"C:\";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                    int stevec = 0;
                    for (int y = 0; y < 240; y+=20)
                    {
                        for (int x = 0; x < 240; x+=20)
                        {
                            PictureBox pb = this.Controls.Find("b" + (stevec + 1), true).FirstOrDefault() as PictureBox;

                            if (pb.BackColor == System.Drawing.Color.Black)
                            {
                                for(int i=x; i<x+20; i++)
                                    for(int j=y; j<y+20; j++)
                                bitmap.SetPixel(i, j, Color.Black);
                            }
                            stevec++;
                        }
                    }
                    //string text = File.ReadAllText(file);
                    //label2.Text = text;
                }
            bitmap.Save(saveFileDialog1.FileName);
        }
        }
        }

