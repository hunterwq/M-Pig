using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_Pig.SQLite
{

    public class SqliteDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public SqliteDbContext() : base("connectionSQLite")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //清除自动生成的数据表名被复数的问题
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            //modelBuilder.Entity<Employee>().ToTable("Employee");
        }
       
    }
    [Table("Employee")]
    public partial class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
}
