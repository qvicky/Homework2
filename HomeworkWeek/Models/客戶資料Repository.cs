using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
	
namespace HomeworkWeek1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 Find(int id) {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public EditMemberView FindEditMember(string  accountName) {
            EditMemberView member = new EditMemberView();
            客戶資料 data = this.All().FirstOrDefault(p => p.帳號.Contains(accountName));
            if (data != null) {
                member.Id = data.Id;
                member.地址 = data.地址;
                member.Email = data.Email;
                member.電話 = data.電話;
                member.傳真 = data.傳真;
                member.密碼 = data.密碼;
            }
            return member;
        }

        public IQueryable<客戶資料> All(string keyword, string clientType, string sort) {
            var data = this.Where(p => p.是否已刪除 == false).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                data = data.Where(p => p.客戶名稱.Contains(keyword));

            if (!string.IsNullOrEmpty(clientType)) {
                data = data.Where(p => p.客戶分類.Contains(clientType));
            }

            #region 排序處理
            switch (sort) {
                case "客戶名稱_desc":
                    data = data.OrderByDescending(s => s.客戶名稱);
                    break;
                case "統一編號_desc":
                    data = data.OrderByDescending(s => s.統一編號);
                    break;
                case "統一編號":
                    data = data.OrderBy(s => s.統一編號);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(s => s.電話);
                    break;
                case "電話":
                    data = data.OrderBy(s => s.電話);
                    break;
                case "傳真_desc":
                    data = data.OrderByDescending(s => s.傳真);
                    break;
                case "傳真":
                    data = data.OrderBy(s => s.傳真);
                    break;
                case "地址_desc":
                    data = data.OrderByDescending(s => s.地址);
                    break;
                case "地址":
                    data = data.OrderBy(s => s.地址);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(s => s.Email);
                    break;
                case "Email":
                    data = data.OrderBy(s => s.Email);
                    break;
                case "客戶分類_desc":
                    data = data.OrderByDescending(s => s.客戶分類);
                    break;
                case "客戶分類":
                    data = data.OrderBy(s => s.客戶分類);
                    break;
                default:
                    data = data.OrderBy(s => s.客戶名稱);
                    break;
            }
            #endregion

            return data;
        }

        public bool LoginIsOK(string account, string pwd) {
            bool bResult = false;
            
            //if (!this.Where(p => p.帳號.Contains(account)).Any());
            //    return false;

            string dbPwd = this.Where(p => p.帳號.Contains(account)).FirstOrDefault().密碼;
            byte[] loginPwd = GenPasswordHash(pwd);
            string sloginPwd = GetPasswordString(loginPwd);

            if (dbPwd == sloginPwd)
                bResult=true;
            else
                bResult = false;

            return bResult;
        }

        public byte[] GenPasswordHash(string pwd) {
            HashAlgorithm hashPwd = HashAlgorithm.Create("MD5");
            byte[] pwData = Encoding.Default.GetBytes(pwd);
            byte[] myHash = hashPwd.ComputeHash(pwData);

            return myHash;
        }

        public string GetPasswordString(byte[] pwd) {
            HashAlgorithm hashPwd = HashAlgorithm.Create("MD5");
            string sPwd = BitConverter.ToString(pwd);
            return sPwd;
        }

        public void Edit客戶資料(EditMemberView member) {

        }

        public 客戶資料 Upd客戶資料(EditMemberView member) {
            客戶資料 data = Find(member.Id);
            data.電話 = member.電話;
            data.傳真 = member.傳真;
            data.地址 = member.地址;
            data.Email = member.Email;
            byte[] aryPwd = GenPasswordHash(member.密碼);
            data.密碼 = GetPasswordString(aryPwd);

            return data;
        }

        public List<SelectListItem> get客戶分類() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "--客戶分類--", Value = ""});
            items.Add(new SelectListItem() { Text = "關鍵客戶", Value = "關鍵客戶" });
            items.Add(new SelectListItem() { Text = "主要客戶", Value = "主要客戶", Selected = true });
            items.Add(new SelectListItem() { Text = "普通客戶", Value = "普通客戶" });
            return items;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}