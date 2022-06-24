using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
namespace Backlog_Segregation_Tool.BusinessLogic
{
	public static class Utility
	{
		public static DataTable ReOrderTable(DataTable table, String[] columnNames)
		{
			int columnIndex = 0;
			foreach (var columnName in columnNames)
			{
				table.Columns[columnName].SetOrdinal(columnIndex);
				columnIndex++;
			}
			int i = columnIndex;
			while (i < table.Columns.Count)
			{
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
	}
}
