using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GettingStartedDemo
{
    public partial class PowerScore : Form
    {
        public PowerScore()
        {
            InitializeComponent();
        }
        public void show_power(int power)
        {
            lblpower.Text = "Power: " + power;
        }
        public void show_stroke(int stroke)
        {
            lblstroke.Text = "Score: " + stroke;
        }

        public void show_par(int par)
        {
            parLabel.Text = "Par: " + par;
        }

        public void show_stroke_per_level(int strokes)
        {
            currentStrokeLabel.Text = "Strokes: " + strokes;
        }
        
    }
}
