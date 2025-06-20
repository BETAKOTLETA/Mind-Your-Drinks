﻿using Mind_Your_Drink_Models.Data.Repositories;

namespace Mind_Your_Drink_Models.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAdminRepository Admins { get; }
        IUserDrinksRepository UserDrinks { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
