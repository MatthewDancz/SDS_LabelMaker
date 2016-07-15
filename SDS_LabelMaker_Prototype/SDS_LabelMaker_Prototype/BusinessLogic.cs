using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace SDS_LabelMaker_Prototype
{
    class BusinessLogic
    {
        DAL newDal = new DAL();
        SDSLabel newLabel = new SDSLabel();
        public void UpdateLabelText(string s)
        {

        }

        public void UpdateChemicalPictograms()
        {

        }

        public void UpdateDataBase()
        {
            newDal.SaveData(newLabel);
        }

        public void UpdateLabelData(string s)
        {
            newLabel.ChemicalName = s;
        }
    }

    class DAL
    {
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"/SDSLabels.xml";

        private string[] nodeNames = new string[]
        {
            "ChemicalName",
            "CASRN",
            "SignalWord",
            "HazardStatement",
            //Something for the pictorgrams
            "ChemicalManufacturer",
        };

        XmlTextWriter xWriter = null;
        
        public void SaveData(SDSLabel l)
        {
            if (!File.Exists(path))
            {
                createData(l);
                return;
            }

            bool ToAdd = shouldLabelBeAdded(l);//Conditional Check for labels existence
            if (ToAdd)
            {
                addSDSLabel(l);
            }
            if (!ToAdd)
            {
                updateData(l);
            }
        }

        public bool shouldLabelBeAdded(SDSLabel l)
        {
            return true;
        }

        private void createData(SDSLabel l)
        {
            xWriter = new XmlTextWriter(path, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.WriteStartElement("SDSLabels");
            xWriter.WriteStartElement("Label");

            for (int i = 0; i < nodeNames.Length; i++)
            {
                xWriter.WriteStartElement(nodeNames[i]);
                xWriter.WriteString(l.Properties[i]);
                xWriter.WriteEndElement();
            }

            xWriter.WriteEndElement();//End Label Node
            xWriter.WriteEndElement();//End SDSLabels Node
            xWriter.Close();
        }

        private void addSDSLabel(SDSLabel l)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNode label = xDoc.CreateElement("Label");
            for (int i = 0; i < l.Properties.Length; i++)
            {
                XmlNode x = xDoc.CreateElement(nodeNames[i]);
                x.InnerText = l.Properties[i];
                label.AppendChild(x);
            }

            xDoc.DocumentElement.AppendChild(label);
            xDoc.Save(path);
            MessageBox.Show("Label data saved.");

        }

        private void updateData(SDSLabel l)
        {
            
        }

        public void RetrieveData()
        {

        }

        public void DeleteData()
        {

        }
    }

    class SDSLabel
    {
        public string[] Properties = null;
        public string ChemicalName = null;
        public string CASRN = null;
        public string SignalWord = null;
        public string[] HazardStatements = null;

        //Some way to store pictures.

        public string[] ChemicalManufacturer = null;
    }

    class HazardStatement
    {

    }
}
