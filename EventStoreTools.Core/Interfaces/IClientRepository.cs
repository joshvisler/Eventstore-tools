﻿using EventStoreTools.Core.Entities;
using System;
using System.Threading.Tasks;

namespace EventStoreTools.Core.Interfaces
{
    public interface IClientRepository : IDisposable
    {
        Client GetByLogin(string login);
        Client GetById(Guid id);
        Task<Client> GetByLoginAsync(string login);
        Client Insert(Client client);
        Task<Client> InsertAsync(Client client);
        void Delete(Client client);
        void Update(Client client);
        Task DeleteAsync(Client client);
        Task UpdateAsync(Client client);
    }
}
