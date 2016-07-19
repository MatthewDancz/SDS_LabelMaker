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
    class ServiceLayer
    {
        BusinessLogic BL = new BusinessLogic();
        public void FormatLabel(string name, string id, string signal, string hazards, string manufacturer)
        {
            BL.UpdateLabel(name, id, signal, hazards, manufacturer);
        }

        public void SaveLabel()
        {
            BL.UpdateDataBase();
        }
    }

    class BusinessLogic
    {
        DAL newDal = new DAL();
        SDSLabel newLabel = new SDSLabel();

        public void UpdateLabel(string name, string id, string signal, string hazards, string manufacturer)
        {
            newLabel.ChemicalName = name;
            newLabel.CASRN = id;
            newLabel.SignalWord = signal;
            newLabel.HazardStatement = hazards;
            //for (int i = 0; i < hazards.Length; i++)
            //{
            //    newLabel.HazardStatements.Add(hazards[i]);
            //}

            newLabel.ChemicalManufacturer = manufacturer;
            newLabel.populateProperties();
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
            "ChemicalName",         //index of 0
            "CASRN",                //index of 1
            "SignalWord",           //index of 2
            "HazardStatement",      //index of 3
            "ChemicalManufacturer", //index of 4
    
            //Something for the pictorgrams

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
            string[] dataArray = l.GetPropertiesArray();

            xWriter = new XmlTextWriter(path, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.WriteStartElement("SDSLabels");
            xWriter.WriteStartElement("Label");

            //Write the Chemical Name, CASRN, and Signal Word in XML format.
            for (int i = 0; i < dataArray.Length; i++)
            {
                xWriter.WriteStartElement(nodeNames[i]);
                xWriter.WriteString(dataArray[i]);
                xWriter.WriteEndElement();
            }

            ////Write the Chemical Name in XML format.
            //xWriter.WriteStartElement(nodeNames[0]);
            //xWriter.WriteString(l.Properties.ElementAt(0));
            //xWriter.WriteEndElement();

            ////Write the CASRN in XML format.
            //xWriter.WriteStartElement(nodeNames[1]);
            //xWriter.WriteString(l.Properties.ElementAt(1));
            //xWriter.WriteEndElement();

            ////Write the Signal Word in XML format.
            //xWriter.WriteStartElement(nodeNames[2]);
            //xWriter.WriteString(l.Properties.ElementAt(2));
            //xWriter.WriteEndElement();

            //Write the Hazard Statements in XML format.
            //foreach (string s in l.HazardStatements)
            //{
            //    xWriter.WriteStartElement(nodeNames[3]);
            //    xWriter.WriteString(s);
            //    xWriter.WriteEndElement();
            //}

            ////Write the Chemical Manufacturer in XML format.
            //xWriter.WriteStartElement(nodeNames[4]);
            //xWriter.WriteString(l.ChemicalManufacturer);
            //xWriter.WriteEndElement();
 
            xWriter.WriteEndElement();//End Label Node
            xWriter.WriteEndElement();//End SDSLabels Node
            xWriter.Close();
        }

        private void addSDSLabel(SDSLabel l)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNode label = xDoc.CreateElement("Label");
            for (int i = 0; i < l.Properties.Count; i++)
            {
                XmlNode x = xDoc.CreateElement(nodeNames[i]);
                x.InnerText = l.Properties.ElementAt(i);
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
        public List<string> Properties = new List<string>();

        public string ChemicalName = null;
        public string CASRN = null;
        public string SignalWord = null;
        public string HazardStatement = null;
        public List<string> HazardStatements = new List<string>();

        //Some way to store pictures.

        public string ChemicalManufacturer = null;

        public void populateProperties()
        {
            Properties.Add(ChemicalName);
            Properties.Add(CASRN);
            Properties.Add(SignalWord);
            Properties.Add(HazardStatement);

            //foreach (string s in HazardStatements)
            //{
            //    Properties.Add(s);
            //}

            Properties.Add(ChemicalManufacturer);
        }

        public void AddHazardStatement(string s)
        {
            HazardStatements.Add(s);
        }

        public string[] GetPropertiesArray()
        {
            return Properties.ToArray();
        }
    }
}
