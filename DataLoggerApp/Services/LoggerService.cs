using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataLoggerApp.ConstConfig;
using DataLoggerApp.FileHelpers;
using DataLoggerApp.Models;
using Newtonsoft.Json;

namespace DataLoggerApp.Services
{
    public class LoggerService : ILoggerService
    {


        public void CreateFilesByDataType(DatasList request)
        {
            foreach (var typedList in request.Data.GroupBy(x => x.Type))
            {
                DatasList dataList = new DatasList { Data = new List<Data>() };
                dataList.Data.AddRange(typedList);
                dataList.TotalCount = dataList.Data.Count();

                string date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
                string fileName = $"{typedList.Key}-{date}.log";

                string path = Path.Combine(Directory.GetCurrentDirectory(), DirectoryConfig.DirectoryName);

                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
                string newPath = Path.Combine(path, fileName);
                string list = "";
                DatasList data = null;

                if (System.IO.File.Exists(newPath))
                {
                    list = System.IO.File.ReadAllText(newPath);
                    data = JsonConvert.DeserializeObject<DatasList>(list);
                }

                if (data != null)
                {
                    dataList.Data.AddRange(data.Data);
                    dataList.TotalCount = dataList.Data.Count();
                    System.IO.File.WriteAllText(newPath, string.Empty);
                }
                DatasList traceListData=new DatasList(){Data = new List<Data>()};
                traceListData.Data.AddRange(typedList);
                CreateUpdateTraceFile(dataList, fileName);
                FileHelper.WriteTextFile(newPath, JsonConvert.SerializeObject(dataList), false);
            }
        }

        private void CreateUpdateTraceFile(DatasList dataList, string fileName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryConfig.DirectoryName);
            string dataTypeFileName = dataList.Data.FirstOrDefault()?.Type;
            
            string traceFileName = "Trace";
            traceFileName = TraceHelper.TraceNameCreator(fileName, dataTypeFileName, directoryPath);
           
            string directoryTracePath = Path.Combine(directoryPath, dataTypeFileName + "-Trace");
            string traceFileNamePath = Path.Combine(directoryTracePath, traceFileName);
            if (!Directory.Exists(directoryTracePath))
            {
                DirectoryInfo di = Directory.CreateDirectory(directoryTracePath);
            }
            if (!System.IO.File.Exists(traceFileNamePath))
            {
                TraceHelper.CreateTraceFile(dataList, traceFileNamePath, fileName, traceFileName);
            }
            else
            {
                TraceHelper.UpdateTraceFile(dataList, traceFileNamePath, fileName, traceFileName);
            }
        }

      
    }
}