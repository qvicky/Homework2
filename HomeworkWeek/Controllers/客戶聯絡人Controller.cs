using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeworkWeek1.Models;
using PagedList;

namespace HomeworkWeek1.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        // GET: 客戶聯絡人
        public ActionResult Index(string keyword, string sortOrder, int p = 1)
        {
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(p => !p.是否已刪除);
            ViewBag.sort職稱 = string.IsNullOrEmpty(sortOrder) ? "職稱_desc" : "";
            ViewBag.sort姓名 = sortOrder == "姓名" ? "姓名_desc" : "姓名";
            ViewBag.sortEmail = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.sort姓名 = sortOrder == "手機" ? "手機_desc" : "手機";
            ViewBag.sort姓名 = sortOrder == "電話" ? "電話_desc" : "電話";
            ViewBag.sort姓名 = sortOrder == "客戶名稱" ? "客戶名稱_desc" : "客戶名稱";

            var 客戶聯絡人 = repo.All(keyword, sortOrder);
            var data = 客戶聯絡人.ToList();
            var pageData = data.ToPagedList(pageNumber: p, pageSize: 5);  //分頁

            return View(pageData);
            //return View(客戶聯絡人.ToList());
        }

        //GET
        public ActionResult BatchIndex(int Id) {
            return View(repo.All().Where(p => p.客戶Id == Id));
        }

        [HttpPost]
        public ActionResult BatchIndex(IList<聯絡人批次更新ViewModel> data) {
            List<客戶聯絡人> 回傳聯絡人 = new List<客戶聯絡人>();
            //IList<客戶聯絡人> 回傳聯絡人 = null;
            if (ModelState.IsValid) {
                foreach (var item in data) {
                    var 客戶聯絡人 = repo.Find(item.Id);
                    客戶聯絡人.手機 = item.手機;
                    客戶聯絡人.電話 = item.電話;
                    客戶聯絡人.職稱 = item.職稱;


                    回傳聯絡人.Add(客戶聯絡人);
                }
                repo.UnitOfWork.Commit();
                //return RedirectToAction("Details", "客戶資料");
                TempData["EditMsg"] = "編輯成功 !!";
            }
            //return View(data);
            return View(回傳聯絡人);
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
           ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");  //下拉選單
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                //db.客戶聯絡人.Add(客戶聯絡人);
                //db.SaveChanges();
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            var db = new 客戶資料Entities();
            if (ModelState.IsValid)
            {
                db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            客戶聯絡人.是否已刪除 = true;
            repo.UnitOfWork.Commit();
            //db.客戶聯絡人.Remove(客戶聯絡人);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
