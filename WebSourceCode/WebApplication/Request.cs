using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public partial class Request
    {
        //Class variable
        Dictionary<string, List<ZipNodes>> graph;
        Dictionary<string, List<int>> vehicleByType;
        Dictionary<string, int> result = new Dictionary<string, int>();
        int vertices;
        string sourceZipCode = "";
        public int requestId;
        public EmergencyVehicles vehicleType;
        public string zipCode;
        public int vehicleId;
        public int gap;

        /// <summary>
        /// Getter and Setter for Request Id
        /// </summary>
        public int RequestId
        {
            get
            {
                return requestId;
            }
            set
            {
                requestId = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Vehicle type
        /// </summary>
        public EmergencyVehicles VehicleType
        {
            get
            {
                return vehicleType;
            }
            set
            {
                vehicleType = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Zip Code
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
        /// Getter and Setter for Vehicle Id
        /// </summary>
        public int VehicleId
        {
            get
            {
                return vehicleId;
            }
            set
            {
                vehicleId = value;
            }
        }

        /// <summary>
        /// Getter and Setter for Distance
        /// </summary>
        public int Gap
        {
            get
            {
                return gap;
            }
            set
            {
                gap = value;
            }
        }

        /// <summary>
        /// Parameterized class constructor which takes request parameters as inputs
        /// </summary>
        /// <param name="requestId">Unique request Id</param>
        /// <param name="vehicleType">Type of the vehicle requested</param>
        /// <param name="zipCode">Zip ocde from where the request was made</param>
        public Request(int requestId, EmergencyVehicles vehicleType, string zipCode)
        {
            try
            {

                this.requestId = requestId;
                this.vehicleType = vehicleType;
                this.zipCode = zipCode;
                gap = int.MaxValue;
                //Once the input is given to class, request would be processed by default
                ProcessRequest();
                //avoiding object creation and throwing exception when there are no vehicle to process.
                if(vehicleId == 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
            }

        }

        /// <summary>
        /// Function to process the request made and pass the control to shortes path function if required
        /// </summary>
        private void ProcessRequest()
        {
            vehicleByType = VehicleDetails.Instance.FetchVehicleByType(vehicleType);
            graph = Distance.Instance.graph;

            //Checking if the requested vehicle is available in the same zipcode
            if (vehicleByType.ContainsKey(zipCode)) //change req
            {
                //Iterating through all the vehicles available in the same zipcode
                for (int i = 0; i < vehicleByType[zipCode].Count(); i++)
                {
                    if (VehicleDetails.Instance.vehicleStatus[vehicleByType[zipCode][i]].status == true)
                    {
                        result[zipCode] = 0;
                    }
                }
            }

            //If the vehicle is not available in the same zipcode going ahe
            if(result.Count()==0)
            {
                foreach (string source in vehicleByType.Keys)
                {
                    for (int i = 0; i < vehicleByType[source].Count(); i++)
                    {
                        if (VehicleDetails.Instance.vehicleStatus[vehicleByType[source][i]].status == true)
                        {
                            result[source] = int.MaxValue;
                            result[source] = ShortestPath(source, zipCode);
                        }
                    }
                }
            }
            foreach (string zip in result.Keys)
            {
                if (gap > result[zip])
                {
                    this.gap = result[zip];
                    sourceZipCode = zip;
                    for (int i = 0; i < vehicleByType[zip].Count(); i++)
                    {
                        if (VehicleDetails.Instance.vehicleStatus[vehicleByType[zip][i]].status == true)
                        {
                            this.vehicleId = vehicleByType[zip][i];
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sourceZipCode))
            {
                VehicleDetails.Instance.VehicleSelected(vehicleType, sourceZipCode, vehicleId);
            }

            else
            {
                //Console.WriteLine("No vehicles left to process!");
            }
        }

        /// <summary>
        /// Function that fetches the shortest path between the source and destination
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private int ShortestPath(string source, string destination)
        {
            int shortestDistance = int.MaxValue;
            vertices = graph.Count;

            ZipNodes zipCodeObj;

            //To store finalised distances
            Dictionary<string, int> distance = new Dictionary<string, int>();

            //Sorted dictionary used in the form of a min heap
            SortedSet<ZipNodes> sortedSet = new SortedSet<ZipNodes>(new CustomComparer());

            Dictionary<string, ZipNodes> trackingHashMap = new Dictionary<string, ZipNodes>();
            
            zipCodeObj = new ZipNodes(source, 0);

            trackingHashMap.Add(source, zipCodeObj);
            sortedSet.Add(zipCodeObj);

            while (distance.Count <= vertices)
            {
                string currentZip = sortedSet.ElementAt(0).zipCode;
                int currentDistance = sortedSet.ElementAt(0).distance;
                distance.Add(currentZip, currentDistance);

                if (currentZip == destination)
                {
                    if (result.ContainsKey(source) && result[source] > distance[destination])
                    {
                        //result[source] = distance[destination];
                        return distance[destination];
                    }
                }

                sortedSet.Remove(trackingHashMap[currentZip]);

                for (int i = 0; i < graph[currentZip].Count; i++)
                {
                    if (!distance.ContainsKey(graph[currentZip][i].zipCode))
                    {
                        if (!trackingHashMap.ContainsKey(graph[currentZip][i].zipCode))
                        {
                            ZipNodes newZipCodeObj = new ZipNodes(graph[currentZip][i].zipCode, graph[currentZip][i].distance + currentDistance);
                            sortedSet.Add(newZipCodeObj);
                            trackingHashMap.Add(graph[currentZip][i].zipCode, newZipCodeObj);
                        }
                        else
                        {
                            if (trackingHashMap[graph[currentZip][i].zipCode].distance > graph[currentZip][i].distance + currentDistance)
                            {
                                sortedSet.Remove(trackingHashMap[graph[currentZip][i].zipCode]);
                                trackingHashMap[graph[currentZip][i].zipCode].distance = graph[currentZip][i].distance + currentDistance;
                                sortedSet.Add(trackingHashMap[graph[currentZip][i].zipCode]);
                            }
                        }
                    }
                }
            }
            return shortestDistance;
        }

        public class CustomComparer : IComparer<ZipNodes>
        {

            public int Compare(ZipNodes x, ZipNodes y)
            {
                if ((x.distance - y.distance) == 0)
                {
                    return x.zipCode.CompareTo(y.zipCode);
                }
                return x.distance - y.distance;
            }
        }

        //Shortest Path problem with complexity as square of v
        /*
        public void ShortestPath(string source, string destination)
        {
            result[source] = int.MaxValue;

            vertices = graph.Count;
            Dictionary<string, int> distance = new Dictionary<string, int>();
            Dictionary<string, bool> sptSet = new Dictionary<string, bool>();

            foreach (string zip in graph.Keys)
            {
                distance.Add(zip, int.MaxValue);
                sptSet.Add(zip, false);
            }
            distance[source] = 0;

            // Pick the minimum distance vertex from the set of vertices
            // not yet processed. u is always equal to src in first
            // iteration.
            foreach (string zip in graph.Keys)
            {
                string u = minDistance(distance, sptSet);
                sptSet[u] = true;


                // Update dist value of the adjacent vertices of the
                // picked vertex.
                foreach (string zip1 in graph.Keys)
                {
                    // Update dist[v] only if is not in sptSet, there is an
                    // edge from u to v, and total weight of path from src to
                    // v through u is smaller than current value of dist[v]
                    if (graph[u].FirstOrDefault(o => o.zipCode == zip1) != null)
                    {
                        if (!sptSet[zip1] && graph[u].FirstOrDefault(o => o.zipCode == zip1).distance != 0
                            && distance[u] != int.MaxValue &&
                                distance[u] + graph[u].FirstOrDefault(o => o.zipCode == zip1).distance < distance[zip1])
                        {
                            distance[zip1] = distance[u] + graph[u].FirstOrDefault(o => o.zipCode == zip1).distance;
                        }
                    }
                }
                if (u == destination)
                {
                    if (result.ContainsKey(source) && result[source] > distance[destination])
                    {
                        result[source] = distance[destination];
                    }
                    return;

                }
            }
        }

        public string minDistance(Dictionary<string, int> distance, Dictionary<string, bool> sptSet)
        {
            // Initialize min value
            int min = int.MaxValue;
            string min_index = "";

            foreach (string zip in graph.Keys)
            {
                if (sptSet[zip] == false && distance[zip] <= min)
                {
                    min = distance[zip];
                    min_index = zip;
                }
            }

            return min_index;
        }
        */
    }
}