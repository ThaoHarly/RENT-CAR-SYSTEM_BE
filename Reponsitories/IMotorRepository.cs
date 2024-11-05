﻿using RentCarSystem.Models.Domain;

namespace RentCarSystem.Reponsitories
{
    public interface IMotorRepository
    {
        Task<Motor> CreateAsync(Motor motor);
        Task<List<Motor>> GetAllAsync();
        Task<Motor?> GetByIdAsync(Guid id);
        Task<Motor?> UpdateAsync(Guid id, Motor motor);
        Task<Motor?> DeleteAsync(Guid id);
    }
}
