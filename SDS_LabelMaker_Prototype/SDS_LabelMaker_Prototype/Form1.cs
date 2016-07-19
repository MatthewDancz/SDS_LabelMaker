using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDS_LabelMaker_Prototype
{
    public partial class Form1 : Form
    {
        ServiceLayer SL = new ServiceLayer();
        List<string> LabelData = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBoxChemicalManufacturer_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font1 = new Font("Times New Roman", 20, FontStyle.Regular);

            e.Graphics.DrawString(richTextBox1.Text, Font, Brushes.Black, 100, 200);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SL.FormatLabel(comboBoxName.Text, comboBoxCASRN.Text, comboBoxSignalWord.Text, textBoxHazardStatements.Text, textBoxChemicalManufacturer.Text);
            SL.SaveLabel();
        }
    }
}
