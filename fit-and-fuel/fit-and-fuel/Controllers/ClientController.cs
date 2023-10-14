﻿using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class ClientController : Controller
    {
		private readonly IPatients _patients;

        public ClientController(IPatients patients)
        {
            _patients = patients;
        }
		[Authorize(Roles = "Patient")]
		public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MyProfile()
        {
            var myprofile = await _patients.GetMyProfile();
            return View(myprofile);
        }
        [Authorize(Roles = "Patient")]
		[HttpPost]
        public async Task<IActionResult> Create(PatientDto patientDto, IFormFile file)
        {
            ModelState.Remove("file");


            await _patients.Post(patientDto, file);
            return RedirectToAction("Index", "Home");
        }
    }
}
