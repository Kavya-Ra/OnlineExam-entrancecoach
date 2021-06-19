using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineExam.Models;

namespace OnlineExam.Controllers
{
    public class ProgrammesController : Controller
    {
        private DB db = new DB();

        // GET: Programmes
        public async Task<ActionResult> Index()
        {
            return View(await db.Programme.ToListAsync());
        }

        // GET: Programmes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programmes programmes = await db.Programme.FindAsync(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // GET: Programmes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CreatedBy,CreatedDate,IsDeleted,DeletedDate,ModifiedBy,ModifiedTime")] Programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.Programme.Add(programmes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(programmes);
        }

        // GET: Programmes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programmes programmes = await db.Programme.FindAsync(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: Programmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreatedBy,CreatedDate,IsDeleted,DeletedDate,ModifiedBy,ModifiedTime")] Programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programmes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(programmes);
        }

        // GET: Programmes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programmes programmes = await db.Programme.FindAsync(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: Programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Programmes programmes = await db.Programme.FindAsync(id);
            db.Programme.Remove(programmes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
