using MasterdevZad1.Data;
using MasterdevZad1.Models;
using MasterdevZad1.Pesel;
using Microsoft.AspNetCore.Mvc;

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
    }
}
