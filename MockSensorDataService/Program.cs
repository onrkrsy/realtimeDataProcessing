// See https://aka.ms/new-console-template for more information
using MockSensorDataService;

var dataService = new DataGeneratorService();
await dataService.StartAsync();