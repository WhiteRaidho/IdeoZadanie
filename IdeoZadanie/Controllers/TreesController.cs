using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeoZadanie.DAL;
using IdeoZadanie.Models;

namespace IdeoZadanie.Controllers
{
    public class TreesController : Controller
    {
        private TreeContext db = new TreeContext();

        // GET: Trees/2
        public ActionResult Index(int? id)
        {
            var trees = db.Trees.Include(t => t.Parent);
            return View(trees.ToList());
        }

        // GET: Trees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            return View(tree);
        }

        // GET: Trees/Create
        public ActionResult Create(int? id)
        {
            ViewBag.Parent = null;
            ViewBag.ParentName = null;
            if (id != null)
            {
                Tree parent = db.Trees.SingleOrDefault(t => t.Id == id);
                if (parent != null)
                {
                    ViewBag.Parent = id;
                    ViewBag.ParentName = parent.Name;
                }
            }
            //ViewBag.ParentId = new SelectList(db.Trees, "Id", "Name");
            return View();
        }

        // POST: Trees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Tree tree, int? id)
        {
            if (ModelState.IsValid)
            {
                db.Trees.Add(tree);
                db.SaveChanges();
                if (id != null)
                {
                    Tree parent = db.Trees.SingleOrDefault(t => t.Id == id);
                    if (parent != null)
                    {
                        tree.Parent = parent;
                    }
                    else
                    {
                        tree.Parent = tree;
                    }
                }
                else
                {
                    tree.Parent = tree;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Trees, "Id", "Name", tree.ParentId);
            return View(tree);
        }

        // GET: Trees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = GetSelectListFor(tree);
            return View(tree);
        }

        // POST: Trees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ParentId")] Tree tree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = GetSelectListFor(tree);
            return View(tree);
        }

        // GET: Trees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            return View(tree);
        }

        // POST: Trees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tree tree = db.Trees.Find(id);
            RemoveChildrensFromDatabase(tree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void RemoveChildrensFromDatabase(Tree tree)
        {
            if (tree.Childrens.Where(t => t.Id != tree.Id).ToList().Count > 0)
            {
                foreach (Tree t in tree.Childrens.Where(t => t.Id != tree.Id).ToList())
                {
                    RemoveChildrensFromDatabase(t);
                }
            }
            db.Trees.Remove(tree);
        }

        // GET: Trees/Move/5
        public ActionResult Move(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = GetSelectListFor(tree);
            return View(tree);
        }

        // POST: Trees/Move/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move([Bind(Include = "Id, Name, ParentId")]Tree tree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = GetSelectListFor(tree);
            return View(tree);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private SelectList GetSelectListFor(Tree tree)
        {
            List<Tree> trees = GetListWithoutChildrens(db.Trees.ToList(), tree);
            if (!trees.Contains(tree)) trees.Add(tree);

            return new SelectList(trees, "Id", "Name", tree.ParentId); ;
        }

        private List<Tree> GetListWithoutChildrens(List<Tree> trees, Tree tree)
        {
            foreach (Tree t in tree.Childrens)
            {
                if (t.Childrens.Count > 1 && t.Id != tree.Id)
                {
                    trees = GetListWithoutChildrens(trees, t);
                }
                trees.Remove(t);
            }
            return trees;
        }
    }
}
