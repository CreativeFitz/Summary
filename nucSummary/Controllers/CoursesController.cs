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
using nucSummary.Models.ViewModels;

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
        public async Task<IActionResult> Index(string searchQuery, string filterQuery)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            //Creating a course list to add serach queried courses
            List<Courses> courseList = await _context.Courses
                .Include(c => c.Reviews)
                .ToListAsync();
            decimal numberOfReviews = 1;
            //Variable to store the average of all a single review's ratings on a course
            decimal combinedRatingsAverage = 0;
            //Variable to store all of the Difficulty ratings of the Courses reviews
            decimal singleDifficultyRatingAvg = 0;
            //Variable to store all of the Difficulty ratings of the Courses reviews
            decimal singleRelevancyRatingAvg = 0;

            //Variable to store the average of a Courses Review ratings
            decimal overallAverage = 0;
            
            
            
            
            List<CourseReviewViewModel> CourseVMList = new List<CourseReviewViewModel>();

            foreach (Courses course in courseList)
            {
                //Setting averages back to zero when the loop moves to the next course
                overallAverage = 0;
                singleDifficultyRatingAvg = 0;
                singleRelevancyRatingAvg = 0;
                foreach (Reviews review in course.Reviews)
                {
                    //Averaging together all the ratings in a review
                    combinedRatingsAverage =
                    Convert.ToDecimal(((double)review.Difficulty + review.Content + review.Design + review.Assessments + review.Exercises + review.Relevancy) / 6);

                    singleDifficultyRatingAvg += Convert.ToDecimal((double)review.Difficulty);

                    singleRelevancyRatingAvg += Convert.ToDecimal((double)review.Relevancy);

                    //The sum of all the reviews' averages 
                    overallAverage += combinedRatingsAverage;
                    



                    //Creating the divisor for the total course average
                    if (course.Reviews.Count != 0)
                    {
                        numberOfReviews = course.Reviews.Count;
                    }

                    
                }
                

                decimal courseAverage = overallAverage / numberOfReviews;
                //Variable to store the average of all the Difficulty ratings of a course.
                decimal difficultyRatingAvg = singleDifficultyRatingAvg / numberOfReviews;
                //Variable to store the average of all the Difficulty ratings of a course.
                decimal relevancyRatingAvg = singleRelevancyRatingAvg / numberOfReviews;


                var viewModel = new CourseReviewViewModel()
                {
                    Course = course,
                    OverallAverage = courseAverage,
                    DifficultyAverage = difficultyRatingAvg,
                    RelevancyAverage = relevancyRatingAvg
                };
                CourseVMList.Add(viewModel);
            };




            //Adding all courses to courselist where the search query is found in a courses title//
            if (searchQuery != null)
            {
                CourseVMList = CourseVMList.Where(courseVM => courseVM.Course.Title.Contains(searchQuery)).ToList();
            }
            //Ordering by drop down selection//
            if (filterQuery == "1")
            {
                CourseVMList = CourseVMList.OrderByDescending(cvm => cvm.OverallAverage).ToList();
            }
            if (filterQuery == "2")
            {
                CourseVMList = CourseVMList.OrderByDescending(cvm => cvm.DifficultyAverage).ToList();
            }
            if (filterQuery == "3")
            {
                CourseVMList = CourseVMList.OrderByDescending(cvm => cvm.RelevancyAverage).ToList();
            }



            return View(CourseVMList);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            


            var course = await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(r => r.Reviews)
                    .ThenInclude(r => r.ApplicationUser)
                    // .OrderBy(r => r.Reviews.OrderBy)
                    .FirstOrDefaultAsync(m => m.Id == id);

            course.Reviews = course.Reviews.OrderByDescending(r => r.DateAdded.Date).ThenByDescending(c => c.DateAdded.TimeOfDay).ToList();

            decimal overallAverage = 0;
            decimal numberOfReviews = 1;

            foreach (Reviews rating in course.Reviews){
                decimal average =
                Convert.ToDecimal(((double)rating.Difficulty + rating.Content + rating.Design + rating.Assessments + rating.Exercises + rating.Relevancy) / 6);

                overallAverage += average;


                if (course.Reviews.Count != 0)
                {
                    numberOfReviews = course.Reviews.Count;
                }
               
            };

            decimal courseAverage = overallAverage / numberOfReviews ;

            var viewModel = new CourseReviewViewModel()
            {
                Course = course,
                OverallAverage = courseAverage
            };
            if (course == null)
            {
                return NotFound();
            }

            return View(viewModel);
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
