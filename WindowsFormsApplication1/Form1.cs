using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            imageBox1.Image = CvInvoke.Imread(@"D:\Emgu\emgucv-windesktop 3.1.0.2504\Emgu.CV.Example\Stitching\stitch3.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
        }
    }
}
