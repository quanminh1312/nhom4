using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baithucuaanhhuy
{
    internal class DB_connect : DbContext
    {
        public DbSet<Phim> Phims { set; get; }
        public DbSet<Rate> Rates { set; get; }

        /// <summary>
        /// sua connection string 
        /// </summary>
        private const string connectionString = @"Data Source= MSI;Initial Catalog=DANHGIAPHIM;Integrated Security=True;Encrypt=False; Trusted_Connection=True
                                                 ;TrustServerCertificate=True";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Các Fluent API
            modelBuilder.Entity<Phim>(entity =>
            {

                // Gọi các API từ đối tượng entity để xây dựng bảng phim
                entity.ToTable("TB_Phim");
                entity.HasKey(e => e.IDPhim);


                entity.Property(p => p.IDPhim).HasColumnType("char(4)").HasColumnName("IDPhim");
                entity.Property(p => p.Name).HasColumnName("TenPhim").HasColumnType("nvarchar(50)");
                entity.Property(p => p.Country).HasColumnName("Nuoc").HasDefaultValue("VN").HasColumnType("varchar(2)");
                entity.Property(p => p.Director).HasColumnName("NhaSanXuat").HasColumnType("nvarchar(30)");
                entity.Property(p => p.Year).HasColumnName("Nam").HasColumnType("varchar(4)").HasDefaultValue(DateTime.Now.Year.ToString());

            });
            modelBuilder.Entity<Rate>(entity =>
            {

                // Gọi các API từ đối tượng entity để xây dựng bảng rate
                entity.ToTable("TB_Rate").HasKey(e => e.IDRate);
                entity.HasOne(p => p.Phim)
                      .WithMany(r => r.rates)
                      .HasForeignKey(p => p.IDPhim);

                entity.Property(p => p.IDRate).HasColumnType("char(6)").HasColumnName("IDRate");
                entity.Property(p => p.IDPhim).HasColumnType("char(4)").HasColumnName("IDPhim");
                entity.Property(p => p.Danhgia).HasDefaultValue("khong co danh gia").HasColumnType("nvarchar(200)").HasColumnName("DanhGia");



            });
        }
        #region tao/xoa database
        public async Task<bool> CreateDatabase()
        {
            using (var dbcontext = new DB_connect())
            {
                String databasename = dbcontext.Database.GetDbConnection().Database;// mydata

                //Console.WriteLine("create " + databasename);

                bool result = await dbcontext.Database.EnsureCreatedAsync();
                return result;
                //string resultstring = result ? "tao thanh cong" : "da co truoc do";
                //Console.WriteLine($"CSDL {databasename} : {resultstring}");
            }
        }

        public async Task DeleteDatabase()
        {

            using (var context = new DB_connect())
            {
                String databasename = context.Database.GetDbConnection().Database;
                //Console.Write($"co chac chan xoa {databasename} (y) ? ");
                //string input = Console.ReadLine();

                //// Hỏi lại cho chắc
                //if (input.ToLower() == "y")
                //{
                bool deleted =
                await context.Database.EnsureDeletedAsync();
                string deletionInfo = deleted ? "da xoa" : "khong xoa duoc";
                Console.WriteLine($"{databasename} {deletionInfo}");
                //}
            }

        }
        #endregion

        #region them phim / danhgia
        public async Task InsertPhim(Phim t)
        {
            using (var context = new DB_connect())
            {
                // Thêm phim
                await context.Phims.AddAsync(t);
                // Thực hiện cập nhật thay đổi trong DbContext lên Server
                int rows = await context.SaveChangesAsync();
                Console.WriteLine($"da luu phim");

            }
        }
        public async Task InsertRate(Rate t)
        {
            using (var context = new DB_connect())
            {
                // Thêm danh gia
                await context.Rates.AddAsync(t);
                // Thực hiện cập nhật thay đổi trong DbContext lên Server
                int rows = await context.SaveChangesAsync();
                Console.WriteLine($"da luu danh gia");

            }
        }
        #endregion

        #region read thong tin
        public async Task<List<Phim>> ReadPhim()
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await context.Phims.ToListAsync();
                return phims;
            }
        }
        public async Task<List<Phim>> ReadPhimsortten(string ten)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await (from p in context.Phims
                                   where (p.Name!.Contains(ten.ToLower()))
                                   select p
                                 )
                                .ToListAsync();
                return phims;
            }
        }
        public async Task<List<Phim>> ReadPhimsortyear(int year)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await (from p in context.Phims
                                   where (string.Compare(p.Year, year.ToString()) == 0)
                                   select p
                                 )
                                .ToListAsync();
                return phims;
            }
        }
        public async Task<List<Phim>> ReadPhimsortdirector(string director)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await (from p in context.Phims
                                   where (p.Director!.Contains(director.ToLower()))
                                   select p
                                 )
                                .ToListAsync();
                return phims;
            }
        }
        public async Task<List<Phim>> ReadPhimsortcountry(string country)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await (from p in context.Phims
                                   where (string.Compare(p.Country, country.ToLower()) == 0)
                                   select p
                                 )
                                .ToListAsync();
                return phims;
            }
        }
        public async Task<Phim> ReadPhimsortid(string id)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var phims = await (from p in context.Phims
                                   where (string.Compare(p.IDPhim, id) == 0)
                                   select p
                                 )
                                .FirstOrDefaultAsync();
                return phims!;
            }
        }
        public async Task<List<Rate>> ReadDanhGia(string IDPhim)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                // Lấy danh sách các sản phẩm trong bảng 
                var danhgia = await (from p in context.Rates
                                     where (string.Compare(p.IDPhim, IDPhim) == 0)
                                     select p
                                 )
                                .ToListAsync();
                return danhgia;
            }
        }
        #endregion

        #region cap nhap phim
        public async Task updatePhim(string id)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                var phim = await (from p in context.Phims where (string.Compare(p.IDPhim, id) == 0) select p).FirstOrDefaultAsync();

                if (phim != null)
                {
                    try
                    {
                        mainfunc.xuatthongtinphim(phim);
                        mainfunc.nhapphim(phim);
                        await context.SaveChangesAsync();
                        Console.WriteLine("da cap nhat phim");
                    }
                    catch(Exception e) { Console.WriteLine(e); }
                }
            }
        }
        #endregion

        #region xoa phim 
        public async Task DeletePhim(string id)
        {
            using (var context = new DB_connect())
            {
                // context.SetLogging();
                var phim = await (from p in context.Phims where (string.Compare(p.IDPhim, id) == 0) select p).FirstOrDefaultAsync();

                // Hỏi lại cho chắc
                Console.Write($"co chac chan xoa (y/n) ? ");
                string input = Console.ReadLine()!;
                if (phim != null)
                {
                    if (input.ToLower() == "y")
                    {
                        context.Remove(phim);
                        Console.WriteLine($"Xoa {phim.IDPhim}");
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        #endregion

    }
}
