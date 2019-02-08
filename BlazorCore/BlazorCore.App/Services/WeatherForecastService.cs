using BlazorCore.Core.Models;
using BlazorCore.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCore.App.Services
{
    public class WeatherForecastService
    {

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public void PostToDo(TodoItem todo)
        {
        }

    }
}