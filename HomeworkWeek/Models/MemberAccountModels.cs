using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeworkWeek1.Models {
    public class MemberAccountModels {
    }

    public class LoginView {
        [Required]
        public string 帳號 { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string 密碼 { get; set; }
    }

    public class EditMemberView {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 傳真 { get; set; }

        [StringLength(100, ErrorMessage = "欄位長度不得大於 100 個字元")]
        public string 地址 { get; set; }

        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元")]
        [EmailAddress(ErrorMessage="Email格式錯誤")]
        public string Email { get; set; }
        [Required]
        public string 密碼 { get; set; }
    }


}