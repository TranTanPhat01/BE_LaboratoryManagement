﻿using Patient_service.Models;
using Patient_service.Repositories.Interface;
using Service.Interface_service.Repositories.Interface;
using Models.Dto;
using Service.GrpcService;
using Patient_service.Models.Dto;

namespace Service.Implement
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IamGrpcService _iamGrpcService;
        public PatientService(IPatientRepository repository, IamGrpcService iamGrpcService)
        {
            _repository = repository;
            _iamGrpcService = iamGrpcService;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Patient?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Patient?> GetByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }

        public async Task<Patient?> AddAsync(PatientDto patient)
        {
           return  await _repository.AddAsync(patient);
        }
        public async Task CreateAccountbyEmailAsync(PatientDto patient)
        {
            // 1. Lưu patient vào DB
           

            await _repository.CreateAccountByPatient(patient);

            if (!string.IsNullOrEmpty(patient.Email))
            {
                await _iamGrpcService.QuickRegisterAsync(patient.Email);
            }
        }

        public async Task UpdateAsync(PatientUpdate patient)
        {
            await _repository.UpdateAsync(patient);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public Task<List<Patient>> SearchAsync(string key)
        {
            return _repository.SearchAsync(key);
        }

        public Task<Patient?> GetByEmailAsync(string email)
        {
            return _repository.GetByEmailAsync(email);
        }
    }
}
