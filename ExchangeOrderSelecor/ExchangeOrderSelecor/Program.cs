// See https://aka.ms/new-console-template for more information
using ExchangeOrderSelecor.Contracts.Services;
using ExchangeOrderSelecor.Models;
using ExchangeOrderSelecor.Models.CustomerModel;
using ExchangeOrderSelecor.Services;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System;
using System.Runtime.CompilerServices;
using ExchangeOrderSelecor.Models.Enums;
using ExchangeOrderSelecor.Models.OrderBookModel;


//Create a sample Customer with a sample wallet and Random Balance
GenerateSampleCustomerService _sampleCustomer = new GenerateSampleCustomerService();
Customer SampleCustomer = await _sampleCustomer.GetSampleCustomer();

GenerateCustomerOrderService _generateCustomerOrder = new GenerateCustomerOrderService();
FindBestOrderService findBestOrderService = new FindBestOrderService();


Console.WriteLine($"Customer\nName:{SampleCustomer.CustomerName}" +
    $"\nId: {SampleCustomer.CustomerId}" +
    $"\nOrders: {SampleCustomer.CustomerOrders.Count}" +
    $"\n\nWallet Info" +
    $"\nBTC: {SampleCustomer.Wallet.BTCAvailable} BTC" +
    $"\nEuro: {SampleCustomer.Wallet.EuroAvailable} €\n\n\n");

Console.WriteLine("type 'B' to Buy BTC type 'S' to sell BTC");


string? selectedOrderType = string.Empty;
bool isSelectionOk = false;
while (!isSelectionOk)
{
    selectedOrderType = Console.ReadLine().ToUpper();
    if (!string.IsNullOrEmpty(selectedOrderType) &&
        (selectedOrderType.Equals("b", comparisonType: StringComparison.CurrentCultureIgnoreCase) ||
        selectedOrderType.Equals("s", comparisonType: StringComparison.CurrentCultureIgnoreCase)))

    {
        isSelectionOk = true;
        Console.WriteLine($"selected -> {selectedOrderType}");
        break;
    }
    Console.WriteLine("Please Type only B or S to select order type");
}

Console.WriteLine("select amount of BTC to Buy or Sell");

string? btcToProcessInput = string.Empty;
decimal btcAmountToProcess = 0;
bool isSellingPossible = false;
isSelectionOk = false;

while (!(isSelectionOk = false && isSellingPossible == false))
{
    btcToProcessInput = Console.ReadLine();
    if (!string.IsNullOrEmpty(btcToProcessInput) &&
        Double.TryParse(Convert.ToString(btcToProcessInput),
        System.Globalization.NumberStyles.Any,
        System.Globalization.NumberFormatInfo.InvariantInfo,
        out _))
    {
        btcAmountToProcess = Convert.ToDecimal(btcToProcessInput);
        if (!(btcAmountToProcess > 0))
        {
            Console.WriteLine("Must be greater 0");

            continue;
        }

        if (selectedOrderType.Equals("s", comparisonType: StringComparison.CurrentCultureIgnoreCase) && 
            SampleCustomer.Wallet.BTCAvailable < btcAmountToProcess)
        {
            Console.WriteLine("Not Enought BTC To Sell");
            isSellingPossible = false;
            continue;
        }
        isSellingPossible = true;
        isSelectionOk = true;
        Console.WriteLine($"Amount of BTC selcted -> {btcToProcessInput}");
        break;
    }
    Console.WriteLine("Please Type only numeric values or try replacing '.' ',' ");
}


Dictionary<SelectedOrder, decimal> selectedOrders = new();
SelectedOrders bestOrderesRes = new();


switch (selectedOrderType)
{
    case "B":
        Console.WriteLine($"\nPurchasing {btcToProcessInput} BTC ...");

        CustomerOrder buyOrderPlaced = await _generateCustomerOrder.PlaceBuyOrder(SampleCustomer, btcAmountToProcess);
        SampleCustomer.CustomerOrders.Add(buyOrderPlaced);
        
        Console.WriteLine("\nSelecting best option for the Order...");

        //selectedOrders = await findBestOrderService.GetOrdersToBuy(orderPlaced, SampleCustomer);
        bestOrderesRes = await findBestOrderService.GetBestOrdersToBuy2Async(buyOrderPlaced, SampleCustomer);

        break;

    case "S":
        Console.WriteLine($"Selling {btcToProcessInput} BTC...\n");

        CustomerOrder sellOrderPlaced = await _generateCustomerOrder.PlaceSellOrder(SampleCustomer, btcAmountToProcess);
        
        SampleCustomer.CustomerOrders.Add(sellOrderPlaced);

        bestOrderesRes = await findBestOrderService.GetBestOrdersToSell2Async(sellOrderPlaced, SampleCustomer);
        bestOrderesRes.IsExcecutable = isSellingPossible;
        break;

    default:
        break;
}
//decimal priceToPay = 0;

if (bestOrderesRes.IsExcecutable == false)
{
    Console.WriteLine($"Not enought Money to execute the Order please Deposit\n");
    Console.WriteLine($"Currently available: {SampleCustomer.Wallet.EuroAvailable} Euro\n");
    Console.WriteLine($"Requierd amount: {bestOrderesRes.PriceToPay} Euro\n");
}
else if(SampleCustomer.CustomerOrders.First().OrderType == OrderType.Buy)
{
    Console.WriteLine($"File name: {bestOrderesRes.FileJsonName}\n");

    foreach (var item in bestOrderesRes.ordersSelected)
    {
        Console.WriteLine($"Order Id's selected: {item.Id}\t Price/BTC {item.PriceProBtc}€");
    }
    Console.WriteLine($"PriceToPay: {bestOrderesRes.PriceToPay} € \n");
}
else if(SampleCustomer.CustomerOrders.First().OrderType == OrderType.Sell)
{
    Console.WriteLine($"File name: {bestOrderesRes.FileJsonName}\n");

    foreach (var item in bestOrderesRes.ordersSelected)
    {
        Console.WriteLine($"Order Id's selected: {item.Id}\t Price/BTC {item.PriceProBtc}€");
    }
    Console.WriteLine($"PriceToGet: {bestOrderesRes.PriceToGet} € \n");
}


Console.WriteLine("finished");

