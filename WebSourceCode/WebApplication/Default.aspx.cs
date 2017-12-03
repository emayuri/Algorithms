using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace WebApplication
{
    public partial class _Default : Page
    {
        public int user = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["default"] == null)
            {
                ////populating vehicle details
                //VehicleDetails.Instance.FetchVehicleData();
                ////populating distance details
                //Distance.Instance.FetchDistanceGraph();
                

                ThreadStart t1 = new ThreadStart(LoadVehicleData);

                ThreadStart t2 = new ThreadStart(LoadDistanceData);

                Thread child1 = new Thread(t1);

                child1.Start();

                Thread child2 = new Thread(t2);

                child2.Start();

                child1.Join();
                child2.Join();


                Session["default"] = "default";
            }

        }

        
        public void LoadVehicleData()
        {
            VehicleDetails.Instance.FetchVehicleData();
        }

        public void LoadDistanceData()
        {
            Distance.Instance.FetchDistanceGraph();
        }
    }
}