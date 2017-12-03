using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;

namespace WebApplication
{
    public sealed class VehicleDetails
    {
        public int type;
        public int id;
        public string zipCode;
        Vehicle vehicle;

        //Hash map to store vehicleType, zipCode and vehicleId's 
        public Dictionary<EmergencyVehicles, Dictionary<string, List<int>>> vehicleMap;

        //Hash map to store vehicle status(i.e. if the vehicle is already assigned to a request or not).
        public Dictionary<int, Vehicle> vehicleStatus;

        /// <summary>
        /// Class constructor
        /// </summary>
        private VehicleDetails()
        {
            vehicleMap = new Dictionary<EmergencyVehicles, Dictionary<string, List<int>>>();
            vehicleStatus = new Dictionary<int, Vehicle>();
        }

        private static VehicleDetails instance = null;

        /// <summary>
        /// Singleton class instance
        /// </summary>
        public static VehicleDetails Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VehicleDetails();
                }
                return instance;
            }
        }



        /// <summary>
        /// Getter and Setter for Vehicle type
        /// </summary>
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Zip code
        /// </summary>
        public string ZipCode
        {
            get
            {
                return zipCode;
            }
            set
            {
                zipCode = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Vehicle id
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Function to fetch Vehicle Data from VehicleDetails.xml document
        /// </summary>
        public Dictionary<EmergencyVehicles, Dictionary<string, List<int>>> FetchVehicleData()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodeList;
            string path = AppDomain.CurrentDomain.BaseDirectory;

            FileStream fs = new FileStream(path+ @"\VehicleDetails.xml", FileMode.Open, FileAccess.Read);
            xmlDoc.Load(fs);
            xmlNodeList = xmlDoc.GetElementsByTagName("Vehicle");
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                xmlNodeList[i].ChildNodes.Item(0).InnerText.Trim();
                int id = int.Parse(xmlNodeList[i].ChildNodes.Item(0).InnerText.Trim());
                EmergencyVehicles type = (EmergencyVehicles)int.Parse(xmlNodeList[i].ChildNodes.Item(1).InnerText.Trim());
                string zipCode = xmlNodeList[i].ChildNodes.Item(2).InnerText.Trim().ToString();
                PopulateVehicleDetails(type, zipCode, id);
            }

            fs.Close();
            return vehicleMap;
        }

        /// <summary>
        /// Function to populate vehicle details in the Hash Map for easy access.
        /// </summary>
        /// <param name="type">Vehicle Type</param>
        /// <param name="zipCode">Zip code at which vehicle is available</param>
        /// <param name="id">Vehicle id number</param>
        public void PopulateVehicleDetails(EmergencyVehicles type, string zipCode, int id)
        {
            if (type == EmergencyVehicles.Ambulance)
            {
                if (vehicleMap.ContainsKey(type))
                {
                    if (vehicleMap[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleMap[type][zipCode];
                        ids.Add(id);
                        vehicleMap[type][zipCode] = ids;
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                    else
                    {
                        vehicleMap[type].Add(zipCode, new List<int> { id });
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                }
                else
                {
                    vehicleMap[type] = new Dictionary<string, List<int>>();
                    vehicleMap[type].Add(zipCode, new List<int> { id });
                    vehicle = new Vehicle(id, type, zipCode, true);
                    vehicleStatus.Add(id, vehicle);
                }
            }
            else if (type == EmergencyVehicles.FireTruck)
            {
                if (vehicleMap.ContainsKey(type))
                {
                    if (vehicleMap[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleMap[type][zipCode];
                        ids.Add(id);
                        vehicleMap[type][zipCode] = ids;
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                    else
                    {
                        vehicleMap[type].Add(zipCode, new List<int> { id });
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                }
                else
                {
                    vehicleMap[type] = new Dictionary<string, List<int>>();
                    vehicleMap[type].Add(zipCode, new List<int> { id });
                    vehicle = new Vehicle(id, type, zipCode, true);
                    vehicleStatus.Add(id, vehicle);
                }
            }
            else
            {
                if (vehicleMap.ContainsKey(type))
                {
                    if (vehicleMap[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleMap[type][zipCode];
                        ids.Add(id);
                        vehicleMap[type][zipCode] = ids;
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                    else
                    {
                        vehicleMap[type].Add(zipCode, new List<int> { id });
                        vehicle = new Vehicle(id, type, zipCode, true);
                        vehicleStatus.Add(id, vehicle);
                    }
                }
                else
                {
                    vehicleMap[type] = new Dictionary<string, List<int>>();
                    vehicleMap[type].Add(zipCode, new List<int> { id });
                    vehicle = new Vehicle(id, type, zipCode, true);
                    vehicleStatus.Add(id, vehicle);
                }
            }

        }

        /// <summary>
        /// Function to fetch list of specific emergency vehicles available across.
        /// </summary>
        /// <param name="vehicleType">Type of emergency vehicle</param>
        /// <returns></returns>
        public Dictionary<string, List<int>> FetchVehicleByType(EmergencyVehicles vehicleType)
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            vehicleDetails = vehicleMap[vehicleType];
            return vehicleDetails;
        }

        /// <summary>
        /// Function to mark vehicle as selected
        /// </summary>
        /// <param name="vehicleType">Vehicle type</param>
        /// <param name="zipCode">Zip code at which vehicle is available</param>
        /// <param name="vehicleId">Vehicle id number</param>
        /// <returns>returns boolean if vehicles status is changed</returns>
        public bool VehicleSelected(EmergencyVehicles vehicleType, string zipCode, int vehicleId)
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            vehicleDetails = vehicleMap[vehicleType];
            List<int> vehicleIds = new List<int>();
            vehicleIds = vehicleDetails[zipCode];
            if (vehicleId != 0)
            {
                //vehicleStatus[vehicleId] = false;
                vehicle = new Vehicle(vehicleId, vehicleType, zipCode, false);
                vehicleStatus[vehicleId] = vehicle;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function to mark vehicle as selected
        /// </summary>
        /// <param name="vehicleType">Vehicle type</param>
        /// <param name="zipCode">Zip code at which vehicle is available</param>
        /// <param name="vehicleId">Vehicle id number</param>
        /// <returns>returns boolean if vehicles status is changed</returns>
        public bool VehicleUnSelected(EmergencyVehicles vehicleType, string zipCode, int vehicleId)
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            vehicleDetails = vehicleMap[vehicleType];
            List<int> vehicleIds = new List<int>();
            vehicleIds = vehicleDetails[zipCode];
            if (vehicleId != 0)
            {
                //vehicleStatus[vehicleId] = false;
                vehicle = new Vehicle(vehicleId, vehicleType, zipCode, true);
                vehicleStatus[vehicleId] = vehicle;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function to print vehicle details available in hash map
        /// </summary>
        public void PrintVehicleDetails()
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            foreach (EmergencyVehicles type in vehicleMap.Keys)
            {
                vehicleDetails = vehicleMap[type];
                foreach (string zip in vehicleDetails.Keys)
                {
                    List<int> vehicleIds = new List<int>();
                    vehicleIds = vehicleDetails[zip];

                    for (int k = 0; k < vehicleIds.Count; k++)
                    {
                        Console.WriteLine(vehicleIds[k]);
                    }
                }
            }
        }
    }
}