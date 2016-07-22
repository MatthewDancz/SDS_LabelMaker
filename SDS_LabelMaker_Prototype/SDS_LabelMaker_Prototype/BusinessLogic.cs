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
        SDSLabel newLabel = null;
        DAL newDal = new DAL();

        public void UpdateLabel(string name, string id, string signal, string hazards, string manufacturer)
        {
            newLabel = new SDSLabel();
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

            bool ToAddName = NameCheck(l);//Conditional Check for labels existence
            bool ToAddCASRN = CASRNCheck(l);

            if (ToAddName)
            {
                addSDSLabel(l);
            }
            if (!ToAddName && ToAddCASRN)
            {
                MessageBox.Show("CASRN number already in use.");
            }
            if (!ToAddName && !ToAddCASRN)
            {
                updateData(l);
            }
        }

        //The following method may need to return a string detailing why a label was rejected for saving.
        public bool NameCheck(SDSLabel l)
        {
            //SDSLabels
            //Label
            //nodeNames
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNodeList nodes = xDoc.SelectNodes("SDSLabels/Label/ChemicalName");
            foreach (XmlNode node in nodes)
            {
                if (node.InnerText == l.ChemicalName)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CASRNCheck(SDSLabel l)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlNodeList nodes = xDoc.SelectNodes("SDSLabels/Label/CASRN");

            foreach (XmlNode node in nodes)
            {
                if (node.InnerText == l.CASRN)
                {
                    return false;
                }
            }

            return true;
        }

        private void createData(SDSLabel l)
        {
            string[] dataArray = l.GetPropertiesArray();

            xWriter = new XmlTextWriter(path, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.WriteStartElement("SDSLabels");
            xWriter.WriteStartElement("Label");

            //Write the label data in XML format.
            for (int i = 0; i < dataArray.Length; i++)
            {
                xWriter.WriteStartElement(nodeNames[i]);
                xWriter.WriteString(dataArray[i]);
                xWriter.WriteEndElement();
            }
 
            xWriter.WriteEndElement();//End Label Node
            xWriter.WriteEndElement();//End SDSLabels Node
            xWriter.Close();
            MessageBox.Show("Label data saved.");
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
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            foreach (XmlNode n in xDoc.SelectNodes("SDSLabels/Label"))
            {
                if (n.SelectSingleNode(nodeNames[0]).InnerText.ToLower() == l.ChemicalName.ToLower())
                {
                    for (int i = 0; i < l.Properties.Count - 1; i++)
                    {
                        n.SelectSingleNode(nodeNames[i]).InnerText = l.Properties.ElementAt(i);
                    }
                }
            }

            xDoc.Save(path);
            MessageBox.Show("Saving Changes.", "Saving");
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
