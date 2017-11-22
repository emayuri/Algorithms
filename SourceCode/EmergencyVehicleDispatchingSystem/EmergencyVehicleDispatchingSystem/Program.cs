using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data;
using System.Xml.Linq;

namespace EmergencyVehicleDispatchingSystem
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine((int)EmergencyVehicles.Ambulance);
            VehicleDetails veh = new VehicleDetails();
            veh.FetchVehicleData();
            //Dictionary<string, List<int>> vehicleByType = veh.FetchVehicleByType("Ambulance");
            //veh.VehicleSelected("Ambulance", "66201", 1);

            //ZipNodes zipNodes = new ZipNodes();
            //zipNodes.FetchNodes();
            Console.ReadLine();
        }

        static void XmlReadData()
        {
            XmlDataDocument xmlDoc = new XmlDataDocument();
            XmlNodeList xmlNodeList;
            
            FileStream fs = new FileStream(@"..\\..\\VehicleDetails.xml", FileMode.Open, FileAccess.Read);
            xmlDoc.Load(fs);
            xmlNodeList = xmlDoc.GetElementsByTagName("Vehicle");
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                xmlNodeList[i].ChildNodes.Item(0).InnerText.Trim();
                int id = int.Parse(xmlNodeList[i].ChildNodes.Item(0).InnerText.Trim());
                int type = int.Parse(xmlNodeList[i].ChildNodes.Item(1).InnerText.Trim());
                string zipCode = xmlNodeList[i].ChildNodes.Item(2).InnerText.Trim().ToString();

                //veh = new Vehicle(type, zipCode, id);
            }

            fs.Close();
        }

        static void XmlWriteData()
        {
            XmlDocument xmlEmloyeeDoc = new XmlDocument();
            xmlEmloyeeDoc.Load(@"..\\..\\VehicleDetails.xml");
            XmlElement ParentElement = xmlEmloyeeDoc.CreateElement("Vehicle");
            XmlElement ID = xmlEmloyeeDoc.CreateElement("ID");
            ID.InnerText = "8";
            XmlElement Type = xmlEmloyeeDoc.CreateElement("Type");
            Type.InnerText = "1";
            XmlElement ZipCode = xmlEmloyeeDoc.CreateElement("ZipCode");
            ZipCode.InnerText = "66202";

            ParentElement.AppendChild(ID);
            ParentElement.AppendChild(Type);
            ParentElement.AppendChild(ZipCode);
            xmlEmloyeeDoc.DocumentElement.AppendChild(ParentElement);
            xmlEmloyeeDoc.Save(@"..\\..\\VehicleDetails.xml");
        }

        static void XmlDeleteRecord(string data)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(@"..\\..\\VehicleDetails.xml");

            XmlNode node = xmlDocument.SelectSingleNode(@"VehicleDetails/Vehicle[Id ='"+ data +"']");

            if (node != null)
            {

                node.ParentNode.RemoveChild(node);

                xmlDocument.Save(@"..\\..\\VehicleDetails.xml");

            }
        }

        static void XmlUpdate(string id, string zipCode)
        {

            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(@"..\\..\\VehicleDetails.xml");

            XmlNode node = xmlDocument.SelectSingleNode(@"VehicleDetails/Vehicle[Id ='" + id + "']");

            if(node != null)
            {
                node.LastChild.InnerText = zipCode;
            }

            xmlDocument.Save(@"..\\..\\VehicleDetails.xml");
        }
    }
}
