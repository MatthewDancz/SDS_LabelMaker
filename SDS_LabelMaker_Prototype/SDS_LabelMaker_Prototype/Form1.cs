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
        List<string> results = new List<string>();

        Font fontBase = new Font("Times New Roman", 9.75F, FontStyle.Regular);
        Font fontName = new Font("Times New Roman", 20, FontStyle.Bold);

        SDSLabel myLabel = new SDSLabel();

        string enter = "\n";

        public Form1()
        {
            InitializeComponent();
            updateProductList();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Font font1 = new Font("Times New Roman", 20, FontStyle.Regular);

            e.Graphics.DrawString(richTextBox1.Text, fontName, Brushes.Black, 100, 200);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SL.FormatLabel(textBoxProductName.Text, textBoxCASRN.Text, comboBoxSignalWord.Text, textBoxHazardStatements.Text, textBoxProductManufacturer.Text);
            SL.SaveLabel();
        }

        private void UpdateLabelPreview()
        {
            myLabel.ProductName = textBoxProductName.Text;
            myLabel.CASRN = textBoxCASRN.Text;
            myLabel.SignalWord = comboBoxSignalWord.Text;
            myLabel.HazardStatement = textBoxHazardStatements.Text;
            myLabel.ProductManufacturer = textBoxProductManufacturer.Text;
            myLabel.populateProperties();  

            richTextBox1.Text = myLabel.ProductName + enter;
            richTextBox1.AppendText(myLabel.CASRN + enter);
            richTextBox1.AppendText(myLabel.SignalWord + enter);
            richTextBox1.AppendText(myLabel.HazardStatement + enter);
            richTextBox1.AppendText(myLabel.ProductManufacturer);
            
            //foreach (string s in myLabel.Properties)
            //{
            //    int index = richTextBox1.Text.LastIndexOf(s);
            //    richTextBox1.Select(index, s.Length);
            //    if (myLabel.ProductName == s)
            //    {
            //        richTextBox1.SelectionBackColor = Color.Black;
            //        richTextBox1.SelectionFont = fontName;
            //    }
            //    else
            //    {
            //        richTextBox1.SelectionBackColor = Color.White;
            //        richTextBox1.SelectionFont = fontBase;
            //    }

            //    if (myLabel.ProductManufacturer == s)
            //    {
            //        richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            //    }
            //    else
            //    {
            //        richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
            //    }
            //}

            //Select the Chemical Name, Highlight it in black and change the color to white.
            richTextBox1.Select(0, myLabel.ProductName.Length);
            richTextBox1.SelectionBackColor = Color.Black;
            richTextBox1.SelectionColor = Color.White;
            richTextBox1.SelectionFont = fontName;

            //Select the CASRN list, and return the font to standard.
            int CASRNIndex = richTextBox1.Text.LastIndexOf(textBoxCASRN.Text);
            richTextBox1.Select(CASRNIndex, myLabel.CASRN.Length);
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionFont = fontBase;

            //Select the Signal Word, and return the font to standard.
            int SignalWordIndex = richTextBox1.Text.LastIndexOf(myLabel.SignalWord);
            richTextBox1.Select(SignalWordIndex, myLabel.SignalWord.Length);
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionFont = fontBase;

            //Select the Hazard Statements, and return their font to standard.
            int HazardStatementIndex = richTextBox1.Text.LastIndexOf(textBoxHazardStatements.Text);
            //richTextBox1.Select(HazardStatementIndex, textBoxHazardStatements.Text.Length);
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionFont = fontBase;

            //Select the Chemical Manufacturer, return the font to standard, and align it in the center.
            int ChemicalManufacturerIndex = richTextBox1.Text.LastIndexOf(textBoxProductManufacturer.Text);
            richTextBox1.Select(ChemicalManufacturerIndex, textBoxProductManufacturer.Text.Length);
            richTextBox1.SelectionBackColor = Color.White;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.SelectionFont = fontBase;
            richTextBox1.Select(ChemicalManufacturerIndex, textBoxProductManufacturer.Text.Length);
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

        private void textBoxCASRN_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
        }

        private void comboBoxName_TextChanged(object sender, EventArgs e)
        {
            UpdateLabelPreview();
            //updateAutoSuggestion();
        }

        private void updateProductList()
        {
            List<string> results = SL.SearchDataBase(textBoxProductName.Text);

            listBoxProductDataBase.Items.Clear();

            foreach (string s in results)
            {
                listBoxProductDataBase.Items.Add(s);
            }
        }

        //private void updateAutoSuggestion()
        //{
        //    List<string> results = SL.SearchDataBase(comboBoxName.Text);

        //    comboBoxName.Items.Clear();

        //    comboBoxName.SelectionStart = comboBoxName.Text.Length;
        //    comboBoxName.SelectionLength = 0;

        //    foreach (string s in results)
        //    {
        //        comboBoxName.Items.Add(s);
        //    }
        //}

        //private void comboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string s = comboBoxName.SelectedItem.ToString();
        //    SDSLabel r = SL.getLabelData(s.ToLower());

        //    comboBoxName.Text = r.ProductName;
        //    comboBoxCASRN.Text = r.CASRN;
        //    comboBoxSignalWord.Text = r.SignalWord;
        //    textBoxHazardStatements.Text = r.HazardStatement;
        //    textBoxProductManufacturer.Text = r.ProductManufacturer;
        //}

        private void listBoxProductDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = listBoxProductDataBase.SelectedItem.ToString();
            SDSLabel r = SL.getLabelData(s.ToLower());

            textBoxProductName.Text = r.ProductName;
            textBoxCASRN.Text = r.CASRN;
            comboBoxSignalWord.Text = r.SignalWord;
            textBoxHazardStatements.Text = r.HazardStatement;
            textBoxProductManufacturer.Text = r.ProductManufacturer;

            listBoxProductDataBase.Visible = false;
        }

        private void textBoxProductName_TextChanged(object sender, EventArgs e)
        {
            updateProductList();
            listBoxProductDataBase.Visible = true;
            if (listBoxProductDataBase.Items.Count == 0)
            {
                listBoxProductDataBase.Visible = false;
            }
        }

        
    }
}