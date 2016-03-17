using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeworkWeek1.Models
{   
	public  class vw客戶關聯資料Repository : EFRepository<vw客戶關聯資料>, Ivw客戶關聯資料Repository
	{

	}

	public  interface Ivw客戶關聯資料Repository : IRepository<vw客戶關聯資料>
	{

	}
}