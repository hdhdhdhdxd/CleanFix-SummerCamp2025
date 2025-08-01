﻿namespace Application.Common.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        T Get(int id);

        void Add(T entity);

        Task Remove(T entity);
    }
}
