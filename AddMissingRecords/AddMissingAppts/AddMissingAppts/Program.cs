using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AddMissingAppts
{
	internal class Program
	{
		private class CreateData
		{
			private static readonly string SqlString = ConfigurationManager.ConnectionStrings["ElfaConn"].ToString();

			private readonly SqlConnection _objConn = new SqlConnection(SqlString);

			public void RecordsAdd()
			{
				try
				{
					SqlCommand objComm = new SqlCommand();
					objComm.Connection = _objConn;
					objComm.CommandType = CommandType.StoredProcedure;
					objComm.CommandText = "AddMissingAppointments";
					_objConn.Open();
					objComm.ExecuteNonQuery();
					_objConn.Close();
				}
				catch (Exception ex)
				{
					AddErrorLog("AddMissingAppts", "RecordsAdd", ex.Message);
				}
			}

			private void AddErrorLog(string PageName, string ProcName, string Message)
			{
				SqlCommand objComm = new SqlCommand();
				objComm.Connection = _objConn;
				objComm.CommandType = CommandType.StoredProcedure;
				objComm.CommandText = "sp_AddErrorLog";
				objComm.Parameters.AddWithValue("@PageName", PageName);
				objComm.Parameters.AddWithValue("@ProcName", ProcName);
				objComm.Parameters.AddWithValue("@Message", Message);
				_objConn.Open();
				objComm.ExecuteNonQuery();
				_objConn.Close();
			}
		}

		private static void Main()
		{
			CreateData process = new CreateData();
			process.RecordsAdd();
		}
	}
}
