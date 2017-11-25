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
            //populating vehicle details
            VehicleDetails.Instance.FetchVehicleData();
            //populating distance details
            Distance.Instance.FetchDistanceGraph();

            Dictionary<EmergencyVehicles, string> requests = new Dictionary<EmergencyVehicles, string>();
            Request req = new Request(1, EmergencyVehicles.Ambulance, "64122");
            req.ProcessRequest();

            Console.WriteLine("Request Id:" + req.requestId + " Vehicle type:" + req.vehicleType + " Destination zipcode:" + req.zipCode + " Vehicle Id:"
                        + req.vehicleId + " Distance:" + req.gap);


            //req = new Request(1, EmergencyVehicles.Ambulance, "66201");
            //req.ProcessRequest();

            //Console.WriteLine("Request Id:" + req.requestId + " Vehicle type:" + req.vehicleType + " Destination zipcode:" + req.zipCode + " Vehicle Id:"
            //            + req.vehicleId + " Distance:" + req.gap);

            //req = new Request(1, EmergencyVehicles.Ambulance, "66201");
            //req.ProcessRequest();

            //Console.WriteLine("Request Id:" + req.requestId + " Vehicle type:" + req.vehicleType + " Destination zipcode:" + req.zipCode + " Vehicle Id:"
            //            + req.vehicleId + " Distance:" + req.gap);

            Console.ReadLine();
            //string userInput = "";
            //Console.WriteLine("Hello!");
            //Console.WriteLine("Do you want to go ahead and request for a vehicle.(YES/NO)");
            //while (userInput != "EXIT")
            //{
            //    if (Console.ReadLine().ToUpper() == "YES")
            //    {
            //        Console.WriteLine("Please enter you request here!");
            //        Console.Write("VehicleType");
            //        int vehId = int.Parse(Console.ReadLine());
            //        EmergencyVehicles vehiclId = (EmergencyVehicles)vehId;

            //        Console.WriteLine("Please enter the destination zip code");
            //        string zipCode = Console.ReadLine().ToString();

            //        Request req = new Request(1, vehiclId, zipCode);

            //        req.ProcessRequest();

            //        Console.WriteLine("Request Id:" + req.requestId + " Vehicle type:" + req.vehicleType + " Destination zipcode:" + req.zipCode + " Vehicle Id:"
            //            + req.vehicleId + " Distance:" + req.gap);
            //    }
            //    else if (Console.ReadLine().ToUpper() == "NO")
            //    {
            //        Console.WriteLine("Thanks you!");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Please enter valid data!");
            //        userInput = Console.ReadLine();
            //    }
            //}
        }

        static void XmlReadData()
        {
            XmlDocument xmlDoc = new XmlDocument();
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
