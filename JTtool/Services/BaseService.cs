using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JTtool.Models;
using JTtool.Models.Entity;

namespace JTtool.Services
{
    public class BaseService
    {
        public JTtoolDBEntities db = new JTtoolDBEntities();
    }
}