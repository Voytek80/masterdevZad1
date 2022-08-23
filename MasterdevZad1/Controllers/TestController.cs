using ClosedXML.Excel;
using MasterdevZad1.Data;
using MasterdevZad1.Models;
using MasterdevZad1.Pesel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

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
            
            System.IO.File.ReadAllText(@"C:\Test\test_WOJ_MIE.txt");
            return View();
        }
    }
}
