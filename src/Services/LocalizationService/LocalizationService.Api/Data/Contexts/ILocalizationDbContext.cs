﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using LocalizationService.Api.Entities;
using LocalizationService.Api.Entities.Abstract;

namespace LocalizationService.Api.Data.Contexts
{
    public interface ILocalizationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Language> Languages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        string GetTableNameWithScheme<T>() where T : class, IEntity;
    }
}
