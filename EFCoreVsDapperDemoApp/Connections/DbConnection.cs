using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreVsDapperDemoApp.Connections
{
    public static class DbConnection
    {
        public static string GetConnectionString()
        {
            // NOTE: This is my local DB string which I had set up with Northwind sample template by Microsoft
            // Make sure to update the path with yours if you're running locally ;) 
            return "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";
        }
    }
}