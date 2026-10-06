
using BookManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private static readonly List<Book> _books = new()
        {
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", PublishedYear = 1925, Genre = "Classic Fiction" },
            new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084", PublishedYear = 1960, Genre = "Drama" }
        };

        // GET: Books
        // Updated to support searching/filtering down the list
        public IActionResult Index(string searchString)
        {
            var filteredBooks = _books.AsEnumerable();

            if (!string.IsNullOrEmpty(searchString))
            {
                filteredBooks = filteredBooks.Where(s => s.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                                      || s.Author.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            return View(filteredBooks.ToList());
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
            // UPDATE 2: Custom validation handling to block invalid dates/empty titles
            if (book.PublishedYear > DateTime.Now.Year)
            {
                ModelState.AddModelError("PublishedYear", "The publishing year cannot be in the future.");
            }

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                ModelState.AddModelError("Title", "Book title field cannot contain empty whitespaces.");
            }

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
            if (updatedBook.PublishedYear > DateTime.Now.Year)
            {
                ModelState.AddModelError("PublishedYear", "The publishing year cannot be in the future.");
            }

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

        // NEW CHANGE: Export entire book list catalog data as a downloadable CSV file
        public IActionResult ExportToCSV()
        {
            var csvBuilder = new System.Text.StringBuilder();

            // Write CSV Column Headers
            csvBuilder.AppendLine("ID,Title,Author,ISBN,Published Year,Genre");

            // Loop through current list items and format row columns
            foreach (var book in _books)
            {
                csvBuilder.AppendLine($"{book.Id},\"{book.Title.Replace("\"", "\"\"")}\",\"{book.Author.Replace("\"", "\"\"")}\",'{book.ISBN},{book.PublishedYear},\"{book.Genre.Replace("\"", "\"\"")}\"");
            }

            // Convert string buffer data into a raw byte array stream for download execution
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(fileBytes, "text/csv", "Library_Collection_Export.csv");
        }

    }
}
