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
            var db = new NanoDB.DB("DefaultEndpointsProtocol=https;AccountName=nanostoredb;AccountKey=UFHN+sdVpT8v4RXGtJkH/GVq3u2kEvNZePAipGoFiYZoD/eBE/zB6iCbkWl6ny2VOnFPUVMF9QNLGCpBpOOeMA==;");
            var data = db.GetRawData();
            MainChart.Series[0].Points.DataBind(data, "DateTime", "X", "");
            MainChart.Series[1].Points.DataBind(data, "DateTime", "Y", "");
        }
    }
}