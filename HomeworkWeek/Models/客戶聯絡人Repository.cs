using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeworkWeek1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All() {

            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);
        }

        public IQueryable<客戶聯絡人> All(string keyword,string sort) {
            var data = this.Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                data = data.Where(p => p.職稱.Contains(keyword));

            #region 排序處理
            switch (sort) {
                case "職稱_desc":
                    data = data.OrderByDescending(s => s.職稱);
                    break;
                case "姓名_desc":
                    data = data.OrderByDescending(s => s.姓名);
                    break;
                case "姓名":
                    data = data.OrderBy(s => s.姓名);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(s => s.Email);
                    break;
                case "Email":
                    data = data.OrderBy(s => s.Email);
                    break;
                case "手機_desc":
                    data = data.OrderByDescending(s => s.手機);
                    break;
                case "手機":
                    data = data.OrderBy(s => s.手機);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(s => s.電話);
                    break;
                case "電話":
                    data = data.OrderBy(s => s.電話);
                    break;
                case "客戶名稱_desc":
                    data = data.OrderByDescending(s => s.客戶資料.客戶名稱);
                    break;
                case "客戶名稱":
                    data = data.OrderBy(s => s.客戶資料.客戶名稱);
                    break;
                default:
                    data = data.OrderBy(s => s.職稱);
                    break;
            }
            #endregion

            return data;
        }

        public 客戶聯絡人 Find(int id) {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

	}

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}