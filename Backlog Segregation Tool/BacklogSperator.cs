using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Backlog_Segregation_Tool.Models;

namespace Backlog_Segregation_Tool
{
	public class BacklogSperator
	{
		public DataTable filtered_data;
		public DataTable challangers = new DataTable();
		public BacklogDataModel backlogData = new BacklogDataModel();
		public DataTable optimizers = new DataTable();
		public DataTable restdata = new DataTable();
		public DataTable diplomets { set; get; } = new DataTable();
		public int OptimizersAssignedCases { set; get; } = 0;
		public int OptimizersUnAssignedCases { set; get; } = 0;
		public int ChallangerAssignedCases { set; get; } = 0;
		public int ChallangerUnAssignedCases { set; get; } = 0;
		public int DiplometsAssignedCases { set; get; } = 0;
		public int DiplometsUnAssignedCases { set; get; } = 0;
		public BacklogSperator(DataTable dataTable)
		{
			filtered_data = dataTable;
		}
		public BacklogDataModel getSepratedBacklog()
		{
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(diplomets);
			dataSet.Tables.Add(challangers);
			dataSet.Tables.Add(optimizers);
			backlogData.backlog = dataSet;
			backlogData.ChallangerUnAssignedCases = ChallangerUnAssignedCases;
			backlogData.ChallangerAssignedCases = ChallangerAssignedCases;
			backlogData.DiplometsUnAssignedCases = DiplometsUnAssignedCases;
			backlogData.DiplometsAssignedCases = DiplometsAssignedCases;
			backlogData.OptimizersAssignedCases = OptimizersAssignedCases;
			backlogData.OptimizersUnAssignedCases = OptimizersUnAssignedCases;
			return backlogData;
		}
		public void getDiplomatsCases(String[] FINames)
		{
			if (FINames == null)
			{
				return;
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
			DiplometsUnAssignedCases=diplomets.AsEnumerable().Where(p => String.IsNullOrWhiteSpace(p.Field<string>("Assigned to"))).Count();
			DiplometsAssignedCases = diplomets.Rows.Count - DiplometsUnAssignedCases;
			return;
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
			ChallangerUnAssignedCases= challangers.AsEnumerable().Where(p => String.IsNullOrWhiteSpace(p.Field<string>("Assigned to"))).Count();
			ChallangerAssignedCases = challangers.Rows.Count - ChallangerUnAssignedCases;
			OptimizersUnAssignedCases = optimizers.AsEnumerable().Where(p => String.IsNullOrWhiteSpace(p.Field<string>("Assigned to"))).Count();
			OptimizersAssignedCases = optimizers.Rows.Count - OptimizersUnAssignedCases;
			return;
		}
	}
}
