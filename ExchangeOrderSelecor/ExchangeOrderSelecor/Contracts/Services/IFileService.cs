using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Contracts.Services
{
    public interface IFileService
    {
        T Read<T>(string folderPath, string fileName);
        Task<T> ReadAsync<T>(string folderPath, string fileName);
        void Save<T>(string folderPath, string fileName, T content);

        void Delete(string folderPath, string fileName);
    }
}
