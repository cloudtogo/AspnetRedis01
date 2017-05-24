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
        public string ExeCommand(string commandText)
        {
            Process p = new Process();
            p.StartInfo.WorkingDirectory = @"c:\";
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + commandText;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                strOutput = p.StandardOutput.ReadToEnd();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
        
        public string GetEnv(string envName)
        {
            string cmdout = ExeCommand("set " + envName).Trim();
            if (String.IsNullOrEmpty(cmdout) || !cmdout.Contains('='))
                return null;
            return cmdout.Split('=')[1].Split('\n')[0].Trim();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string host = GetEnv("REDIS_HOST");
//             string host = System.Environment.GetEnvironmentVariable("REDIS_HOST", System.EnvironmentVariableTarget.Machine);
//             if (String.IsNullOrEmpty(host))
//             {
//                 this.Page.Response.Write("REDIS_HOST is not set.");
//                 return;
//             }

            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(host + ":6379");
            IDatabase db = conn.GetDatabase();
            
            string uuid = Guid.NewGuid().ToString();
            db.StringAppend("uuids", uuid + "|");
            string uuids = db.StringGet("uuids");
            
            this.Page.Response.Write("host = " + host + " ,uuids = " + uuids);
        }
    }
}
