using MasterdevZad1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;

using HtmlAgilityPack;
using MasterdevZad1.ReadHTMLClassLibrary;

namespace MasterdevZad1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Zadania2345 zadanie2 = new Zadania2345();
        Zadania2345 zadanie3 = new Zadania2345();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Zadanie2()
        {
            string mystring = System.IO.File.ReadAllText(@"C:\Test\test_WOJ_MIE.txt");
            zadanie2.name = mystring;
            char ch = 'a';
            int freq = mystring.Count(f => (f == ch));
            zadanie2.liczba = freq;
            ViewBag.Name = zadanie2.name;
            ViewBag.Liczba = zadanie2.liczba;
            return View();
        }
        public IActionResult Zadanie3()
        {
            string mystring = System.IO.File.ReadAllText("C:/Test/praca.txt");
            ViewBag.praca = mystring;
            zadanie3.name = mystring;
            zadanie3.name = zadanie3.name.Replace("praca", "job");
            ViewBag.job = zadanie3.name;

            
            DateTime czas = DateTime.Now;
            czas = czas.Date;
            string czasy = czas.ToString();
            czasy = czasy.Remove(10);
            czasy = czasy.Replace("-", "");
            ViewBag.jobfilename = $"C:\\Test\\job_changed-{czasy}.txt";
            


            System.IO.File.WriteAllText($"C:/Test/job_changed-{czasy}.txt", zadanie3.name);



            return View();
        }
        public IActionResult Zadanie4()
        {
            Random rnd = new Random();
            string[] imie = {"Ania","Kasia", "Basia", "Zosia" };
            string[] nazwisko = { "Kowalska", "Nowak" };
            int rok_urodzenia = 0;
            ViewBag.imie = imie[0];
            ViewBag.rok_urodzenia = rok_urodzenia;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Lp,Imię,Nazwisko,Rok urodzenia");
            for (int row = 1; row < 101; row++)
            {
                int index1 = rnd.Next(imie.Length);
                int index2 = rnd.Next(nazwisko.Length);
                rok_urodzenia = rnd.Next(1990, 2000);
                stringBuilder.AppendLine($"{row},{imie[index1]},{nazwisko[index2]},{rok_urodzenia}");
            }
            string a = stringBuilder.ToString();
            DateTime czas = DateTime.Now;
            string czasy = czas.ToString();
            czasy = czasy.Replace("-", "_");
            czasy = czasy.Replace(":", "_");
            ViewBag.jobfilename2 = $"C:\\Test\\users-{czasy}.csv";
            System.IO.File.WriteAllText($"C:/Test/users-{czasy}.csv", a);


            return View();
        }
        
        
        public IActionResult Zadanie5()
        {
            
            ViewBag.stronka = ReadHTML.Send("https://www.nbp.pl/home.aspx?f=/kursy/kursya.html", "//td");
            ViewBag.zlotowki = 666;
            
            return View();
        }
        [HttpPost]
        public IActionResult Zadanie5wynik(Currency obj)
        {
            
            ViewBag.stronka = ReadHTML.Send("https://www.nbp.pl/home.aspx?f=/kursy/kursya.html", "//td");
            ViewBag.zlotowki = obj.zlotowki;
            obj.kursDolara = float.Parse(ViewBag.stronka);

            
            ViewBag.dolary = obj.zlotowki / obj.kursDolara;
            ViewBag.dolary = ViewBag.dolary.ToString("0.00");
            return View(obj);
        }

    }
}