﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class UserDatabaseSettings : IUserDatabaseSettings
    {
        public string UserCollectionName { get; set; } = "User";
        public string ConnectionString { get; set; } = "mongodb+srv://omerfdev:Admin1234@cluster0.xsev4x8.mongodb.net/?retryWrites=true&w=majority";
        public string DatabaseName { get; set; } = "ChatApps";
    }
}