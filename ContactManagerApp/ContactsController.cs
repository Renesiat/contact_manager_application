using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManagerApp.Models;

namespace ContactManagerApp
{
    public class ContactsController : Controller
    {
        private readonly ContactManagerDbContext _context;

        public ContactsController(ContactManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateOfBbirth,Married,Phone,Salary")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("Contacts/UploadCsv")]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {

            if (csvFile == null)
            {
                ModelState.AddModelError("csvFile", "Select a file");
                return View("Index", await _context.Contacts.ToListAsync());
            }

            try
            {
                Contact? contact = null;

                using (var stream = new StreamReader(csvFile.OpenReadStream()))
                {
                    string? line;

                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        var values = line.Split(',');
                        if (values.Length == 0)
                        {
                            ModelState.AddModelError("", $"File is empty");
                            continue;
                        }

                        var name = values[0].Trim();
                        var married = values[2].Trim().ToLower() == "yes";
                        var phone = values[3].Trim();

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            ModelState.AddModelError("", $"Name is required");
                            continue;
                        }

                        if (!DateTime.TryParse(values[1].Trim(), out var dob) || dob > DateTime.Now)
                        {
                            ModelState.AddModelError("", $"Invalid Date of Birth");
                            continue;
                        }

                        if (phone.Length != 8)
                        {
                            ModelState.AddModelError("", $"Invalid phone number");
                            continue;
                        }
                        if (_context.Contacts.Any(c => c.Phone == values[3].Trim()))
                        {
                            ModelState.AddModelError("", $"The phone number {values[3].Trim()} already exists.");
                            continue;
                        }
                        if (!decimal.TryParse(values[4].Trim(), out var salary) || salary < 0)
                        {
                            ModelState.AddModelError("", $"Invalid salary.");
                            continue;
                        }
                        contact = new Contact
                        {
                            Name = name,
                            DateOfBbirth = dob,
                            Married = married,
                            Phone = phone,
                            Salary = salary
                        };
                        break;

                    }
                }

                if (contact != null)
                {
                    _context.Contacts.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "The file could not be processed.");
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing the file: {ex.Message}");
            }
            return View("Index", await _context.Contacts.ToListAsync());
        }
    }
}
