
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Abstraction.Services;
using ClientSWH.Core.Models;


namespace ClientSWH.Application.Services
{
    public class StatusServices(IStatusRepositoty statusRepository) : IStatusServices
    {
        private readonly IStatusRepositoty _statusRepository = statusRepository;
        public async Task<string> AddStatus(Status status)
        {
             var resId = await _statusRepository.Add(status);
            if (resId>0)
                return resId.ToString();
            else return "статус не добавлен";
        }

        public async Task<string> DelStatus(int Id)
        {
            var status = await _statusRepository.GetById(Id);
            if (status != null)
            {
                 await _statusRepository.Delete(Id);
                status = await _statusRepository.GetById(Id);
                return (status == null)?"Статус удален": "Ошибка удаления статуса";
            }
            else return  "Ошибка удаления статуса"; 

        }
        public async Task<List<Status>> GetAllStatus()
        {
            var status = await _statusRepository.GetAllSt();
            if (status != null)
            {
                return status;
            }
            else return null;

        }

    }
}
