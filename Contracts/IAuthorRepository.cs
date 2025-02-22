﻿using BookStore_API.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Contracts
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
    }
}
