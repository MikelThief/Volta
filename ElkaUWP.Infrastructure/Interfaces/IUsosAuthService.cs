﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.Infrastructure.Interfaces
{
    public interface IUsosAuthService
    {
        Task AuthorizeAsync();
    }
}