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
    public class 客戶資料Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶資料
        public ActionResult Index(string keyword, string clientType, string sortOrder, int p = 1)
        {
            //var data = db.客戶資料.AsQueryable().Where(p => p.是否已刪除 == false).AsQueryable();
            //if (!string.IsNullOrEmpty(keyword))
            //    data = data.Where(p => p.客戶名稱.Contains(keyword));
            ViewBag.sort客戶名稱 = string.IsNullOrEmpty(sortOrder) ? "客戶名稱_desc" : "";
            ViewBag.sort統一編號 = sortOrder == "統一編號" ? "統一編號_desc" : "統一編號";
            ViewBag.sort電話 = sortOrder == "電話" ? "電話_desc" : "電話";
            ViewBag.sort傳真 = sortOrder == "傳真" ? "傳真_desc" : "傳真";
            ViewBag.sort地址 = sortOrder == "地址" ? "地址_desc" : "地址";
            ViewBag.sortEmail = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.sort客戶分類 = sortOrder == "客戶分類" ? "客戶分類_desc" : "客戶分類";
            var 客戶資料 = repo.All(keyword, clientType, sortOrder);

            var data = 客戶資料.ToList();
            var pageData = data.ToPagedList(pageNumber: p, pageSize: 5);

            return View(pageData);
        }

        public ActionResult 客戶關聯資料() {
            var db = new 客戶資料Entities();

            return View(db.vw客戶關聯資料.ToList());
        }
        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號, 密碼")] 客戶資料 客戶資料) {
            if (ModelState.IsValid) {
                //db.客戶資料.Add(客戶資料);
                //db.SaveChanges();
                byte[] myPwd = repo.GenPasswordHash(客戶資料.密碼);
                客戶資料.密碼 = repo.GetPasswordString(myPwd);
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            客戶資料.密碼 = string.Empty;
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,IsDeleted, 客戶分類, 帳號, 密碼")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid) {
                byte[] aryPwd = repo.GenPasswordHash(客戶資料.密碼);
                客戶資料.密碼 = repo.GetPasswordString(aryPwd);
                var db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(客戶資料).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.Find(id);
            //db.客戶資料.Remove(客戶資料);
            客戶資料.是否已刪除 = true;
            //db.SaveChanges();
            repo.UnitOfWork.Commit();
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
