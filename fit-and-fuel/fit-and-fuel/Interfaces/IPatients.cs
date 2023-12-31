﻿using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IPatients
    {
        Task<List<Patient>> GetAll();
        Task<List<PatientDtoView>> GetAllDto();
        Task<Patient> GetById(int id);
        Task<Patient> GetMyProfile();
        Task<PatientDtoView> GetByIdDto(int id);

        Task SelectNut(int NutId, int UserId);
        Task<List<Patient>> Search(string str);
        Task<List<PatientDtoView>> SearchDto(string str);
        Task<DietPlan> GetMyDietPlan();
        Task<DietPlanDtoView> GetMyDietPlanDto(int UserId);
        Task<List<Meal>> GetMyMealsForToday();
       


        Task MealIsCompletion( int MealId);

        Task<Patient> Post(PatientDto patientDto, IFormFile file);

        
        Task Put(int id, PatientDto patientDto);
        Task Delete(int id);
		Task<int> Count();

	}
}
