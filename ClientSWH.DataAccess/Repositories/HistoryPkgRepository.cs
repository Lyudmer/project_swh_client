

using AutoMapper;
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Models;
using ClientSWH.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientSWH.DataAccess.Repositories
{
    public class HistoryPkgRepository(ClientSWHDbContext dbContext, IMapper mapper) : IHistoryPkgRepository
    {
        private readonly ClientSWHDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        public async Task<HistoryPkg> Add(HistoryPkg HpPkg)
        {
            var HpPkgEntity = _mapper.Map<HistoryPkgEntity>(HpPkg);
            var resEntity=await _dbContext.AddAsync(HpPkgEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<HistoryPkg>(resEntity.Entity);
           
        }
        public async Task<HistoryPkg> GetById(int Pid)
        {
            var hpPkgEntity = await _dbContext.HistoryPkg
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Pid == Pid);
            if (hpPkgEntity == null) return null;
            else return _mapper.Map<HistoryPkg>(hpPkgEntity);

        }
        
    }
}
