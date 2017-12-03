using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using System.Threading;



namespace WebApplication
{
    public partial class EmergencySystem : System.Web.UI.Page
    {
        public static List<Request> list;
        public Request req;
        Thread child1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack && Session["EmergencySystem"] == null)
            {
                Session["Check_Page_Refresh"] = DateTime.Now.ToString();
                list = new List<Request>();
                BindGridViewData();
                Session["EmergencySystem"] = "EmergencySystem";

                //ThreadStart t1 = new ThreadStart(Delete);
                //child1 = new Thread(t1);

                //child1.Start();
            }
            else if(!IsPostBack && Session["EmergencySystem"] != null)
            {
                Session["state"] = list;
                GridView1.DataSource = list;
                GridView1.DataBind();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["Check_Page_Refresh"] = Session["Check_Page_Refresh"];
        }

        private void BindGridViewData()
        {
            req = null;
            list.Add(null);

            Session["state"] = list;

            GridView1.DataSource = list;
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

        public void Delete()
        {
            Thread.Sleep(10000);
            VehicleDetails.Instance.VehicleUnSelected(list[0].vehicleType, list[0].zipCode, list[0].vehicleId);
            list.RemoveAt(0);
            if (list.Count == 0)
            {
                req = null;
                list.Add(null);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].requestId = i + 1;
                }
            }
            Session["state"] = list;
            Session["Check_Page_Refresh"] = DateTime.Now.ToString();
            GridView1.DataSource = list;
            GridView1.DataBind();
        }
        public void Insert(object sender, EventArgs e)
        {
            if (ViewState["Check_Page_Refresh"].ToString() == Session["Check_Page_Refresh"].ToString())
            {
                int requestId = 0;
                list = new List<Request>();
                list = (List<Request>)Session["state"];

                int vehicleType = ((DropDownList)GridView1.FooterRow.FindControl("ddlVehicleType")).SelectedIndex;
                string zipCode = ((TextBox)GridView1.FooterRow.FindControl("txtZipCode")).Text;
                if (vehicleType != 0 && !string.IsNullOrEmpty(zipCode))
                {
                    if (list[list.Count - 1] != null)
                    {
                        requestId = list.Count() + 1;
                    }
                    else
                    {
                        list = new List<Request>();
                        requestId = 1;
                    }
                    req = new Request(requestId, (EmergencyVehicles)vehicleType, zipCode);
                    if (req.Gap < int.MaxValue)
                    {
                        list.Add(req);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sorry! All the vehicles are currently attending other emergencies.')", true);
                    }
                    
                }

                Session["state"] = list;

                Session["Check_Page_Refresh"] = DateTime.Now.ToString();

                GridView1.DataSource = list;
                GridView1.DataBind();
                
            }
            else
            {
                list = (List<Request>)Session["state"];
                GridView1.DataSource = list;
                GridView1.DataBind();
                Response.Write("Page Refresh Detected....");
            }
        }
    }
    
}