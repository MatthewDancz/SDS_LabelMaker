﻿using System;
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

        public List<string> SearchDataBase(string s)
        {
            return BL.SearchDataBase(s);
        }

        public SDSLabel getLabelData(string s)
        {
            return BL.getLabelData(s);
        }
    }

    class BusinessLogic
    {
        SDSLabel newLabel = null;
        DAL newDal = new DAL();

        public void UpdateLabel(string name, string id, string signal, string hazards, string manufacturer)
        {
            newLabel = new SDSLabel();
            newLabel.ProductName = name;
            newLabel.CASRN = id;
            newLabel.SignalWord = signal;
            newLabel.HazardStatement = hazards;
            //for (int i = 0; i < hazards.Length; i++)
            //{
            //    newLabel.HazardStatements.Add(hazards[i]);
            //}

            newLabel.ProductManufacturer = manufacturer;
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
            newLabel.ProductName = s;
        }

        public List<string> SearchDataBase(string s)
        {
            return newDal.SearchDataBase(s);
        }

        public SDSLabel getLabelData(string s)
        {
            return newDal.getLabelData(s);
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

            if (!ToAddName)
            {
                DialogResult result = DialogResult.No;
                result = MessageBox.Show("This product has already been saved, would you like to update the data?", "Update Data", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    updateData(l);
                }
            }
            else
            {
                addSDSLabel(l);
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
                if (node.InnerText == l.ProductName)
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
                if (n.SelectSingleNode(nodeNames[0]).InnerText.ToLower() == l.ProductName.ToLower())
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

        public List<string> SearchDataBase(string s)
        {
            List<string> stringList = null;
            // SDSLabels
            //Label
            //nodeNames
            if (File.Exists(path))
            {
                stringList = new List<string>();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);
                foreach (XmlNode node in xDoc.SelectNodes("SDSLabels/Label"))
                {
                    if (node.InnerText.ToLower().Contains(s.ToLower()))
                    {
                        stringList.Add(node.SelectSingleNode(nodeNames[0]).InnerText);
                    }
                }
            }
            return stringList;
        }

        public SDSLabel getLabelData(string s)
        {
            SDSLabel retrievedLabel = new SDSLabel();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            foreach (XmlNode node in xDoc.SelectNodes("SDSLabels/Label"))
            {
                if (node.InnerText.ToLower().Contains(s))
                {
                    retrievedLabel.ProductName = node.SelectSingleNode(nodeNames[0]).InnerText;
                    retrievedLabel.CASRN = node.SelectSingleNode(nodeNames[1]).InnerText;
                    retrievedLabel.SignalWord = node.SelectSingleNode(nodeNames[2]).InnerText;
                    retrievedLabel.HazardStatement = node.SelectSingleNode(nodeNames[3]).InnerText;
                    retrievedLabel.ProductManufacturer = node.SelectSingleNode(nodeNames[4]).InnerText;
                    retrievedLabel.populateProperties();
                }
            }

            return retrievedLabel;
        }

        public void DeleteData()
        {

        }
    }

    class SDSLabel
    {
        public List<string> Properties = new List<string>();

        public string ProductName = null;
        public string CASRN = null;
        public string SignalWord = null;
        public string HazardStatement = null;
        public List<string> HazardStatements = new List<string>();

        //Some way to store pictures.

        public string ProductManufacturer = null;

        public void populateProperties()
        {
            Properties.Add(ProductName);
            Properties.Add(CASRN);
            Properties.Add(SignalWord);
            Properties.Add(HazardStatement);

            //foreach (string s in HazardStatements)
            //{
            //    Properties.Add(s);
            //}

            Properties.Add(ProductManufacturer);
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
