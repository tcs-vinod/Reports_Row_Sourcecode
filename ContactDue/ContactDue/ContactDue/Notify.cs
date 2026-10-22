using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace ContactDue
{
	internal class Notify
	{
		private static readonly string ConnString = ConfigurationManager.ConnectionStrings["ElfaConn"].ToString();

		private readonly SqlConnection _objConn = new SqlConnection(ConnString);

		private SqlDataReader GetOrders()
		{
			SqlCommand objComm = new SqlCommand();
			objComm.Connection = _objConn;
			objComm.CommandType = CommandType.StoredProcedure;
			objComm.CommandText = "sp_OrdersDue";
			_objConn.Open();
			return objComm.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public void SendEmail()
		{
			string emailList = ConfigurationManager.AppSettings["Emails"];
			string mailSeverName = ConfigurationManager.AppSettings["MailServer"];
			string to = emailList;
			string tableBody = "";
			string body = "<table border='1' width='500' id='table1'>";
			body += "<tr><td width='70'>Order #</td><td width='70'>Date</td>";
			body += "<td width='180'>Vendor</td><td width='180'>Customer</td></tr>";
			SqlDataReader drInfo = GetOrders();
			if (drInfo.HasRows)
			{
				while (drInfo.Read())
				{
					DateTime ordDate = DateTime.Parse(drInfo[2].ToString());
					tableBody = string.Concat(tableBody, "<tr><td>", drInfo[1], "</td><td>", ordDate.ToShortDateString(), "</td><td>", drInfo[3], "</td><td>", drInfo[4], "</td></tr>");
				}
			}
			else
			{
				tableBody = "<tr><td colspan='4' align='center'>No Order Due</td></tr>";
			}
			drInfo.Close();
			body += tableBody;
			body += "</table>";
			body += "<br><br>This is automated e-mail. Do not reply.";
			try
			{
				MailMessage message = new MailMessage("notification@elfainstall.com", to, "Customer Contact Due ", body);
				message.IsBodyHtml = true;
				SmtpClient mailClient = new SmtpClient(mailSeverName);
				mailClient.UseDefaultCredentials = true;
				mailClient.Send(message);
			}
			catch
			{
			}
		}
	}
}
