 using OnlineExam.Models;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DB db = new DB();

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole(RoleViewModel roleViewModel)
        {
            var role = new UserRole
            {
                RoleName = roleViewModel.RoleName
            };

            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                await db.SaveChangesAsync();
                ViewBag.StatusMessage = "Role Succesfully Completed";
                return RedirectToAction("RoleList");
            }

            return View(role);
        }

        public async Task<ActionResult> RoleList()
        {
            return View(await db.Roles.ToListAsync());
        }

        public async Task<ActionResult> UserAccounts()
        {
            var users = await db.Users.ToListAsync();
            foreach (var item in users)
            {
                item.Role = db.Roles.Where(r => r.RoleId == item.RoleId).FirstOrDefault();
            }
            return View(users);
        }

        public ActionResult AddUser()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Roles = db.Roles.ToList();
            model.RoleId = 0;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            string alpha = null;

            if (model.RoleId == 1)
            {
                alpha = "ECA";
            }
            else if (model.RoleId == 2)
            {
                alpha = "ECS";
            }
            else if (model.RoleId == 3)
            {
                alpha = "ECT";
            }
            else if (model.RoleId == 4)
            {
                alpha = "ECD";
            }
            else
            {
                alpha = "ECC";
            }

            Random random = new Random();
            int unique = random.Next(10000, 99999);
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            var userId = alpha + y + m + unique;

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                CreatedDate = DateTime.Now,
                RoleId = model.RoleId
            };

            var data = db.Users.Where(d => d.Email == user.Email || d.UserName == user.UserName).FirstOrDefault();

            if (data != null)
            {
                ModelState.AddModelError("", "Email or Username already exists");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                ViewBag.StatusMessage = "User Created Succesfully.";
                return RedirectToAction("UserAccounts");
            }

            return View(model); ;
        }


        public async Task<ActionResult> ProgrammeList()
        {

            return View(await db.Programme.Where(p => p.IsDeleted == 0).ToListAsync());
        }

        public async Task<ActionResult> CreateProgramme(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                var data = await db.Programme.Where(d => d.Id == id).FirstOrDefaultAsync();
                ProgrammesViewModel viewModel = new ProgrammesViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate
                };
                return View(viewModel);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProgramme(ProgrammesViewModel programmes)
        {
            if (programmes.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Programmes model = new Programmes()
                    {
                        Id = (int)programmes.Id,
                        Name = programmes.Name,
                        CreatedBy = programmes.CreatedBy,
                        CreatedDate = programmes.CreatedDate,
                        IsDeleted = programmes.IsDeleted,
                        ModifiedTime = DateTime.Now,
                        ModifiedBy = 1,
                        DeletedDate = programmes.DeletedDate,

                    };

                    db.Entry(model).State = EntityState.Modified;
                    ViewBag.StatusMessage = "Programme Edited Succesfully.";
                    await db.SaveChangesAsync();
                    return RedirectToAction("ProgrammeList");
                }

                return View(programmes);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Programmes model = new Programmes()
                    {
                        Name = programmes.Name,
                        //CreatedBy = 0,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now
                    };

                    db.Programme.Add(model);
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Programme Created Succesfully.";
                    return RedirectToAction("ProgrammeList");
                }
                return View();
            }
        }

        public async Task<ActionResult> DeleteProgramme(int? id)
        {
            if (id == null)
            {
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("ProgrammeList");
            }
            else
            {
                Programmes programmes = await db.Programme.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (programmes != null)
                {
                    programmes.IsDeleted = 1;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Programme Deleted Succesfully.";
                    return RedirectToAction("ProgrammeList");
                }
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("ProgrammeList");
            }


        }

        public async Task<ActionResult> CreateSubProgram(int? id)
        {

            if (id == null)
            {
                SubProgramViewModel model = new SubProgramViewModel();
                model.Programme = db.Programme.ToList();
                model.PgmId = 0;
                return View(model);
            }
            else
            {
                var data = await db.SubPrograms.Where(d => d.Id == id).FirstOrDefaultAsync();
                SubProgramViewModel viewModel = new SubProgramViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    PgmId = data.PgmId,
                    Programme = db.Programme.ToList()
                };
                //pgmid = data.PgmId;
                //viewModel.Programme = db.Programme.ToList();
                //viewModel.PgmId = pgmid;
                return View(viewModel);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSubProgram(SubProgramViewModel subProgram)
        {
            if (subProgram.Id != null)
            {
                if (ModelState.IsValid)
                {
                    SubProgram model = new SubProgram()
                    {
                        Id = (int)subProgram.Id,
                        Name = subProgram.Name,
                        CreatedBy = subProgram.CreatedBy,
                        CreatedDate = subProgram.CreatedDate,
                        IsDeleted = subProgram.IsDeleted,
                        ModifiedTime = DateTime.Now,
                        ModifiedBy = 1,
                        DeletedDate = subProgram.DeletedDate,

                    };

                    db.Entry(model).State = EntityState.Modified;
                    ViewBag.StatusMessage = "SubProgram Edited Succesfully.";
                    await db.SaveChangesAsync();
                    return RedirectToAction("SubProgramList");
                }

                return View(subProgram);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    SubProgram model = new SubProgram()
                    {
                        Name = subProgram.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now,
                        PgmId = subProgram.PgmId

                    };

                    db.SubPrograms.Add(model);
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "SubProgram Created Succesfully.";
                    return RedirectToAction("SubProgramList");
                }
                return View();
            }
        }

        public async Task<ActionResult> SubProgramList()
        {

            return View(await db.SubPrograms.Where(p => p.IsDeleted == 0).ToListAsync());
        }

        public async Task<ActionResult> DeleteSubProgram(int? id)
        {
            if (id == null)
            {
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("SubProgramList");
            }
            else
            {
                SubProgram subProgram = await db.SubPrograms.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (subProgram != null)
                {
                    subProgram.IsDeleted = 1;
                    db.Entry(subProgram).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "SubProgram Deleted Succesfully.";
                    return RedirectToAction("SubProgramList");
                }
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("SubProgramList");
            }


        }

        public async Task<ActionResult> CreateClass(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                var data = await db.Classes.Where(c => c.Id == id).FirstOrDefaultAsync();
                ClassViewModel viewModel = new ClassViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateClass(ClassViewModel classView)
        {
            if (classView.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Class model = new Class()
                    {
                        Id = (int)classView.Id,
                        Name = classView.Name,
                        CreatedBy = classView.CreatedBy,
                        CreatedDate = classView.CreatedDate,
                        IsDeleted = classView.IsDeleted,
                        ModifiedTime = DateTime.Now,
                        ModifiedBy = 1,
                        DeletedDate = classView.DeletedDate,

                    };

                    db.Entry(model).State = EntityState.Modified;
                    ViewBag.StatusMessage = "Class Edited Succesfully.";
                    await db.SaveChangesAsync();
                    return RedirectToAction("ClassList");
                }

                return View(classView);

            }
            else
            {
                if (ModelState.IsValid)
                {
                    Class model = new Class()
                    {
                        Name = classView.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now
                    };

                    db.Classes.Add(model);
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Class Created Succesfully.";
                    return RedirectToAction("ClassList");
                }
                return View();
            }




        }

        public async Task<ActionResult> ClassList()
        {
            return View(await db.Classes.Where(c => c.IsDeleted == 0).ToListAsync());
        }


        public async Task<ActionResult> DeleteClass(int? id)
        {
            if (id == null)
            {
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("ClassList");
            }
            else
            {
                Class classes = await db.Classes.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (classes != null)
                {
                    classes.IsDeleted = 1;
                    db.Entry(classes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Class Deleted Succesfully.";
                    return RedirectToAction("ClassList");
                }
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("ClassList");
            }


        }

        public async Task<ActionResult> CreateCourse(int? id)
        {
            if (id == null)
            {
                CourseViewModel model = new CourseViewModel();
                model.Classes = db.Classes.ToList();
                model.ClassId = 0;
                return View(model);
            }
            else
            {
                var data = await db.Courses.Where(d => d.Id == id).FirstOrDefaultAsync();
                CourseViewModel course = new CourseViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    ClassId = data.ClassId,
                    Classes = db.Classes.ToList()
                };
                /* viewModel.Classes = db.Classes.ToList();
                 viewModel.ClassId = classId;*/
                return View(course);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCourse(CourseViewModel course)
        {

            if (course.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Course model = new Course()
                    {
                        Id = (int)course.Id,
                        Name = course.Name,
                        CreatedBy = course.CreatedBy,
                        CreatedDate = course.CreatedDate,
                        IsDeleted = course.IsDeleted,
                        ModifiedTime = DateTime.Now,
                        ModifiedBy = 1,
                        DeletedDate = course.DeletedDate,
                        ClassId = course.ClassId

                    };

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Course Edited Succesfully.";
                    return RedirectToAction("CourseList");
                }

                return View(course);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Course model = new Course()
                    {
                        Name = course.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now,
                        ClassId = course.ClassId

                    };

                    db.Courses.Add(model);
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Course Created Succesfully.";
                    return RedirectToAction("CourseList");
                }
                return View();
            }

        }

        public async Task<ActionResult> CourseList()
        {

            return View(await db.Courses.Where(p => p.IsDeleted == 0).ToListAsync());
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]*/
        public async Task<ActionResult> DeleteCourse(int? id)
        {
            if (id == null)
            {
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("CourseList");
            }
            else
            {
                Course course = await db.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();

                if(course != null)
                {
                    course.IsDeleted = 1;
                    db.Entry(course).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.StatusMessage = "Course Deleted Succesfully.";
                    return RedirectToAction("CourseList");
                }
                ViewBag.StatusMessage = "Not Deleted..!";
                return RedirectToAction("CourseList");
            }


        }
    }
}