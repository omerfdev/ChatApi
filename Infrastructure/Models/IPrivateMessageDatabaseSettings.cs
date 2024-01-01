﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public interface IPrivateMessageDatabaseSettings
    {
        string PrivateMessageCollectionName { get; set; } 
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
