﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsItem>> GetNews();
    }
}