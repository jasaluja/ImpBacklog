﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backlog_Segregation_Tool.BusinessLogic;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Backlog_Segregation_Tool.BusinessLogic
{
    public static class ExcelReader
    {
        public static DataSet dataSet { set; get; }
        public static DataTable FilteredData;
        static IConfiguration configuration;
        public static bool ValidateExcelCoulmns(IFormFile file,String[] mustColumns)
        {
            List<String> errors = new List<String>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    IExcelDataReader reader;

                    file.CopyTo(stream);
                    reader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(stream);

                    //// reader.IsFirstRowAsColumnNames


                    dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                }
            }catch(Exception e)
			{
                throw e;
			}
                // Now you can get data from each sheet by its index or its "name"/// a b c d   //c d
                var dataTable = dataSet.Tables[0];
                for (int i = 0; i < mustColumns.Length; i++)
                {
                    bool matched = false;
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (String.Equals(dataTable.Columns[j].ColumnName, mustColumns[i], StringComparison.OrdinalIgnoreCase))
                        {
                            matched = true;
                            break;
                        }
                    }
					if (!matched)
					{
                        return false;
					}
                }
                return true;
            }
        public static DataTable MergeDataTables(DataTable dataTable1,DataTable dataTable2)
		{
            dataTable1.PrimaryKey = new DataColumn[] { dataTable1.Columns["Inquiry Number"] };
            dataTable2.PrimaryKey = new DataColumn[] { dataTable2.Columns["Inquiry Number"] };
            dataTable2.Merge(dataTable1);
            return dataTable1;
		}
        public static DataTable ReadExcel(String path)
		{
            DataTable fileData;
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader;


                reader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(stream);

                //// reader.IsFirstRowAsColumnNames


                dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                // Now you can get data from each sheet by its index or its "name"
                 fileData = dataSet.Tables[0];

            }
            return fileData;

        }
        public static DataTable FilterByGroupName(DataTable dataTable, String groupName) {
            
            var q = dataTable.AsEnumerable().Where(p => p.Field<string>("Assignment Group") == groupName);

            var dt2 = q.AsDataView();
             return dt2.ToTable();
           
        }
       
    }
}

