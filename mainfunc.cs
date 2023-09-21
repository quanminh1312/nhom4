using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace baithucuaanhhuy
{
    internal static class mainfunc
    {
        
        static void listphim(List<Phim> phim, int trang=1, int dong=0)
        {
            if (dong==0) dong = phim.Count;
            try
            {
                Console.WriteLine("trang " + trang);
                for (int i = (trang - 1) * dong; i < trang * dong; i++)
                {
                    if (phim.Count > i) Console.WriteLine("ID phim: " + phim[i].IDPhim + " Ten Phim: " + phim[i].Name);
                    //Console.WriteLine("id phim: " + phim[i].IDPhim + " ten: " + phim[i].Name + " nam: " + phim[i].Year + " nha san xuat: " + phim[i].Director + " nuoc: " + phim[i].Country);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        static public void xuatthongtinphim(Phim phim)
        {
            Console.WriteLine("id phim: " + phim.IDPhim + " ten: " + phim.Name + " nam: " + phim.Year + " nha san xuat: " + phim.Director + " nuoc: " + phim.Country);
        }

        static public void nhapphim(Phim phim)
        {
            Console.Write("ID Phim: ");
            phim.IDPhim = Console.ReadLine()!;
            Console.Write("Ten Phim: ");
            phim.Name = Console.ReadLine()!;

            Console.Write("Nam san xuat Phim: ");
            phim.Year = int.Parse(Console.ReadLine()!).ToString();

            Console.Write("Dao dien Phim: ");
            phim.Director = Console.ReadLine()!;
            Console.Write("Nuoc san xuat Phim ( gom 2 ky tu quoc gia): ");
            phim.Country = Console.ReadLine()!;
        }
        static public void dataseeding(DB_connect dB_Connect)
        {
            try
            {
                List<Phim>? phims = new List<Phim>();
                using (StreamReader sr = new StreamReader("MOCK_DATA.json"))
                {
                    string json = sr.ReadToEnd();
                    phims = JsonSerializer.Deserialize<List<Phim>>(json);
                }
                foreach (Phim item in phims!)
                {
                    var t1 = dB_Connect.InsertPhim(item);
                }

                List<Rate>? rates = new List<Rate>();
                using (StreamReader sr = new StreamReader("tb_rate.json"))
                {
                    string json = sr.ReadToEnd();
                    rates = JsonSerializer.Deserialize<List<Rate>>(json);
                }
                foreach (Rate item in rates!)
                {
                    var t2 = dB_Connect.InsertRate(item);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        static public void chucnang1(DB_connect dB_Connect)
        {
            int trang = 1;
            int dong = 4;
            char chucnang = ' ';
            do
            {
                var t1 = dB_Connect.ReadPhim();
                t1.Wait();
                List<Phim> phim = t1.Result;
                int ket = (int)Math.Ceiling((float)phim.Count / dong);
                listphim(phim,trang, dong);
                Console.WriteLine("~~~~~~~~Danh Sach Cac Bo Phim~~~~~~~~~");
                Console.WriteLine("1.De xem trang tiep theo, bam phim N");
                Console.WriteLine("2.De quay lai trang truoc, bam phim P");
                Console.WriteLine("3.De tim kiem phim, bam phim C");
                Console.WriteLine("4.De quay lai menu chinh, bam phim B");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                try
                {
                    chucnang = char.Parse(Console.ReadLine()!.ToUpper());
                }
                catch { Console.WriteLine("nhap sai moi ban nhap lai"); }
                switch (chucnang)
                {
                    case 'N':
                        {
                            Console.Clear();
                            if (trang != ket) trang++;
                            break;
                        }
                    case 'P':
                        {
                            Console.Clear();
                            if (trang != 1) trang--;
                            break;
                        }
                    case 'B':
                        {
                            Console.Clear();
                            return;
                        }
                    case 'C':
                        {
                            Console.Clear();
                            chucnang4(dB_Connect);
                            ket = phim.Count / dong + phim.Count % dong;
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("nhap sai moi ban nhap lai");
                            break;
                        }
                }
            } while (chucnang != 'B');
        }

        static public void chucnang2(DB_connect dB_Connect)
        {
            try
            {
                Console.WriteLine("moi nhap thong tin phim: ");
                Phim phim = new Phim();
                nhapphim(phim);
                var t1 = dB_Connect.InsertPhim(phim);
                t1.Wait();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        static public void chucnang3(DB_connect dB_Connect)
        {
            int chucnang = 0;
            do
            {
                Console.WriteLine("~~~~~~~~Loc Phim Theo~~~~~~~~~");
                Console.WriteLine("1.De loc phim theo ten, bam phim 1");
                Console.WriteLine("2.De loc phim theo nam, bam phim 2");
                Console.WriteLine("3.De loc phim theo dao dien, bam phim 3");
                Console.WriteLine("4.De loc phim theo quoc gia, bam phim 4");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();
                try
                {
                    chucnang = int.Parse(Console.ReadLine()!);
                }
                catch { Console.WriteLine("nhap sai moi ban nhap lai"); }
                switch (chucnang)
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.Write("moi nhap ten: ");
                            try
                            {
                                string ten = Console.ReadLine()!;
                                var t1 = dB_Connect.ReadPhimsortten(ten);
                                t1.Wait();
                                List<Phim> list = t1.Result;
                                if (list == null) Console.WriteLine("khong co du lieu");
                                else listphim(list);
                                return;
                            }
                            catch { }
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.Write("moi nhap nam: ");
                            try
                            {
                                int nam = int.Parse(Console.ReadLine()!);
                                var t1 = dB_Connect.ReadPhimsortyear(nam);
                                t1.Wait();
                                List<Phim> list = t1.Result;
                                if (list == null) Console.WriteLine("khong co du lieu");
                                else listphim(list);
                                return;
                            }
                            catch { }
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            Console.Write("moi nhap dao dien: ");
                            try
                            {
                                string daodien = Console.ReadLine()!;
                                var t1 = dB_Connect.ReadPhimsortdirector(daodien);
                                t1.Wait();
                                List<Phim> list = t1.Result;
                                if (list == null) Console.WriteLine("khong co du lieu");
                                else listphim(list);
                                return;
                            }
                            catch { }
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Console.Write("moi nhap nuoc: ");
                            try
                            {
                                string country = Console.ReadLine()!.ToString();
                                var t1 = dB_Connect.ReadPhimsortcountry(country);
                                t1.Wait();
                                List<Phim> list = t1.Result;
                                if (list == null) Console.WriteLine("khong co du lieu");
                                else listphim(list);
                                return;
                            }
                            catch { }
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("nhap sai moi ban nhap lai");
                            break;
                        }
                }
            } while (true);
        }
        static public void chucnang4(DB_connect dB_Connect)
        {
            int chucnang = 0;
            string idphim = "";
            do
            {
                try
                {
                    Console.Write("moi nhap ID phim: ");
                    idphim = Console.ReadLine()!;
                    break;
                }
                catch (Exception e) { Console.WriteLine(e); }
            } while (true);
            do
            {
                try
                {
                    var t1 = dB_Connect.ReadPhimsortid(idphim);
                    t1.Wait();
                    Phim phim = t1.Result;
                    if (phim != null) xuatthongtinphim(phim);
                    else
                    {
                        Console.WriteLine("khong ton tai");
                        return;
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
                Console.WriteLine("~~~~~~~~chuc nang tim kiem phim~~~~~~~~~");
                Console.WriteLine("1.De chinh sua thong tin, bam phim 1");
                Console.WriteLine("2.De xoa bo phim, bam phim 2");
                Console.WriteLine("3.De them danh gia, bam phim 3");
                Console.WriteLine("4.De xem tat ca danh gia, bam phim 4");
                Console.WriteLine("5.De quay lai menu chinh, bam phim 5");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();
                try
                {
                    chucnang = int.Parse(Console.ReadLine()!);
                }
                catch { Console.WriteLine("nhap sai moi ban nhap lai"); }
                switch (chucnang)
                {
                    case 1:
                        {
                            Console.Clear();
                            var t1 = dB_Connect.updatePhim(idphim);
                            t1.Wait();
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            var t1 = dB_Connect.DeletePhim(idphim);
                            t1.Wait();
                            return;
                        }
                    case 3:
                        {
                            try
                            {
                                Console.Clear();
                                Rate rate = new Rate();
                                Console.Write("moi nhap dang gia: ");
                                rate.Danhgia = Console.ReadLine()!;
                                rate.IDPhim = idphim.ToString();

                                var t1 = dB_Connect.ReadDanhGia(idphim.ToString());
                                t1.Wait();
                                List<Rate> list = t1.Result;
                                rate.IDRate= (list.Count + 1).ToString();

                                var t2 = dB_Connect.InsertRate(rate);
                                t2.Wait();
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            var t1 = dB_Connect.ReadDanhGia(idphim);
                            t1.Wait();
                            List<Rate> list = t1.Result;
                            if (list == null) Console.WriteLine("chua co danh gia");
                            else
                                foreach (var item in list)
                                {
                                    Console.WriteLine("ID: " + item.IDRate + "Review: " + item.Danhgia);
                                }
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("nhap sai moi ban nhap lai");
                            break;
                        }
                }
            } while (chucnang != '5');
        }
    }
}
