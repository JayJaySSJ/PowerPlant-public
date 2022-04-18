using PowerPlant.Domain;
using PowerPlant.Infrastructure;
using PowerPlant.Wcf.ServiceDefinition;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemberFunction = PowerPlant.Domain.Models.MemberFunction;

namespace PowerPlant.Wcf.SelfhostServer
{
    internal class PowerPlantServiceDefinition : IInspectionsManagementClient, IMembersManagementClient, IReadingsManagementClient
    {
        private readonly InspectionsService _inspectionsService;
        private readonly MembersService _membersService;
        private readonly ReadingsService _readingsService;

        private readonly Mapper _mapper;

        public PowerPlantServiceDefinition()
        {
            _inspectionsService = new InspectionsService(
                new InspectionsRepository(), 
                new PlantDataProvider());
            _membersService = new MembersService(
                new MembersRepository());
            _readingsService = ReadingsService.Instance;

            _mapper = new Mapper();
        }

        public async Task<bool> CreateInspectionTicketAsync(InspectionTicket inspectionTicket)
        {
            if (_membersService.GetAsync(_membersService.GetLoggedMember()).Result.Function == MemberFunction.Engineer)
            {
                return false;
            }
            return await _inspectionsService.CreateAsync(_mapper.MapTicketToDomain(inspectionTicket));
        }

        public async Task<List<InspectionTicket>> GetAllStatusTicketsAsync()
        {
            return _mapper.MapTicketsToWcf(await _inspectionsService.GetAllAsync());
        }

        public async Task<bool> ItemExistsAsync(string itemName)
        {
            return await _inspectionsService.ItemExistsAsync(itemName);
        }

        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            return await _inspectionsService.OpenTicketExistsAsync(itemName);
        }

        public async Task<bool> AssignTicketAsync(InspectionTicket pickedTicket)
        {
            return await _inspectionsService.AssignAsync(_mapper.MapTicketToDomain(pickedTicket));
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public async Task<bool> CreateAsync(Member member)
        {
            if (!_membersService.IsAdminLogged())
            {
                return false;
            }
            return await _membersService.CreateAsync(_mapper.MapMemberToDomain(member));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!_membersService.IsAdminLogged())
            {
                return false;
            }
            return await _membersService.DeleteAsync(id);
        }

        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            return _mapper.MapMembersToWcf(await _membersService.GetAllAsync());
        }

        public async Task<Member> GetAsync(string login)
        {
            return _mapper.MapMemberToWcf(await _membersService.GetAsync(login));
        }

        public async Task<bool> MemberExists(string login, string password)
        {
            return await _membersService.MemberExistsAsync(login, password);
        }

        public async Task UpdateLoggedMemberAsync(string login)
        {
            await _membersService.UpdateLoggedMemberAsync(login);
        }

        // ----------------------------------------------------------------------------------------------------------------------

        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime beginDate, DateTime endDate)
        {
            return _mapper.MapCriticalStatisticsToWcf(await _readingsService.GetCriticalStatisticsAsync(beginDate, endDate));
        }

        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime beginDate, DateTime endDate)
        {
            return _mapper.MapCriticalReadingsToWcf(await _readingsService.GetCriticalReadingsAsync(beginDate, endDate));
        }

        public async Task<NewDataSet> GetNewDataSetAsync()
        {
            return _mapper.MapNewDataSetToWcf(await _readingsService.GetNewDataSetAsync());
        }

        public async Task<PowerDataSet[]> GetPowerDataSetAsync()
        {
            return _mapper.MapPowerDataSetsToWcf(await _readingsService.GetPowerDataSetAsync());
        }

        public async Task<DateTime> GetReadingTimeAsync()
        {
            return await _readingsService.GetReadingTimeAsync();
        }

        public async Task UpdateTime(DateTime dateTime)
        {
            await _readingsService.UpdateTimeAsync(dateTime);
        }
    }
}