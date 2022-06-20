using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
namespace Backlog_Segregation_Tool
{
	public class BacklogSperator
	{
		public DataTable filtered_data;
		public DataTable challangers = new DataTable();
		public DataTable optimizers = new DataTable();
		public DataTable restdata = new DataTable();
		public DataTable diplomets { set; get; } = new DataTable();
		public BacklogSperator(DataTable dataTable)
		{
			filtered_data = dataTable;
		}

		public DataTable getDiplomatsCases(String[] FINames)
		{
			if (FINames == null)
			{
				return new DataTable();
			}


			diplomets = filtered_data.Clone();
			diplomets.TableName = "Diplomets";
			restdata = filtered_data.Clone();
			for (int i = 0; i < filtered_data.Rows.Count; i++)
			{
				bool flag = false;
				if (filtered_data.Rows[i]["Client Name"] != null)
				{
					for (int j = 0; j < FINames.Length; j++)
					{

						if (filtered_data.Rows[i]["Client Name"].ToString() == FINames[j])
						{
							diplomets.ImportRow(filtered_data.Rows[i]);
							flag = true;
							//break;

						}

					}
					if (flag == false)
					{

						restdata.ImportRow(filtered_data.Rows[i]);

					}
				}

			}
			return diplomets;
		}

		public void getChallangersCases(int MaxNumberOfDays)
		{
			challangers = filtered_data.Clone();
			challangers.TableName = "Challangers";

			optimizers = filtered_data.Clone();
			optimizers.TableName = "Optimizers";
			for (int i = 0; i < restdata.Rows.Count; i++)
			{

				String date = restdata.Rows[i]["Created Date"].ToString();
				int mm = Convert.ToInt32(date.Substring(0, 2));
				int dd = Int32.Parse(date.Substring(3, 2));
				int yy = Int32.Parse(date.Substring(6, 4));
				DateTime today = DateTime.UtcNow;
				DateTime dt = new DateTime(yy, mm, dd, 0, 0, 0, 0);
				int days = Convert.ToInt32((today - dt).TotalDays);
				if (days < MaxNumberOfDays)
				{
					challangers.ImportRow(restdata.Rows[i]);
				}
				else
				{
					optimizers.ImportRow(restdata.Rows[i]);
				}
			}
			return;
		}
	}
}
