using DataLoggerApp.ConstConfig;
using DataLoggerApp.FileHelpers;
using DataLoggerApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLoggerApp.Services
{
    public class TraceService : ITraceService
    {
        public void CreateOrUpdateLoggerTraceInDirectory()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryConfig.DirectoryName);
            DirectoryInfo rootDi = new DirectoryInfo(directoryPath);
            foreach (FileInfo file in rootDi.GetFiles())
            {
                DatasList data = null;
                FileTraceModel traceData = null;
                string filePath = Path.Combine(directoryPath, file.Name);
                DatasList dataList = new DatasList { Data = new List<Data>() };
                ReadLogDataFile(filePath, out data);
                if (data != null)
                {
                    dataList.Data.AddRange(data.Data);
                    dataList.TotalCount = dataList.Data.Count();
                    string dataTypeFileName = dataList.Data.FirstOrDefault()?.Type;
                    string traceFileName = TraceHelper.TraceNameCreator(file.Name, dataTypeFileName, directoryPath);

                    string directoryTracePath = Path.Combine(directoryPath, dataTypeFileName + "-Trace");
                    string traceFileNamePath = Path.Combine(directoryTracePath, traceFileName);
                 
                    string traceString = "";
                    if (System.IO.File.Exists(traceFileNamePath))
                    {
                        traceString = System.IO.File.ReadAllText(traceFileNamePath);
                        traceData = JsonConvert.DeserializeObject<FileTraceModel>(traceString);
                    }
                    else
                    {
                        TraceHelper.CreateTraceFile(dataList, traceFileNamePath, file.Name, traceFileName);
                    }

                    if (traceData != null)
                    {
                        foreach (var item in traceData.FileItemsList)
                        {
                            if (item.FileState == FileStatus.Full)
                            {
                                continue;
                            }
                            string fileItemNamePath = Path.Combine(directoryTracePath, item.FileItemName);
                            DatasList organizedList = new DatasList() { Data = new List<Data>() };
                            var toBeFetchedData = data.Data.Where(x => x.IsProcessed == false).Take(100).ToList();
                            organizedList.Data.AddRange(toBeFetchedData);
                            organizedList.TotalCount = toBeFetchedData.Count();
                            organizedList.Data.ForEach(UpdateProcessedItems);
                            if (item.FileState == FileStatus.Empty)
                            {
                                if (organizedList.Data.Any())
                                    FileHelper.WriteTextFile(fileItemNamePath, JsonConvert.SerializeObject(organizedList), false);
                            }

                            if (item.FileState == FileStatus.HavingSize)
                            {
                                DatasList traceFileData = ReadTraceData(fileItemNamePath);
                                if (organizedList.Data.Any())
                                {
                                    organizedList.Data.AddRange(traceFileData.Data);
                                    organizedList.TotalCount = organizedList.Data.Count();
                                    FileHelper.WriteTextFile(fileItemNamePath, JsonConvert.SerializeObject(organizedList), false);
                                }
                            }
                            if (organizedList.Data.Count == 100)
                                item.FileState = FileStatus.Full;
                            if (organizedList.Data.Count < 100)
                                item.FileState = FileStatus.HavingSize;
                      
                            traceData.TotalCount = data.Data.Count();
                            //FileHelper.WriteTextFile(traceFileNamePath, string.Empty, false);
                            FileHelper.WriteTextFile(traceFileNamePath, JsonConvert.SerializeObject(traceData), false);

                            foreach (var addeditem in organizedList.Data)
                            {
                                Data first = null;
                                foreach (var data1 in data.Data)
                                {
                                    if (addeditem != null && data1 == addeditem)
                                    {
                                        first = data1;
                                        break;
                                    }
                                }

                                if (first != null) first.IsProcessed = true;
                            }

                            var x = data.Data.Where(data1 => data1.IsProcessed == true).ToList();
                            //data.Data.ForEach(UpdateProcessedItems);
                            data.TotalCount = data.Data.Count();
                            data.ProcessedCount = data.Data.Count(data1 => data1.IsProcessed == true);
                            //FileHelper.WriteTextFile(filePath, string.Empty, false);
                            FileHelper.WriteTextFile(filePath, JsonConvert.SerializeObject(data), false);
                        }
                    }
                }
            }
        }

        private DatasList ReadTraceData(string fileItemNamePath)
        {
            string fileString = "";
            DatasList data = new DatasList { Data = new List<Data>() };

            if (System.IO.File.Exists(fileItemNamePath))
            {
                fileString = System.IO.File.ReadAllText(fileItemNamePath);
                data = JsonConvert.DeserializeObject<DatasList>(fileString);
            }

            return data;
        }

        private Data UpdateStatusProcess(Data obj)
        {
            obj.IsProcessed = true;
            return obj;
        }

        private void UpdateProcessedItems(Data dataItem)
        {
            dataItem.IsProcessed = true;
        }

        private DatasList ReadLogDataFile(string filePath, out DatasList data)
        {
            string fileString = "";
            data = new DatasList { Data = new List<Data>() };

            if (System.IO.File.Exists(filePath))
            {
                fileString = System.IO.File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<DatasList>(fileString);
            }

            return data;
        }
    }
}