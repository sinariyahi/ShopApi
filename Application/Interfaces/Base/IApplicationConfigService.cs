﻿using Infrastructure.Common;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Base
{
    public interface IApplicationConfigService
    {
        Task<ShopActionResult<int>> Update(ApplicationConfigDto model);
        Task<ShopActionResult<ApplicationConfigDto>> Get();
    }
}
