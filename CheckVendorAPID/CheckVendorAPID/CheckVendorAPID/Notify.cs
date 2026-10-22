using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace CheckVendorAPID
{
	internal class Notify
	{
		private static readonly string ConnString = ConfigurationManager.ConnectionStrings["ElfaConn"].ToString();

		private readonly SqlConnection _objConn = new SqlConnection(ConnString);

		public SqlDataReader GetVendors()
		{
			SqlCommand objComm = new SqlCommand();
			objComm.Connection = _objConn;
			objComm.CommandType = CommandType.StoredProcedure;
			objComm.CommandText = "pCheckVendorAPID";
			_objConn.Open();
			return objComm.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public void SendEmail()
		{
			string emailList = ConfigurationManager.AppSettings["Emails"];
			string mailSeverName = ConfigurationManager.AppSettings["MailServer"];
			string to = emailList;
			string body = "Some vendors set for payment do not have AP ID<br><br>";
			body += "Order # Vendor Name<br>";
			SqlDataReader drInfo = GetVendors();
			if (drInfo.HasRows)
			{
				while (drInfo.Read())
				{
					body = body + drInfo[2].ToString() + " " + drInfo[5].ToString() + "<br>";
				}
				drInfo.Close();
				body += "<br><br>This is automated e-mail. Do not reply.";
				try
				{
					MailMessage message = new MailMessage("notification@elfainstall.com", to, "Missing an AP Vendor ID", body);
					message.IsBodyHtml = true;
					SmtpClient mailClient = new SmtpClient(mailSeverName);
					mailClient.UseDefaultCredentials = true;
					mailClient.Send(message);
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			drInfo.Close();
		}
	}
}
