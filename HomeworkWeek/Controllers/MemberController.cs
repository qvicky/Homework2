using HomeworkWeek1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HomeworkWeek1.Controllers
{
    public class MemberController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginView login) {
            if (CheckLogin(login.帳號, login.密碼)) {
                FormsAuthentication.RedirectFromLoginPage(login.帳號, false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("密碼", "您輸入的帳號或密碼是錯誤的");
            return View();
        }

        [AllowAnonymous]
        public ActionResult EditMember() {
            客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
            EditMemberView member = repo.FindEditMember(User.Identity.Name);
            if (member != null) {
                member.密碼 = string.Empty;
            }
            return View(member);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult EditMember([Bind(Include = "Id,電話,傳真,地址,Email,密碼")]EditMemberView member) {
            客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
           
            if (ModelState.IsValid) {
                byte[] aryNewPwd = repo.GenPasswordHash(member.密碼);
                member.密碼 = repo.GetPasswordString(aryNewPwd);
                //要修改
                客戶資料 data = repo.Upd客戶資料(member);
                var db = (客戶資料Entities)repo.UnitOfWork.Context;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(member);
        }

        private bool CheckLogin(string account, string password) {
            客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

            return repo.LoginIsOK(account, password);
        }

        public ActionResult LogOut() {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}