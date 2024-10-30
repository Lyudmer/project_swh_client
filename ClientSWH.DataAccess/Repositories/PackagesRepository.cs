﻿
using AutoMapper;
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Models;
using ClientSWH.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;


namespace ClientSWH.DataAccess.Repositories
{
    public class PackagesRepository(ClientSWHDbContext dbContext, IMapper mapper) : IPackagesRepository
    {
        private readonly ClientSWHDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        public async Task<Package> Add(Package Pkg)
        {
            var PkgEntity = _mapper.Map<PackageEntity>(Pkg);
            var resPkg= await _dbContext.AddAsync(PkgEntity);
            var resSt=await _dbContext.SaveChangesAsync();
            if (resSt==1)
                return _mapper.Map<Package>(resPkg.Entity);
            else return null;
        }
        public async Task<Package> GetByUUId(Guid uuid)
        {
            var pkgEntity = await _dbContext.Packages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UUID == uuid);
            if (pkgEntity == null)  return null;
            else return _mapper.Map<Package>(pkgEntity);

        }
        public async Task<Package> GetById(int Pid)
        {
            var pkgEntity = await _dbContext.Packages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Pid);
                
            if (pkgEntity == null) return null;
            else return _mapper.Map<Package>(pkgEntity);

        } 
        public async Task<List<Package>> GetAll()
        {
            var query = _dbContext.Packages
                .AsNoTracking()
                .OrderBy(p => p.UserId)
                .ThenBy(p => p.Id);
            var pkgList = await query.ToListAsync();
            if (pkgList == null) return null;
            else return _mapper.Map<List<Package>>(pkgList);
        }
        public async Task<List<Package>> GetPkgUser(Guid UserId)
        {
            var query = _dbContext.Packages
                .AsNoTracking()
                .OrderBy(p => p.Id);
            var pkgList = await query.ToListAsync();
            if (pkgList == null) return null;
            else return _mapper.Map<List<Package>>(pkgList);
        }
        public async Task<Package> GetPkgWithDoc(int Pid)
        {
            var pkgEntity = await _dbContext.Packages
                .AsNoTracking()
                .Include(p => p.Documents)
                .FirstOrDefaultAsync(p => p.Id == Pid);

            if (pkgEntity == null) return null;
            else
                return _mapper.Map<Package>(pkgEntity);

        }
        public async Task<List<Package>> GetUserStatus(Guid UserId, int Status)
        {
            var query = _dbContext.Packages.AsNoTracking();
            var user = UserId.ToString();
            if (user is not null)
            { query = query.Where(p => p.UserId == UserId); }
            if (Status > -1)
            { query = query.Where(p => p.StatusId == Status); }
            var pkgList = await query.ToListAsync();
            if (pkgList == null) return null;
            else
                return _mapper.Map<List<Package>>(pkgList);
        }
        public async Task<List<Package>> GetByPage(int Page, int Page_Size)
        {
            var query = _dbContext.Packages
                .AsNoTracking()
                .Skip((Page - 1) * Page_Size)
                .Take(Page_Size);
            var pkgList = await query.ToListAsync();
            if (pkgList == null) return null;
            else
                return _mapper.Map<List<Package>>(pkgList);

        }
        public async Task UpdateStatus(int Pid, int Status)
        {
            await _dbContext.Packages
                .Where(p => p.Id == Pid)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.StatusId, Status)
                                          .SetProperty(p => p.ModifyDate, DateTime.UtcNow));
        }
        public async Task Delete(int Pid)
        {
            await _dbContext.Packages
                .Where(u => u.Id == Pid)
                .ExecuteDeleteAsync();
        }
        public async Task<int> GetLastPkgId()
        {
            int cPkg = 0;
            try
            {
                var resId = await _dbContext.Packages.MaxAsync(p=>p.Id);
                if (resId != 0) cPkg = resId;
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }
            return cPkg;
        }

    }
}
