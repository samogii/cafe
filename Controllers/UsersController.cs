using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafe.Data;
using Cafe.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;

namespace Cafe.Controllers
{
    
    public class UsersController : Controller
    {
        private DataContext _context;
        private IJwtUtils _jwtUtils;
        public UsersController(DataContext context , IJwtUtils jwtUtils)
        {
            _context = context;
            _jwtUtils = jwtUtils;
        }

        [Authorize]
        [Route("users/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Redirect("/users/login");
        }

        // GET: Users
        [Admin]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            var exist = (User?)HttpContext.Items["User"];
            if (exist.Role == Enums.Role.ADMIN)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            var myUser = await _context.Users.Where(x => x.Id == exist.Id).FirstAsync();
            return View(myUser);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Admin]
        public async Task<IActionResult> Create(User user)
        {
            if (!_context.Users.Any(x => x.Email == user.Email))
            {
                var newUser = new User(user.Email, user.Password , user.UserName);
                _context.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            if (!_context.Users.Any(x=>x.Email == user.Email))
            {
                var newUser = new User(user.Email, user.Password, user.UserName);
                _context.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(user);
        }

        // GET: Users/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,EmailConfirmed,Phone,Birthdate,CreatedAt")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //User user = _context.Users.Where(x=>x.Email == model.Email).First();
                var user =  _context.Users.FirstOrDefault(x=>x.Email == model.Email);

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        // Redirect to the original URL or a default if provided
                        var token = _jwtUtils.GenerateToken(user);
                        Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            // Set additional options like expiration, secure, httpOnly, etc.
                            Expires = DateTimeOffset.UtcNow.AddHours(1), // Example: Cookie expires in 1 hour
                            HttpOnly = true, // Ensures that the cookie is only accessible on the server side
                            Secure = true, // Requires an HTTPS connection to send the cookie              // Add other options as needed
                        });

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
