using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyVehicleDispatchingSystem
{
    /*Class to hold vehicle details*/
    public class Vehicle
    {
        public int id;
        public EmergencyVehicles type;
        public string zipCode;
        public bool status;

        /// <summary>
        /// Setter and getter for vehicleId
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
        /// Setter and getter for vehicle type
        /// </summary>
        public EmergencyVehicles Type
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
        /// setter and getter for zipcode
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
        /// setter and getter for vehicle status
        /// </summary>
        private bool Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        
        /// <summary>
        /// Class parameterized construtor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="zipCode"></param>
        /// <param name="status"></param>
        public Vehicle(int id, EmergencyVehicles type, string zipCode, bool status)
        {
            this.id = id;
            this.type = type;
            this.zipCode = zipCode;
            this.status = status;
        }
    }
}
