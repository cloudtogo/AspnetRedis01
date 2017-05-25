using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StackExchange.Redis;

namespace AspnetRedis
{
    public partial class WebForm1 : System.Web.UI.Page
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            string host = System.Environment.GetEnvironmentVariable("REDIS_HOST");
            if (String.IsNullOrEmpty(host))
            {
                this.Page.Response.Write("REDIS_HOST is not set.");
                return;
            }

            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(host + ":6379");
            IDatabase db = conn.GetDatabase();
            
            string uuid = Guid.NewGuid().ToString();
            db.StringAppend("uuids", uuid + "|");
            string uuids = db.StringGet("uuids");
            
            this.Page.Response.Write("host = " + host + " ,uuids = " + uuids);
        }
    }
}
