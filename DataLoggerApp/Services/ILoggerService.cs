using DataLoggerApp.Models;

namespace DataLoggerApp.Services
{
    public interface ILoggerService
    {
        void CreateFilesByDataType(DatasList request);

      
    }
}