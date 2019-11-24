using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLoggerApp.Mapper;
using DataLoggerApp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace DataLoggerApp.FileHelpers
{
    public static class TraceHelper
    {
        public static void UpdateTraceFile(DatasList dataList, string traceFileNamePath, string fileName, string traceFileName)
        {
            FileTraceModel model = ReadTraceFile(traceFileNamePath);
            //model.Data.AddRange(dataList.Data);

            int filesCount = 0;
            int itemsCount = dataList.Data.Count();
            while(itemsCount >= 100)
            {
                filesCount++;
                itemsCount -= 100;
            }
            if (itemsCount % 100 > 0 || itemsCount > 0)
            {
                filesCount++;
            }


     

            for (int i = model.FilesItemCount+1; i <= filesCount; i++)
            {
                StringBuilder itemName = new StringBuilder(fileName);
                int nameNumber = model.FilesItemCount + 1;
                FileItemModel fileItem = new FileItemModel() { FileItemName = itemName.Replace(".log", "-000" + (nameNumber)).Append(".log").ToString(), FileState = FileStatus.Empty };
                model.FileItemsList.Add(fileItem);
                model.FilesItemCount = filesCount;
            }
           
            model.TotalCount = dataList.Data.Count();
            FileHelper.WriteTextFile(traceFileNamePath,string.Empty,false);
            FileHelper.WriteTextFile(traceFileNamePath, JsonConvert.SerializeObject(model), false);
        }


        public static void CreateTraceFile(DatasList dataList, string traceFileNamePath, string logFileName, string traceFileName)
        {
            int filesCount = dataList.Data.Count() / 100;
            if (dataList.Data.Count() % 100 > 0)
            {
                filesCount++;
            }

            FileTraceModel model = new FileTraceModel() { FileName = logFileName, FileItemsList = new List<FileItemModel>(), FilesItemCount = filesCount };

            for (int i = 0; i < filesCount; i++)
            {
                StringBuilder itemName = new StringBuilder(logFileName);
                FileItemModel fileItem = new FileItemModel() { FileItemName = itemName.Replace(".log", "-000" + (i + 1)).Append(".log").ToString(), FileState = FileStatus.Empty };
                model.FileItemsList.Add(fileItem);
            }

            model.TotalCount = dataList.Data.Count();
            FileHelper.WriteTextFile(traceFileNamePath, string.Empty, false);
            FileHelper.WriteTextFile(traceFileNamePath, JsonConvert.SerializeObject(model), false);
        }


        public static string TraceNameCreator(string fileName,string dataTypeFileName, string directoryPath)
        {
            string traceFileName = "Trace";
            if (!string.IsNullOrWhiteSpace(dataTypeFileName))
            {
                StringBuilder traceFileNameBuilder = new StringBuilder(fileName);
                traceFileNameBuilder.Replace(dataTypeFileName + "-", traceFileName);
                traceFileName = traceFileNameBuilder.ToString();
            }

            return traceFileName;
        }

        private static FileTraceModel ReadTraceFile(string traceFileNamePath)
        {

            string fileString = System.IO.File.ReadAllText(traceFileNamePath);
            return JsonConvert.DeserializeObject<FileTraceModel>(fileString);

        }
    }
}
