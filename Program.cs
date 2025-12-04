using System;
using System.Drawing;
using System.Windows.Forms;

namespace FunctionPlot
{
    public partial class Form1 : Form
    {
        private readonly Pen _axisPen = new Pen(Color.Black, 2);
        private readonly Pen _graphPen = new Pen(Color.Red, 2);

        public Form1()
        {
            InitializeComponent();
            this.Resize += (s, e) => Invalidate(); 
            this.DoubleBuffered = true; 
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph(e.Graphics, this.ClientSize);
        }

        private void DrawGraph(Graphics graphics, Size area)
        {
            graphics.Clear(Color.White);

            // Padding for aesthetics
            int padding = 40;
            int width = area.Width - 2 * padding;
            int height = area.Height - 2 * padding;

            if (width <= 0 || height <= 0)
                return;

            // Draw axes
            graphics.DrawLine(_axisPen, padding, area.Height - padding, area.Width - padding, area.Height - padding); // X-axis
            graphics.DrawLine(_axisPen, padding, padding, padding, area.Height - padding); // Y-axis

            // Function boundaries
            double xStart = 0.0;
            double xEnd = 0.5;
            double step = 0.1;

            // Find max Y for scaling
            double maxY = 0;
            for (double x = xStart; x <= xEnd; x += step)
            {
                double y = CalculateFunction(x);
                if (y > maxY) maxY = y;
            }
            if (maxY == 0) maxY = 1;

            // Draw the graph
            PointF? prevPoint = null;

            for (double x = xStart; x <= xEnd + 0.0001; x += step)
            {
                double y = CalculateFunction(x);

                float scaledX = padding + (float)((x - xStart) / (xEnd - xStart) * width);
                float scaledY = area.Height - padding - (float)(y / maxY * height);

                PointF current = new PointF(scaledX, scaledY);

                if (prevPoint != null)
                {
                    graphics.DrawLine(_graphPen, prevPoint.Value, current);
                }

                prevPoint = current;
            }
        }

        private double CalculateFunction(double x)
        {
            return (2.5 * Math.Pow(x, 3)) / (Math.Exp(2 * x) + 2);
        }
    }
}
