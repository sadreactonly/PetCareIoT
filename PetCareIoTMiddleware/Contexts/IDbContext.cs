using Microsoft.EntityFrameworkCore;
using PetCareIoTMiddleware.Models;
using System;

namespace PetCareIoTMiddleware.Authentication
{
    public interface IDbContext : IDisposable
    {
        DbSet<BaseEvent> Events { get; set; }
        //DbSet<FeederEvent> FeederEvents { get; set; }
        //DbSet<PumpEvent> PumpEvents { get; set; }
        //DbSet<LightEvent> LightEvents { get; set; }

        int SaveChanges();

        void MarkAsModified(Object item);
    }
}