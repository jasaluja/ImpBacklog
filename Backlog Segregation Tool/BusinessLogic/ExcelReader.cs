using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backlog_Segregation_Tool.BusinessLogic;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;

namespace Backlog_Segregation_Tool
{
    public class ExcelReader
    {
        public DataSet dataSet { set; get; }
        public DataTable FilteredData;
        IConfiguration configuration;


        public ExcelReader(String path, String groupName)
        {

            Console.WriteLine();

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
                var dataTable = dataSet.Tables[0];

                var q = dataTable.AsEnumerable().Where(p => p.Field<string>("Assignment Group") == groupName);

                var dt2 = q.AsDataView();
                FilteredData = dt2.ToTable();

            }
        }
    }
}

