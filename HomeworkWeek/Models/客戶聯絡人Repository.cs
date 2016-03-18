using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.HSSF.Util;
	
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

        public MemoryStream ExportExcel(List<客戶聯絡人> exportData, HSSFWorkbook book) {
            DataTable dt = new DataTable();
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("職稱", typeof(string));
            dt.Columns.Add("電話", typeof(string));
            dt.Columns.Add("手機", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("客戶名稱", typeof(string));
            for (int i = 0; i < exportData.Count; i++) {
                dt.Rows.Add(exportData[i].姓名, exportData[i].職稱,
                    exportData[i].電話, exportData[i].手機,
                    exportData[i].Email,exportData[i].客戶資料.客戶名稱);
            }
            HSSFSheet sheet = (HSSFSheet)book.CreateSheet();
            HSSFCellStyle headCellStyle = (HSSFCellStyle)book.CreateCellStyle();
            HSSFCellStyle dataCellStyle = (HSSFCellStyle)book.CreateCellStyle();

            HSSFFont font = (HSSFFont)book.CreateFont();
            font.FontHeightInPoints = 12;
            font.FontName = "微軟正黑體";
            font.Color = HSSFColor.Green.Index;  //字的顏色
            headCellStyle.SetFont(font);

            HSSFFont dataFont = (HSSFFont)book.CreateFont();
            font.FontName = "微軟正黑體";
            dataCellStyle.SetFont(dataFont);

            var hRow = sheet.CreateRow(0);
            //表頭
            for (int h = 0; h < dt.Columns.Count; h++) {
                HSSFRow r = (HSSFRow)sheet.GetRow(0);
                r.CreateCell(h).SetCellValue(dt.Columns[h].ColumnName.ToString());
                r.GetCell(h).CellStyle = headCellStyle;
            }
            //表身
            for (int j = 0; j < dt.Rows.Count; j++) {
                var dRow = sheet.CreateRow(j + 1);
                HSSFRow dr = (HSSFRow)sheet.GetRow(j + 1);
                for (int k = 0; k < dt.Columns.Count; k++) {
                    dr.CreateCell(k).SetCellValue(dt.Rows[j][k].ToString());
                }
            }
            MemoryStream output = new MemoryStream();
            book.Write(output);
            return output;
        }

	}

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}