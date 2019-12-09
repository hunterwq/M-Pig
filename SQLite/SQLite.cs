using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
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
            ConfigurationFunc();
        }
        private void ConfigurationFunc()
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }
        /// <summary>
        /// 代码指定数据库连接
        /// </summary>
        /// <param name="existingConnection"></param>
        /// <param name="contextOwnsConnection"></param>
        public SqliteDbContext(DbConnection existingConnection, bool contextOwnsConnection) :
            base(existingConnection, contextOwnsConnection)
        {
            ConfigurationFunc();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //清除自动生成的数据表名被复数的问题
            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            //modelBuilder.Entity<Employee>().ToTable("Employee");
            var initializer = new SqliteDropCreateDatabaseWhenModelChanges<SqliteDbContext>(modelBuilder);
            Database.SetInitializer(initializer);
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
