using PowerPlant.Domain;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace PowerPlant.WebApi.Server.Controllers
{
    [RoutePrefix("api/v1/readings")]
    public class ReadingsController : ApiController
    {
        private readonly ReadingsService _readingsService;

        public ReadingsController()
        {
            _readingsService = ReadingsService.Instance;
        }

        [HttpGet]
        [Route("dataset")]
        public async Task<NewDataSet> GetNewDataSetAsync()
        {
            return await _readingsService.GetNewDataSetAsync();
        }

        [HttpGet]
        [Route("powerdataset")]
        public async Task<PowerDataSet[]> GetNewPowerDataSetAsync()
        {
            return await _readingsService.GetPowerDataSetAsync();
        }

        [HttpGet]
        [Route("time")]
        public async Task<DateTime> GetReadingTimeAsync()
        {
            return await _readingsService.GetReadingTimeAsync();
        }

        [HttpPost]
        [Route("")]
        public async void UpdateTimeAsync([FromBody] DateTime dateTime)
        {
            await _readingsService.UpdateTimeAsync(dateTime);
        }

        [HttpGet]
        [Route("dates/{floorValue}/{ceilingValue}")]
        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            return await _readingsService.GetCriticalReadingsAsync(floorValue, ceilingValue);
        }

        [HttpGet]
        [Route("statisticdates/{floorValue}/{ceilingValue}")]
        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            return await _readingsService.GetCriticalStatisticsAsync(floorValue, ceilingValue);
        }
    }
}