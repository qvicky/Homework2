using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeworkWeek1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All() {
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);
        }

        public IQueryable<客戶銀行資訊> All(string sort) {
            var data = base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);
            switch (sort) {
                case "銀行代碼_desc":
                    data = data.OrderByDescending(p => p.銀行代碼);
                    break;
                case "銀行代碼":
                    data = data.OrderBy(p => p.銀行代碼);
                    break;
                case "客戶名稱_desc":
                    data = data.OrderByDescending(p => p.客戶資料.客戶名稱);
                    break;
                default:
                    data = data.OrderBy(p => p.客戶資料.客戶名稱);
                    break;
            }
            return data;
        }

        public 客戶銀行資訊 Find(int id) {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
       
	}

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}