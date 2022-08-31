using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models.OrderBookModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Services
{
    public class GetSingleOrderBookService : IGetSingleOrderBookService
    {
        public FileService FileService { get; private set; }

        public GetSingleOrderBookService(/*IFileService fileService*/)
        {
            FileService = new();
        }
        public Task<List<Order>> GetSingleOrderBook()
        {
            throw new NotImplementedException();
        }

        public Task<Orders> ReadFromLocalStorageJsonOrderBook()
        {
            string pathToFolder = AppDomain.CurrentDomain.BaseDirectory + @"//ExchangJsons";
            string fileName = @"Sample-1-ExchangeData.json";

            var res = FileService.Read<Orders>(pathToFolder, fileName);
            return Task.FromResult(res);
        }

        public async Task<Dictionary<string, Orders>> ReadAllFilesFromLocalJsonOrderBook()
        {
            string pathToFolder = AppDomain.CurrentDomain.BaseDirectory + @"//ExchangJsons";
            string[] jsonDataFiles = Directory.GetFiles(pathToFolder);

            Dictionary<string, Orders> asks = new();
            foreach (var file in jsonDataFiles)
            {
                string tmpFileName = Path.GetFileName(file);
                var res = await FileService.ReadAsync<Orders>(pathToFolder, Path.GetFileName(tmpFileName));

                asks.Add(tmpFileName, res);
            }
            return asks;
        } 

    }
}
