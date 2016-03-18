using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.HSSF.Util;
	
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


        public MemoryStream ExportExcel(List<客戶銀行資訊> exportData, HSSFWorkbook book) {
            DataTable dt = new DataTable();
            dt.Columns.Add("銀行名稱", typeof(string));
            dt.Columns.Add("銀行代碼", typeof(string));
            dt.Columns.Add("分行代碼", typeof(string));
            dt.Columns.Add("帳戶名稱", typeof(string));
            dt.Columns.Add("帳戶號碼", typeof(string));
            dt.Columns.Add("客戶稱", typeof(string));
            for (int i = 0; i < exportData.Count; i++) {
                dt.Rows.Add(exportData[i].銀行名稱, exportData[i].銀行代碼,
                    exportData[i].分行代碼, exportData[i].帳戶名稱,
                    exportData[i].帳戶號碼, exportData[i].客戶資料.客戶名稱);
            }
            HSSFSheet sheet = (HSSFSheet)book.CreateSheet();
            HSSFCellStyle headCellStyle = (HSSFCellStyle)book.CreateCellStyle();
            HSSFCellStyle dataCellStyle = (HSSFCellStyle)book.CreateCellStyle();

            HSSFFont font = (HSSFFont)book.CreateFont();
            font.FontHeightInPoints = 12;
            font.FontName = "微軟正黑體";
            font.Color = HSSFColor.Orange.Index;  //字的顏色
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

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}