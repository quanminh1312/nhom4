using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
namespace baithucuaanhhuy
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DB_connect dB_Connect = new DB_connect();
            bool t1=false;
            try
            {
                t1 =await dB_Connect.CreateDatabase();
                //await dB_Connect.DeleteDatabase();
                //t1 = await dB_Connect.CreateDatabase();
            }
            catch (Exception e) { Console.WriteLine(e); }
            { }
            if (t1)
            try
            {
                List<Phim>? phims = new List<Phim>();
                using (StreamReader sr = new StreamReader("MOCK_DATA.json"))
                {
                    string json = sr.ReadToEnd();
                    phims = JsonSerializer.Deserialize<List<Phim>>(json);
                }
                foreach (var item in phims)
                {
                    await dB_Connect.InsertPhim(item);
                }

                List<Rate>? rates = new List<Rate>();
                using (StreamReader sr = new StreamReader("tb_rate.json"))
                {
                    string json = sr.ReadToEnd();
                    rates = JsonSerializer.Deserialize<List<Rate>>(json);
                }
                foreach (var item in rates)
                {
                    await dB_Connect.InsertRate(item);
                }
                }
            catch (Exception e) { Console.WriteLine(e); }
            int? chucnang = 0;
            do
            {
                Console.WriteLine("~~~~~~~~phan mem danh gia phim~~~~~~~~~");
                Console.WriteLine("1.xem danh sach cac bo phim, bam phim 1");
                Console.WriteLine("2.Thêm bo phim moi, bam phim 2");
                Console.WriteLine("3.loc phim , bam phim 3");
                Console.WriteLine("4.thoat, bam phim 4" );
                try
                {
                    chucnang = int.Parse(Console.ReadLine());
                }   catch { Console.WriteLine("nhap sai moi ban nhap lai"); }
                switch (chucnang)
                {
                    case 1:
                        {
                            Console.Clear();
                            mainfunc.chucnang1(dB_Connect);
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            mainfunc.chucnang2(dB_Connect);
                            break;
                        }
                    case 3:
                        {   
                            Console.Clear() ;
                            mainfunc.chucnang3(dB_Connect);
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("nhap sai moi ban nhap lai");
                            break;
                        }
                }
            } while (chucnang!=4);
        }
    }
}