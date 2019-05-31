﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Context;

namespace PlanningPoker
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PokerContext _dbContext;

        public UnitOfWork(PokerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Complete()
        {
             _dbContext.SaveChanges();
        }

    }
}
