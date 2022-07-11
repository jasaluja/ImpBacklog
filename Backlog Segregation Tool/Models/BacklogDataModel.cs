using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
namespace Backlog_Segregation_Tool.Models
{
	public class BacklogDataModel
	{
		public String SelectedGroup { set; get; }
		public DataSet backlog { set; get; }
		public int OptimizersAssignedCases { set; get; } = 0;
		public int OptimizersUnAssignedCases { set; get; } = 0;
		public int ChallangerAssignedCases { set; get; } = 0;
		public int ChallangerUnAssignedCases { set; get; } = 0;
		public int DiplometsAssignedCases { set; get; } = 0;
		public int DiplometsUnAssignedCases { set; get; } = 0;
		public Boolean IsImpSelected { set; get; }
		public Boolean IsBaseSelected { set; get; }

	}
	public class TasksBacklogDataModel
	{
		public BacklogDataModel FTDataModel;
		public BacklogDataModel SwDataModel;
	}
}
