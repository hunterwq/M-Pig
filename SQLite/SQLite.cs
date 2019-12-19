using M_Pig.Controler;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DbSet<PigData> Pigs { get; set; }
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
    
    [Table("PigData")]
    public partial class PigData
    {
        [Key]
        public long MessageID { get; set; }
        public long PigSerial { get; set; }
        public DateTime Date { get; set; } = new DateTime();
        public int BatcherSum { get; set; }
        public int WaterSum { get; set; }
        public int Weight { get; set; }
        public int DeviceAddress { get; set; }
        public int RoomNum { get; set; }
        public int BatcherCalibration { get; set; }
        public int Threshold { get; set; }
        public int Days { get; set; }
    }
    public class SqliteOperate
    {

        public SqliteDbContext sqliteDb { get; set; }
        public SqliteOperate()
        {
            sqliteDb = new SqliteDbContext();
            var b = sqliteDb.Pigs.Where(p => p.PigSerial == 1).FirstOrDefault();
            
        }
        public void SqliteAddPig(PigData pig)
        {
            sqliteDb.Pigs.Add(pig);
            _ = sqliteDb.SaveChanges();
            //Console.WriteLine("ssssssssssssssssssssssssssssssssss");
        }


    }
}
