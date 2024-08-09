using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AssignementsService : IAssignementsService
    {
        private readonly IAssignementsRepository _assignementsRepository;

        public AssignementsService(IAssignementsRepository assignementsRepository)
        {
            _assignementsRepository = assignementsRepository;
        }

        public async Task<IEnumerable<AssignementsModel>> GetAll()
        {
            var data = await _assignementsRepository.GetAll();
            return data.Select(d => d.ToBLL());
        }

        public async Task<AssignementsModel> GetbyId(int id)
        {
            var data = await _assignementsRepository.GetbyId(id);
            return data?.ToBLL();
        }

        public async Task<IEnumerable<AssignementsModel>> GetAllByCourse(int courseId)
        {
            var data = await _assignementsRepository.GetAllByCourse(courseId);
            return data.Select(d => d.ToBLL());
        }

        public async Task<IEnumerable<AssignementsModel>> GetAllByUser(int userId)
        {
            var data = await _assignementsRepository.GetAllByUser(userId);
            return data.Select(d => d.ToBLL());
        }

        public async Task<IEnumerable<AssignementsModel>> GetAllByTeacher(int userId)
        {
            var data = await _assignementsRepository.GetAllByteacher(userId);
            return data.Select(d => d.ToBLL());
        }

        public async Task Insert(AssignementsModel assignementsModel)
        {
            var data = assignementsModel.AssignementsToDAL();
            await _assignementsRepository.Insert(data);
        }

        public async Task Update(AssignementsModel assignementsModel)
        {
            var data = assignementsModel.AssignementsToDAL();
            await _assignementsRepository.Update(data);
        }

        public async Task Delete(int id)
        {
            await _assignementsRepository.Delete(id);
        }

      

      
      
    }
}
