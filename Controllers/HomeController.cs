using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        //GET:/HOME/
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(dbContext.Dishes.OrderByDescending(d => d.CreatedAt).ToList());
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                // stages an insert!
                dbContext.Dishes.Add(newDish);
                // executes the insert and creates dish in Db
                dbContext.SaveChanges();
 
                return RedirectToAction("Index");
            }
            //if error in validations will return View 
            return View("New");
        }

        [HttpGet("{dishId}")]
        public IActionResult Show(int dishId)
        {
            // get a dish with dish id
            //oneDish pulls info from Dishes table
            //lamda expression (d => d.DishId == dishId) dish with dishId == equal to {dishId} - week 6 lec 2 30:00
            Dish oneDish = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishId);

            //redirect to Index page if oneDish does not exist
            if(oneDish == null)
            {
                return RedirectToAction("Index");
            }
            return View(oneDish);
        }

        [HttpGet("edit/{dishId}")]
        public IActionResult Edit(int dishId)
        {
            //query db for dish with dishId
            Dish thisDish = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishId);

            if(thisDish == null)
            {
                return RedirectToAction("Index");
            }
            return View(thisDish);
        }

        [HttpPost("update/{dishId}")]
        public IActionResult Update(Dish dish, int dishId)
        {
            if(ModelState.IsValid)
            {
                Dish toUpdate = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishId);
                toUpdate.Name = dish.Name;
                toUpdate.Chef = dish.Chef;
                toUpdate.Description = dish.Description;
                toUpdate.Tastiness = dish.Tastiness;
                toUpdate.Calories = dish.Calories;
                toUpdate.UpdatedAt = DateTime.Now;

                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", dish);
        }

        [HttpGet("delete/{dishId}")]
        public IActionResult Delete(int dishId)
        {
            //query db for dish with dishId, just like Edit
            Dish toDelete = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishId); 

            if(toDelete == null)
            {
                return RedirectToAction("Index");
            }
            dbContext.Dishes.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
