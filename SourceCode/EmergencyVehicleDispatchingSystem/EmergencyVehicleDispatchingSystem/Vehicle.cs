using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace EmergencyVehicleDispatchingSystem
{
    class VehicleDetails
    {
        public int type;
        public int id;
        public string zipCode;

        //Hash map to store vehicleType, zipCode and vehicleId's 
        public static Dictionary<EmergencyVehicles, Dictionary<string, List<int>>> vehicleDict;

        //Hash map to store vehicle status(i.e. if the vehicle is already assigned to a request or not). bool -- vehicle object
        public static Dictionary<int, bool> vehicleStatus;

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
        /// Default Constructor
        /// </summary>
        public VehicleDetails()
        {
            vehicleDict = new Dictionary<EmergencyVehicles, Dictionary<string, List<int>>>();
            vehicleStatus = new Dictionary<int, bool>();
        }

        /// <summary>
        /// Function to fetch Vehicle Data from VehicleDetails.xml document
        /// </summary>
        public void FetchVehicleData()
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
                EmergencyVehicles type = (EmergencyVehicles)int.Parse(xmlNodeList[i].ChildNodes.Item(1).InnerText.Trim());//int.Parse(xmlNodeList[i].ChildNodes.Item(1).InnerText.Trim());
                string zipCode = xmlNodeList[i].ChildNodes.Item(2).InnerText.Trim().ToString();
                PopulateVehicleDetails(type, zipCode, id);
            }

            fs.Close();
        }

        /// <summary>
        /// Function to populate vehicle details in the Hash Map for easy access.
        /// </summary>
        /// <param name="type">Vehicle Type</param>
        /// <param name="zipCode">Zip code at which vehicle is available</param>
        /// <param name="id">Vehicle id number</param>
        private void PopulateVehicleDetails(EmergencyVehicles type, string zipCode, int id)
        {
            if (type == EmergencyVehicles.Ambulance)
            {
                if (vehicleDict.ContainsKey(type))
                {
                    if (vehicleDict[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleDict[type][zipCode];
                        ids.Add(id);
                        vehicleDict[type][zipCode] = ids;
                        vehicleStatus.Add(id, false);
                    }
                    else
                    {
                        vehicleDict[type].Add(zipCode, new List<int> { id });
                        vehicleStatus.Add(id, false);
                    }
                }
                else
                {
                    vehicleDict[type] = new Dictionary<string, List<int>>();
                    vehicleDict[type].Add(zipCode, new List<int> { id });
                    vehicleStatus.Add(id, false);
                }
            }
            else if (type == EmergencyVehicles.FireTruck)
            {
                if (vehicleDict.ContainsKey(type))
                {
                    if (vehicleDict[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleDict[type][zipCode];
                        ids.Add(id);
                        vehicleDict[type][zipCode] = ids;
                        vehicleStatus.Add(id, false);
                    }
                    else
                    {
                        vehicleDict[type].Add(zipCode, new List<int> { id });
                        vehicleStatus.Add(id, false);
                    }
                }
                else
                {
                    vehicleDict[type] = new Dictionary<string, List<int>>();
                    vehicleDict[type].Add(zipCode, new List<int> { id });
                    vehicleStatus.Add(id, false);
                }
            }
            else
            {
                if (vehicleDict.ContainsKey(type))
                {
                    if (vehicleDict[type].ContainsKey(zipCode))
                    {
                        List<int> ids = vehicleDict[type][zipCode];
                        ids.Add(id);
                        vehicleDict[type][zipCode] = ids;
                        vehicleStatus.Add(id, false);
                    }
                    else
                    {
                        vehicleDict[type].Add(zipCode, new List<int> { id });
                        vehicleStatus.Add(id, false);
                    }
                }
                else
                {
                    vehicleDict[type] = new Dictionary<string, List<int>>();
                    vehicleDict[type].Add(zipCode, new List<int> { id });
                    vehicleStatus.Add(id, false);
                }
            }

        }

        /// <summary>
        /// Function to fetch list of specific emergency vehicles available across.
        /// </summary>
        /// <param name="vehicleType">Type of emergency vehicle</param>
        /// <returns></returns>
        public Dictionary<string,List<int>> FetchVehicleByType(EmergencyVehicles vehicleType)
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            vehicleDetails = vehicleDict[vehicleType];
            return vehicleDetails;
        }

        /// <summary>
        /// Function to mark vehicle as selected
        /// </summary>
        /// <param name="vehicleType">Vehicle type</param>
        /// <param name="zipCode">Zip code at which vehicle is available</param>
        /// <param name="vehicleId">Vehicle id number</param>
        /// <returns>returns boolean if vehicles status is changed</returns>
        public bool VehicleSelected(EmergencyVehicles vehicleType, string zipCode, int vehicleId) //enum
        {
            Dictionary<string, List<int>> vehicleDetails = new Dictionary<string, List<int>>();
            vehicleDetails = vehicleDict[vehicleType];
            List<int> vehicleIds = new List<int>();
            vehicleIds = vehicleDetails[zipCode];
            if(vehicleIds.Remove(vehicleId))
            {
                vehicleStatus[vehicleId] = true;
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
            foreach (EmergencyVehicles type in vehicleDict.Keys)
            {
                vehicleDetails = vehicleDict[type];
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
