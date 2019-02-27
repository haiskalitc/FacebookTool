using AutoTool.BaseModel;
using AutoTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTool.Login
{
    /// <summary>
    /// 1: Import token ==> Danh sách token
    /// 2. Lấy thông tin cá nhân từ token
    /// 3. Backup
    /// 4. Đăng nhập(dùng Cookie,Token), xử lý checkpoint nếu có
    /// </summary>
    public class LoginHandler
    {
        public async void LayThongTin(Task task)
        {
            await task;
            task.Dispose();
        }
    }
}
