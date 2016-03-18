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
using HomeworkWeek1.ActionFilters;
using System.IO;
using NPOI.HSSF.UserModel;

namespace HomeworkWeek1.Controllers
{
    [記錄Action的執行時間]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error")]
    public class 客戶銀行資訊Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶銀行資訊Repository repo = RepositoryHelper.Get客戶銀行資訊Repository();
        // GET: 客戶銀行資訊
        public ActionResult Index(string sortOrder, int p = 1)
        {
            //var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料).Where(p => !p.是否已刪除);
            ViewBag.sort銀行代碼 = string.IsNullOrEmpty(sortOrder) ? "銀行代碼_desc" : "銀行代碼";
            ViewBag.sort客戶名稱 = string.IsNullOrEmpty(sortOrder) ? "客戶名稱_desc" : "";
            var 客戶銀行資訊 = repo.All(sortOrder);

            var data = 客戶銀行資訊.ToList();
            var pageData = data.ToPagedList(pageNumber: p, pageSize: 5);

            return View(pageData);
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null && 客戶銀行資訊.是否已刪除)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶銀行資訊);
                repo.UnitOfWork.Commit();
                //db.客戶銀行資訊.Add(客戶銀行資訊);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null && 客戶銀行資訊.是否已刪除)
            {
                return HttpNotFound();
            }
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            var db = (客戶資料Entities)repo.UnitOfWork.Context;
            if (ModelState.IsValid)
            {
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null && 客戶銀行資訊.是否已刪除)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id);
            客戶銀行資訊.是否已刪除 = true;
            repo.UnitOfWork.Commit();

            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            //db.客戶銀行資訊.Remove(客戶銀行資訊);
            //db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ExportByNPOI() {
            List<客戶銀行資訊> exportData = repo.All().ToList();

            MemoryStream output = new MemoryStream();
            HSSFWorkbook book = new HSSFWorkbook();
            output = repo.ExportExcel(exportData, book);  //產生excel檔案

            return File(output.ToArray(), "application/vnd.ms-excel", "客戶銀行資訊.xls");
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
