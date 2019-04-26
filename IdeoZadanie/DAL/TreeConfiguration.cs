using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace IdeoZadanie.DAL
{
    public class TreeConfiguration : DbConfiguration
    {
        public TreeConfiguration() {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}