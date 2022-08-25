using ClosedXML.Excel;
using ExcelDataReader;
using MasterdevZad1.Data;
using MasterdevZad1.Models;
using MasterdevZad1.Pesel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using System.IO;

namespace MasterdevZad1.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TestController(ApplicationDbContext db)
        {
            _db =  db;
        }

        public IActionResult Index()
        {
            IEnumerable<Klienci> objKlienciList = _db.Klienci;
            return View(objKlienciList);
        }
        //GET
        public IActionResult Create()
        {
            
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Klienci obj)
        {
            if(!PeselValidator.ValidatePesel(obj.PESEL))
            {
                ModelState.AddModelError("pesel", "Nieprawidłowy PESEL");
            }
            
            

            if (obj.Name == obj.Surname.ToString())
            {
                ModelState.AddModelError("name", "Pole Name i Surname muszą być różne");
            }
            if(ModelState.IsValid)
            {
                obj.Płeć = int.Parse(obj.PESEL.Substring(9, 1));
                if (int.Parse(obj.PESEL.Substring(0, 2)) > 22) obj.BirthYear = int.Parse(obj.PESEL.Substring(0, 2)) + 1900;
                else obj.BirthYear = int.Parse(obj.PESEL.Substring(0, 2)) + 2000;
                _db.Klienci.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            var KlientfromDb = _db.Klienci.Find(id);
            

            if (KlientfromDb == null)
            {
                return NotFound();
            }
            return View(KlientfromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Klienci obj)
        {
            if (obj.Name == obj.Surname.ToString())
            {
                ModelState.AddModelError("name", "Pole Name i Surname muszą być różne");
            }
            if (ModelState.IsValid)
            {
                _db.Klienci.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var KlientfromDb = _db.Klienci.Find(id);
            //var KlientfromDbFirst = _db.Klienci.FirstOrDefault(u => u.Id == id);
            //var KlientfromDbSingle = _db.Klienci.SingleOrDefault(u => u.Id == id);

            if (KlientfromDb == null)
            {
                return NotFound();
            }
            return View(KlientfromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Klienci.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Klienci.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
            
            

        }
        //GET
        public IActionResult Export()
        {
            return View();
        }
        //POST
        
        public IActionResult ExportCSV()
        {
            IEnumerable<Klienci> objKlienciList = _db.Klienci;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Name,Surname,Pesel,Rok urodzenia,Płeć");
            foreach (var row in objKlienciList)
            {
                stringBuilder.AppendLine($"{row.Name},{row.Surname},{row.PESEL},{row.BirthYear},{row.Płeć}");
            }
            return File(System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "DatabaseToCSVExport.csv");
        }
        public IActionResult ExportXLSX()
        {

            IEnumerable<Klienci> objKlienciList = _db.Klienci;
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Klienci");
            ws.Cell(2, 1).Value = "Name";
            ws.Cell(2, 2).Value = "Surname";
            ws.Cell(2, 3).Value = "PESEL";
            ws.Cell(2, 4).Value = "BirthYear";
            ws.Cell(2, 5).Value = "Płeć";

            int i = 3;
            foreach(var row in objKlienciList)
            {
                ws.Cell(i, 1).Value = row.Name;
                ws.Cell(i, 2).Value = row.Surname;
                ws.Cell(i, 3).Value = row.PESEL;
                ws.Cell(i, 4).Value = row.BirthYear;
                ws.Cell(i, 5).Value = row.Płeć;
                i = i + 1;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet", "Klienci.xlsx"); 
        }

        public IActionResult Import()
        {
            
            
            return View();
        }
        [HttpPost]
        public IActionResult Import(ImportFile obj, Klienci baza)
        {
            if (obj.Plik == null)
            {
                ViewBag.Plik = "Wybierz plik";
            }
            else 
            {
                string nazwa_pliku = obj.Plik.FileName;
                if (nazwa_pliku.Contains(".csv"))
                {
                    ViewBag.Plik = obj.Plik.FileName;
                    var reader = new StreamReader(obj.Plik.OpenReadStream());

                    string row;
                    List<Klienci> deps = new List<Klienci>();
                    row = reader.ReadLine();
                    while ((row = reader.ReadLine()) != null)
                    {
                        string[] element = row.Split(",");
                        deps.Add(new Klienci { Name = element[0], Surname = element[1], PESEL = element[2], BirthYear = Convert.ToInt32(element[3]), Płeć = Convert.ToInt32(element[4]) });
                    }
                    _db.Klienci.AddRange(deps);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (nazwa_pliku.Contains(".xlsx"))
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    
                    var sdf = obj.Plik.OpenReadStream();


                    var reader = ExcelReaderFactory.CreateReader(sdf);
                    List<Klienci> deps = new List<Klienci>();

                    reader.Read();
                    reader.Read();
                    
                    //ViewBag.Plik = reader.GetValue(0);
                    while (reader.Read()) //Each row of the file
                    {


                        deps.Add(new Klienci { Name = reader.GetValue(0).ToString(), Surname = reader.GetValue(1).ToString(), PESEL = reader.GetValue(2).ToString(), BirthYear = Convert.ToInt32(reader.GetValue(3).ToString()), Płeć = Convert.ToInt32(reader.GetValue(4).ToString()) });

                    }
                    _db.Klienci.AddRange(deps);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else ViewBag.Plik = "Nieprawidłowy plik";






            }


            return View(obj);
        }
    }
}
