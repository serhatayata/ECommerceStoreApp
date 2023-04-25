using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class ClientService : IClientService
    {
        private AppConfigurationDbContext _confDbContext;
        private IMapper _mapper;

        public ClientService(AppConfigurationDbContext confDbContext, IMapper mapper)
        {
            _confDbContext = confDbContext;
            _mapper = mapper;
        }

        public async Task<DataResult<ClientDto>> AddAsync(ClientDto model)
        {
            var mappedClient = _mapper.Map<IdentityServer4.EntityFramework.Entities.Client>(model);
            await _confDbContext.Clients.AddAsync(mappedClient);
            var result = await _confDbContext.SaveChangesAsync();
            return result == 1 ? new SuccessDataResult<ClientDto>(model) : new ErrorDataResult<ClientDto>();
        }

        public DataResult<ClientDto> Update(ClientDto model)
        {
            var mappedClient = _mapper.Map<IdentityServer4.EntityFramework.Entities.Client>(model);
            _confDbContext.Clients.Update(mappedClient);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessDataResult<ClientDto>() : new ErrorDataResult<ClientDto>();
        }

        public Result DeleteAsync(string clientId)
        {
            var existingClient = _confDbContext.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if (existingClient == null)
                return new ErrorResult();

            _confDbContext.Clients.Remove(existingClient);
            var result = _confDbContext.SaveChanges();
            return result == 1 ? new SuccessResult() : new ErrorResult();
        }

        public async Task<DataResult<List<ClientDto>>> GetAllAsync()
        {
            var result = await _confDbContext.Clients.AsNoTracking().ToListAsync();
            var mappedResult = _mapper.Map<List<ClientDto>>(result);

            return new SuccessDataResult<List<ClientDto>>(mappedResult);
        }

        public async Task<DataResult<ClientDto>> GetAsync(string clientId)
        {
            var result = await _confDbContext.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == clientId);
            var mappedResult = _mapper.Map<ClientDto>(result);

            return new SuccessDataResult<ClientDto>(mappedResult);
        }
    }
}
