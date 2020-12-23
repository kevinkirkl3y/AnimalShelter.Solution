using Microsoft.AspNetCore.Mvc;
using AnimalShelter.Models;
using System.Collections.Generic;
using System.Linq; // LINQ is short for Language-Integrated Query

namespace AnimalShelter.Controllers
{
  public class AnimalsController : Controller
  {
    private readonly AnimalShelterContext _db;

    public AnimalsController(AnimalShelterContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Animal> model = _db.Animals.ToList(); // this replaces GetAll()
      return View(model);

// db is an instance of our DbContext class. It's holding a reference to our database
// Once there, it looks for an object named Items. This is the DbSet we declared in ToDoListContext.cs
// LINQ turns this DbSet into a list using the ToList() method, which comes from the System.Linq namespace
// This expression is what creates the model we'll use for the Index view
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Animal animal)
    {
      _db.Animals.Add(animal);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Details(int id)
    {
        Animal thisAnimal = _db.Animals.FirstOrDefault(animals => animals.AnimalId == id);
        return View(thisAnimal);
    }
    public async Task<IActionResult> Index(string searchString)
    {
      var animals = from a in _db.Animals
            select a;

      if (!String.IsNullOrEmpty(searchString))
      {
        animals = animals.Where(s => s.Type.Contains(searchString));
      }
      return View(await animals.ToListAsync());
    }

  }
}