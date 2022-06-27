using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Backlog_Segregation_Tool.BusinessLogic
{
	public static class Utility
	{
		public static DataTable ReOrderTable(DataTable table, String[] columnNames)
		{
			int columnIndex = 0;
			if (columnNames.Length == 1 && columnNames[0] == "") return table;
			foreach (var columnName in columnNames)
			{
				table.Columns[columnName].SetOrdinal(columnIndex);
				columnIndex++;
			}
			int i = columnIndex;
			while (i < table.Columns.Count)
			{
				//if(table.Columns[i].ColumnName!="PreTriageStatus")
				table.Columns.RemoveAt(i);
			}
			return table;
		}

		public static DataTable AddPreTriageStatusColumn(DataTable backlog)
		{
			backlog.Columns.Add("PreTriageStatus", typeof(String));
			foreach (var data in backlog.AsEnumerable())
			{
				String tag = data.Field<String>("Tag");
				if (tag != null)
				{
					if (tag.ToLower().Contains("PretriageAccepted".ToLower()))
					{
						data.BeginEdit();
						data["PreTriageStatus"] = "Accepted";
						data.EndEdit();
					}
					else
					{
						data.BeginEdit();
						data["PreTriageStatus"] = "";
						data.EndEdit();
					}

				}
				else
				{
					data.BeginEdit();
					data["PreTriageStatus"] = "";
					data.EndEdit();
				}
			}
			return backlog;
		}
	
	public static String[] ColumnsOrder;
	public static String[] diplometsClients;
	public static String ChallangerTag;
	public static int ChallangersMaxDays;
	public static void ValidateConfig(IConfiguration _config)
	{
			
			
			if (_config.GetSection("DiplometsClients")!=null)
			{
				diplometsClients = _config.GetSection("DiplometsClients").Get<List<string>>().ToArray();
			}
			else
			{
				diplometsClients = new String[] { "" };
			}
			if (!String.IsNullOrEmpty(_config.GetSection("ChallangerTag").Value))
			{
				ChallangerTag = _config.GetSection("ChallangerTag").Value.ToString();
			}
			else
			{
				ChallangerTag = "";
			}
			if (!String.IsNullOrEmpty(_config.GetSection("ChallangersMaxDays").Value))
			{

				ChallangersMaxDays = Convert.ToInt32(_config.GetSection("ChallangersMaxDays").Value.ToString());

			}

			if (_config.GetSection("ColumnsOrder") != null)
			{
				try
				{
					ColumnsOrder = _config.GetSection("ColumnsOrder").Get<List<string>>().ToArray();
				}catch(Exception e)
				{
					ColumnsOrder = new string[] { "" };
				}
			}
			else
			{
				ColumnsOrder = new string[] { "" };
			}

		
		
	}
}
}
