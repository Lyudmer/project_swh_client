
using AutoMapper;
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Models;
using ClientSWH.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientSWH.DataAccess.Repositories
{
    public class StatusRepositoty(ClientSWHDbContext dbContext, IMapper mapper) : IStatusRepositoty
    {
        private readonly ClientSWHDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        public async Task<int> Add(Status status)
        {
            var stEntity = _mapper.Map<StatusEntity>(status);
            await _dbContext.AddAsync(stEntity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Status>(stEntity).Id;
        }
        public async Task Update(Status status)
        {
            await _dbContext.Status
                .Where(u => u.Id == status.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.StatusName, status.StatusName)
                                          .SetProperty(u => u.RunWf, status.RunWf)
                                          .SetProperty(u => u.MkRes, status.MkRes));
        }
        public async Task Delete(int Id)
        {
            await _dbContext.Status
                .Where(u => u.Id == Id)
                .ExecuteDeleteAsync();
        }
        public async Task<Status> GetById(int Id)
        {
            var stEntity = await _dbContext.Status
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == Id) ?? throw new Exception();
            return _mapper.Map<Status>(stEntity);
        }
    }
}
