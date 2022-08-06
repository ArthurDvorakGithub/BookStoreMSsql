using BookStoreApp.Data;
using BookStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index(Sorting sortOrder = Sorting.NameAsc)
        {
            IQueryable<Book> books = _db.Books.Include(x => x.Author);

            ViewData["NameSort"] = sortOrder == Sorting.NameAsc ? Sorting.NameDesc : Sorting.NameAsc;
            ViewData["AgeSort"] = sortOrder == Sorting.AgeAsc ? Sorting.AgeDesc : Sorting.AgeAsc;
            ViewData["AuthorSort"] = sortOrder == Sorting.AuthorAsc ? Sorting.AuthorDesc : Sorting.AuthorAsc;

            books = sortOrder switch
            {
                Sorting.NameDesc => books.OrderByDescending(s => s.Name),
                Sorting.AgeAsc => books.OrderBy(s => s.Age),
                Sorting.AgeDesc => books.OrderByDescending(s => s.Age),
                Sorting.AuthorAsc => books.OrderBy(s => s.Author.Name),
                Sorting.AuthorDesc => books.OrderByDescending(s => s.Author.Name),
                _ => books.OrderBy(s => s.Name),
            };

            ViewData["Count"] = _db.Books.Where(u => u.ExistInShopCart == true).Count();
            return View(await books.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Buy(int id)
        {
            IQueryable<Book> books = _db.Books.Include(x => x.Author);

            if (id != 0)
            {
                var booksBuy = _db.Books.Find(id);
                booksBuy.ExistInShopCart = true;
                _db.SaveChanges();
            }

            ViewData["Count"] = _db.Books.Where(u => u.ExistInShopCart == true).Count();
            return View( await books.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Replace(int id)
        {
            IQueryable<Book> books = _db.Books.Include(x => x.Author);

            if (id != 0)
            {
                var booksBuy = _db.Books.Find(id);
                booksBuy.ExistInShopCart = false;
                _db.SaveChanges();
            }

            ViewData["Count"] = _db.Books.Where(u => u.ExistInShopCart == true).Count();
            return View("Buy",await books.AsNoTracking().ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
