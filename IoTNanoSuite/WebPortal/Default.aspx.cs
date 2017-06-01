using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPortal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var db = new NanoDB.DB();
            var data = db.GetRawData();
            if (ddDispType.SelectedValue == "X,Y")
            {
                MainChart.Series[0].Points.DataBind(data, "DateTime", "X", "");
                MainChart.Series[1].Points.DataBind(data, "DateTime", "Y", "");
            }
            if (ddDispType.SelectedValue == "Temp")
            {
                MainChart.Series[0].Points.DataBind(data, "DateTime", "Temp", "");
            }
            if (ddDispType.SelectedValue == "Wind")
            {
                MainChart.Series[0].Points.DataBind(data, "DateTime", "WindX", "");
                MainChart.Series[1].Points.DataBind(data, "DateTime", "WindY", "");
            }
            if (ddDispType.SelectedValue == "Power")
            {
                MainChart.Series[0].Points.DataBind(data, "DateTime", "Power", "");
            }

        }
    }
}