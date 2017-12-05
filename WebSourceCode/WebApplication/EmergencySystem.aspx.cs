using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class EmergencySystem : System.Web.UI.Page
    {
        public static List<Request> requests;
        //public static Dictionary<int, DateTime> track = new Dictionary<int, DateTime>();
        public Request request;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack && Session["EmergencySystem"] == null)
            {
                Session["Check_Page_Refresh"] = DateTime.Now.ToString();
                requests = new List<Request>();
                BindGridViewData();
                Session["EmergencySystem"] = "EmergencySystem";

                System.Threading.Thread deleteThread = new System.Threading.Thread(TrackRequest);
                deleteThread.Start();
            }
            else if(!IsPostBack && Session["EmergencySystem"] != null)
            {
                Session["state"] = requests;
                GridView1.DataSource = requests;
                GridView1.DataBind();
            }
        }

        /// <summary>
        /// Function to track for how long a vehicle takes for servicing a request
        /// </summary>
        void TrackRequest()
        {
            //This will take 30 minutes
            for (int i = 0; i < 360; i++)
            {
                if (requests.Count != 0 && requests[requests.Count - 1] != null)
                {
                    for(int j = 0; j< requests.Count; j++)
                    {
                        TimeSpan t = (DateTime.Now - requests[j].requestTime);
                        if ( t.Minutes >= 2)
                        {
                            Delete(delete, EventArgs.Empty);
                            //track.Remove(requestId);
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine("Task " + i + " - " + DateTime.Now);
                System.Threading.Thread.Sleep(5000);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["Check_Page_Refresh"] = Session["Check_Page_Refresh"];
        }

        /// <summary>
        /// Function to bind data to grid view
        /// </summary>
        private void BindGridViewData()
        {
            request = null;
            requests.Add(null);

            Session["state"] = requests;
            GridView1.DataSource = requests;
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
            }
        }

        /// <summary>
        /// Function to delete a request once the processing time is crossed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Delete(object sender, EventArgs e)
        {
            VehicleDetails.Instance.VehicleUnSelected(requests[0].vehicleType, requests[0].zipCode, requests[0].vehicleId);
            requests.RemoveAt(0);
            
            if (requests.Count == 0)
            {
                request = null;
                requests.Add(null);
            }
            else
            {
                for (int i = 0; i < requests.Count; i++)
                {
                    requests[i].requestId = i + 1;
                }
            }
            Session["state"] = requests;
            Session["Check_Page_Refresh"] = DateTime.Now.ToString();
            GridView1.DataSource = requests;
            GridView1.DataBind();
            ViewState["Check_Page_Refresh"] = Session["Check_Page_Refresh"];
        }

        /// <summary>
        /// Insert a new request into list of requests maintained
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Insert(object sender, EventArgs e)
        {
            if (ViewState["Check_Page_Refresh"].ToString() == Session["Check_Page_Refresh"].ToString())
            {
                int requestId = 0;
                requests = new List<Request>();
                requests = (List<Request>)Session["state"];

                int vehicleType = ((DropDownList)GridView1.FooterRow.FindControl("ddlVehicleType")).SelectedIndex;
                string zipCode = ((TextBox)GridView1.FooterRow.FindControl("txtZipCode")).Text;
                if (vehicleType != 0 && !string.IsNullOrEmpty(zipCode))
                {
                    if (requests[requests.Count - 1] != null)
                    {
                        requestId = requests.Count() + 1;
                    }
                    else
                    {
                        requests = new List<Request>();
                        requestId = 1;
                    }
                    request = new Request(requestId, (EmergencyVehicles)vehicleType, zipCode);
                    if (request.Gap < int.MaxValue)
                    {
                        request.requestTime = DateTime.Now;
                        requests.Add(request);
                        //track.Add(request.requestId, DateTime.Now);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sorry! All the vehicles are currently attending other emergencies.')", true);
                    }
                    
                }
                Session["state"] = requests;
                Session["Check_Page_Refresh"] = DateTime.Now.ToString();
                GridView1.DataSource = requests;
                GridView1.DataBind();
                
            }
            else
            {
                requests = (List<Request>)Session["state"];
                GridView1.DataSource = requests;
                GridView1.DataBind();
                Response.Write("Page Refresh Detected....");
            }
        }
    }
}