using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using nucSummary.Data;
using nucSummary.Models;


namespace nucSummary.Controllers
{
    public class CoursesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        //GET current signed-in user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Courses
        [Authorize]
        public async Task<IActionResult> Index(string searchQuery)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            //Creating a course list to add serach queried courses
            List<Courses> courseList = await _context.Courses
                .ToListAsync();
            //Adding all courses to courselist where the search query is found in a courses title//
            if(searchQuery != null)
            {
                courseList = courseList.Where(course => course.Title.Contains(searchQuery)).ToList();
            }
            return View(courseList);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var courses = await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(r => r.Reviews)
                    .ThenInclude(r => r.ApplicationUser)
                    // .OrderBy(r => r.Reviews.OrderBy)
                    .FirstOrDefaultAsync(m => m.Id == id);

            courses.Reviews = courses.Reviews.OrderByDescending(r => r.DateAdded.Date).ThenByDescending(c => c.DateAdded.TimeOfDay).ToList();
            //courses.Reviews = new List<Reviews>();

            //foreach ( var reviews in courses.Reviews)
            //{
            //    courses.Reviews.Add(reviews);
            //}

            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Code,Host,ApplicationUserId")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();
                courses.ApplicationUserId = user.Id;
                _context.Add(courses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", courses.ApplicationUserId);
            return View(courses);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", courses.ApplicationUserId);
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Code,Host,ApplicationUserId")] Courses courses)
        {
            if (id != courses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursesExists(courses.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", courses.ApplicationUserId);
            return View(courses);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courses = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursesExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
        // GET: Reviews/Create
        public IActionResult CreateReviews()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReviews(int id,  Reviews reviews)
        {
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("CourseId");
            ModelState.Remove("Course");
            var reviewsId = await _context.Reviews.FindAsync(id);
            if (ModelState.IsValid)
            {
                var CurrentUser = await GetCurrentUserAsync();
                reviews.ApplicationUserId = CurrentUser.Id;
                reviews.CourseId = id;
                reviews.DateAdded = DateTime.Now;
                reviews.Id = null;
                _context.Add(reviews);
               
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new RouteValueDictionary(
    new { controller = "Courses", action = "Details", Id = id }));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", reviews.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", reviewsId.CourseId);
            return View(reviews);
        }
    }
}
