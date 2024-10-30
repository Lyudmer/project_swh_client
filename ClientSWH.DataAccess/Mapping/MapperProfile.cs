using AutoMapper;
using ClientSWH.Core.Models;
using ClientSWH.DataAccess.Entities;

namespace ClientSWH.DataAccess.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DocumentEntity, Document>().ReverseMap();
            CreateMap<Document,DocumentEntity>().ReverseMap();
            CreateMap<PackageEntity, Package>().ReverseMap();

            CreateMap<Package, PackageEntity>().ReverseMap();

            CreateMap<StatusEntity, Status>().ReverseMap();
            CreateMap<Status, StatusEntity>().ReverseMap();
            CreateMap<UserEntity, User>().ReverseMap();

            CreateMap<HistoryPkgEntity, HistoryPkg>().ReverseMap();
            CreateMap<HistoryPkg, HistoryPkgEntity>().ReverseMap();

        }
    }
}
