using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Data;
using DAL.Interface;
using DAL.Mapper;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CoursService : ICoursService
    {
        private readonly ICoursRepository _coursRepository;

        public CoursService(ICoursRepository coursRepository)
        {
            _coursRepository = coursRepository;
        }

        public async Task CreateAsync(CoursModel cours)
        {
            if (cours == null)
                throw new ArgumentNullException(nameof(cours));

            CoursData coursData = cours.ToCoursDAL();
            await _coursRepository.AddAsync(coursData);
        }

        public async Task DeleteAsync(int id)
        {
            await _coursRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CoursModel>> GetAvailableCoursesAsync()
        {
            var courses = await _coursRepository.GetAllAvailable();
            return courses.Select(c => c.ToCoursBLL());
        }

        public async Task<IEnumerable<CoursModel>> GetAllAsync()
        {
            var courses = await _coursRepository.GetAllAsync();
            return courses.Select(c => c.ToCoursBLL());
        }

        public async Task<CoursModel> GetByIdAsync(int id)
        {
            var course = await _coursRepository.GetByIdAsync(id);
            return course?.ToCoursBLL();
        }

        public async Task UpdateAsync(int id, CoursModel cours)
        {
            if (cours == null)
                throw new ArgumentNullException(nameof(cours));

            var existingCourse = await _coursRepository.GetByIdAsync(id);
            if (existingCourse == null)
                throw new KeyNotFoundException("Course not found.");

            existingCourse = cours.ToCoursDAL();
            await _coursRepository.UpdateAsync(existingCourse);
        }

        //public async Task InsertUserCourseAsync(CoursModel cours, UsersModel user)
        //{
        //    if (cours == null)
        //        throw new ArgumentNullException(nameof(cours));

        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user));

        //    CoursData coursData = cours.ToCoursDAL();
        //    UsersData userData = user.ToUserDAL();

        //    await _coursRepository.InsertUserCourseAsync(coursData, userData);
        //}

        //public async Task EnrollUserInCourseAsync(CoursModel cours, UsersModel user)
        //{
        //    await InsertUserCourseAsync(cours, user);
        //}

        public Task<IEnumerable<CoursModel>> GetAllAvailble()
        {
            throw new NotImplementedException();
        }

       

        public void Create(CoursModel cours)
        {
            throw new NotImplementedException();
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoursModel>> GetallByUser(int id)
        {
            // Appel à la DAL pour obtenir les données
            var coursData = await _coursRepository.GetallByUser(id);

            // Utilisation d'AutoMapper pour convertir les entités en modèles
            var coursModels = coursData.Select(x => x.ToCoursBLL());

            return coursModels;
        }

        //public async Task InsertUserCourseAsync(int userId, int courseId)
        //{
        //    if (userId <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");

        //    if (courseId <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero.");

        //    try
        //    {
        //        // You may need to implement a method to map userId and courseId to the corresponding entities
        //        await _coursRepository.InsertUserCourseAsync()
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        Console.WriteLine($"Error inserting user course: {ex.Message}");

        //        // Optionally, rethrow the exception if it should be handled by a higher-level handler
        //        throw;
        //    }
        //}
    }
}
