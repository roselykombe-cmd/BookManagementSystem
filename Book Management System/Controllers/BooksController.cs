using Book_Management_System.Models;
using BookManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book_Management_System.Controllers
{
    public class BooksController : Controller
    {
        private static readonly List<Book> _books = new()
        {
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", PublishedYear = 1925, Genre = "Classic Fiction" },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084", PublishedYear = 1960, Genre = "Drama" }
        };

        // GET: Books
        public IActionResult Index()
        {
            return View(_books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
                _books.Add(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book updatedBook)
        {
            if (ModelState.IsValid)
            {
                var existingBook = _books.FirstOrDefault(b => b.Id == id);
                if (existingBook != null)
                {
                    existingBook.Title = updatedBook.Title;
                    existingBook.Author = updatedBook.Author;
                    existingBook.ISBN = updatedBook.ISBN;
                    existingBook.PublishedYear = updatedBook.PublishedYear;
                    existingBook.Genre = updatedBook.Genre;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedBook);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
