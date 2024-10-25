﻿

using ClientSWH.Application.Services;
using ClientSWH.Contracts;
using ClientSWH.Core.Abstraction.Services;
using System.Text;




namespace ClientSWH.Endpoints
{
    public static  class PackagesEndpoints
    {
        public static IEndpointRouteBuilder MapPackagesEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("Packages");
            app.MapPost("LoadFile {UserId:guid}", LoadFile);
            app.MapPost("PkgFLK {Pid:int}", PkgFLK);
            app.MapGet("LoadPackage", LoadMessage);
            app.MapGet("GetHistory{Pid:int}", GetHistoryPkg);
            app.MapGet("GetAllPackage", GetAllPkg);
            app.MapGet("GetPackage{Pid:int}", GetPkgId);
            app.MapDelete("DelPackage{Pid:int}", DeletePkg);
            app.MapPost("SendToServer {Pid:int}", SendPkgToServer);
            app.MapPost("SendToServerDelPkg {Pid:int}", SendDelPkgToServer);
            app.MapGet("GetDocsPackage{Pid:int}", GetDocsPkg);
            return app;
        }
        private static async Task<IResult> LoadMessage(PackagesServices pkgService)
        {
           var res= await ((IPackagesServices)pkgService).LoadMessage();
            return Results.Ok(res);
        }
        private static async Task<IResult> PkgFLK(int Pid, PackagesServices pkgService)
        {
            var res = await ((IPackagesServices)pkgService).PkgFLK(Pid);
            return Results.Ok(res);
        }
        private static async Task<IResult> LoadFile(LoadFileRequest request, PackagesServices pkgService, Guid UserId)
        {
            
            using (var fileStream = new FileStream(request.FileName, FileMode.Create))
            {
                
                fileStream.Position = 0;
                using (StreamReader reader = new(fileStream, Encoding.UTF8))
                {
                    var resFile = reader.ReadToEnd();
                    await ((IPackagesServices)pkgService).LoadFile(UserId, resFile);
                }
            }
            
            return Results.Ok();
        }
        
        private static async Task<IResult> SendPkgToServer(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).SendToServer(Pid);
            return Results.Ok();
        }
        private static async Task<IResult> SendDelPkgToServer(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).SendDelPkgToServer(Pid);
            return Results.Ok();
        }
        private static async Task<IResult> GetHistoryPkg(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).HistoriPkgByPid(Pid);
            return Results.Ok();
        } 
        private static async Task<IResult> GetAllPkg(PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).GetAll();
            return Results.Ok();
        }  
        private static async Task<IResult> GetPkgId(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).GetPkgId(Pid);
            return Results.Ok();
        }  
        private static async Task<IResult> DeletePkg(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).DeletePkg(Pid);
            return Results.Ok();
        } 
        private static async Task<IResult> GetDocsPkg(int Pid, PackagesServices pkgService)
        {
            await ((IPackagesServices)pkgService).GetDocsPkg(Pid);
            return Results.Ok();
        }
    }
}
