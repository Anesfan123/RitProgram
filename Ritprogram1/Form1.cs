using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ritprogram1
{
    public partial class Form1 : Form
    {
        private bool isDrawing = false;
        private Point previousPoint;
        public Penna Penna; // Skapa en instans av Penna
        private Point startLinePoint; // För att hålla koll på linjens startpunkt
        private Point endLinePoint; // För att hålla koll på linjens slutpunkt
        private bool DrawingStraightLine = false; // Kollar vilken av ritfunktionerna som ska användas
        private readonly Bitmap bm = new Bitmap(800, 600);
        private bool DrawingRectangel = false;
        private bool DrawingEllipse = false;

        public Form1()
        {
            InitializeComponent();
            Penna = new Penna(Color.Black, 4.0f); // Skapa en Penna-instans med standardfärg och tjocklek  
        }
        
       
        private void btn_Rensa_Click(object sender, EventArgs e)
        {
            // Rensa PictureBox genom att rita en tom bild
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
            }

            // Uppdatera PictureBox för att visa den tomma bilden
            pic.Invalidate();
            DrawingStraightLine = false;
            DrawingEllipse = false;
            DrawingRectangel = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            previousPoint = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                if (DrawingRectangel == true)
                {
                    startLinePoint = e.Location;
                    endLinePoint = e.Location;
                }
                else
                {
                    isDrawing = true;
                }
            }
           
            if (e.Button == MouseButtons.Left)
            {
                if (DrawingStraightLine == true)
                {

                    startLinePoint = e.Location; // Spara startpunkten för linjen
                    endLinePoint = e.Location;   // Sätt slutpunkten till samma som startpunkten
                }
                else
                {
                    // Användaren ritar en frihandlinje
                    isDrawing = false;             // Flagga för att indikera att vi ritar
                    previousPoint = e.Location;   // Spara musens position som den tidigare punkten
                }
            }

            if(e.Button == MouseButtons.Left)
            {
                if(DrawingEllipse == true)
                {
                    startLinePoint = e.Location;
                    endLinePoint = e.Location;
                }
                else
                {
                isDrawing = false;
                    previousPoint = e.Location;
                }
                
            }
        }

        public void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    // Använd minPenna för att rita
                    Penna.Rita(previousPoint, e.Location, g);
                }
                previousPoint = e.Location;
                pic.Invalidate();
            }
            
                        
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;

            if (e.Button == MouseButtons.Left)
            {
                if (DrawingRectangel== true)
                {
                    endLinePoint = e.Location;
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        //Tar alla koordinater och gör om de till variabler.
                        Pen pen = new Pen(Color.Black, 9);
                        int x = Math.Min(startLinePoint.X, endLinePoint.X);
                        int y = Math.Min(startLinePoint.Y, endLinePoint.Y);
                        int w = Math.Abs(endLinePoint.X - startLinePoint.X);
                        int h = Math.Abs(endLinePoint.Y - startLinePoint.Y);
                        g.DrawRectangle(pen, new Rectangle(x, y, w, h));
                    }
                    pic.Invalidate();
                }
                else
                {
                    isDrawing = false;
                }
            }
         
            if (e.Button == MouseButtons.Left)
            {
                if (DrawingStraightLine == true )
                {
                    // Användaren ritar en rak linje
                    endLinePoint = e.Location; // Spara slutpunkten för linjen
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        g.DrawLine(Pens.Black, startLinePoint, endLinePoint); // Rita linjen från startpunkt till slutpunkt
                    }
                    pic.Invalidate(); // Uppdatera PictureBox för att visa ritningen
                }
                else
                {
                    // Användaren ritar en frihandlinje
                    isDrawing = false; // Sluta rita
                }
            } 
            
            if (DrawingEllipse == true)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    Pen pen = new Pen(Color.Black, 5);
                    g.DrawEllipse(pen, new Rectangle(endLinePoint.X, endLinePoint.Y, (e.Location.X - endLinePoint.X), (e.Location.Y - endLinePoint.Y)));
                }
                pic.Invalidate();
            }
            else
            {
                isDrawing = false;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        } 
        private void Färg_Click(object sender, EventArgs e)
        {
            Penna.ÄndraFärg();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Penna.ÄndraTjocklek(trackBar1.Value);
        }

        public void Save()
        {
            //Metod som sparar användarens målning 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Spara bilden på den valda sökvägen som PNG
                bm.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }

        private void btn_Spara_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btn_Linje_Click(object sender, EventArgs e)
        {
            DrawingStraightLine = true;
            DrawingRectangel = false;
            DrawingEllipse = false;
            isDrawing = false;
            
        }

        private void btn_Cirkel_Click(object sender, EventArgs e)
        {
            DrawingEllipse = true;
            DrawingRectangel = false;
            DrawingStraightLine = false;
            isDrawing = false;
           
        }

        private void btn_Rektangel_Click(object sender, EventArgs e)
        {
            DrawingRectangel = true;
            DrawingEllipse = false;
            DrawingStraightLine = false;
            isDrawing = false;
            
        }

        private void btn_Penna_Click(object sender, EventArgs e)
        {
            isDrawing = true;
            DrawingRectangel = false;
            DrawingStraightLine = false;
            DrawingEllipse = false;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
