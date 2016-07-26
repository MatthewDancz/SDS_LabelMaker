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

        string enter = "\n";

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Font font1 = new Font("Times New Roman", 20, FontStyle.Regular);

            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 200);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SL.FormatLabel(comboBoxName.Text, comboBoxCASRN.Text, comboBoxSignalWord.Text, textBoxHazardStatements.Text, textBoxChemicalManufacturer.Text);
            SL.SaveLabel();
        }

        private void UpdateLabelPreview()
        {
            Font baseFont = new Font("Times New Roman", 9.75F, FontStyle.Regular);
            Font fontName = new Font("Times New Roman", 20, FontStyle.Bold);

            //Reset the Font so everything looks right.
            richTextBox1.Font = baseFont;

            richTextBox1.Text = comboBoxName.Text + enter;
            richTextBox1.AppendText(comboBoxCASRN.Text + enter);
            richTextBox1.AppendText(comboBoxSignalWord.Text + enter);
            richTextBox1.AppendText(textBoxHazardStatements.Text + enter);
            richTextBox1.AppendText(textBoxChemicalManufacturer.Text);

            //Select the Chemical Name, Highlight it in black and change the color to white.
            richTextBox1.Select(0, comboBoxName.Text.Length);
            richTextBox1.SelectionBackColor = Color.Black;
            richTextBox1.SelectionColor = Color.White;
            richTextBox1.SelectionFont = fontName;

            //Select the Chemical Manufacturer and align it in the center.
            int ChemicalManufacturerIndex = richTextBox1.Text.LastIndexOf(textBoxChemicalManufacturer.Text);
            richTextBox1.Select(ChemicalManufacturerIndex, textBoxChemicalManufacturer.Text.Length);
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void textBoxChemicalManufacturer_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }

        private void textBoxHazardStatements_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }

        private void comboBoxSignalWord_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }

        private void comboBoxCASRN_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }

        private void comboBoxName_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }
    }
}
