namespace ASC.Web.Areas.Accounts.Models;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity; // Assuming you are using ASP.NET Core Identity

public class ServiceEngineerViewModel
{
    public List<IdentityUser> ServiceEngineers { get; set; } // Lưu trữ danh sách nhân viên

    public ServiceEngineerRegistrationViewModel Registration { get; set; } // Lưu trữ nhân viên mới thêm hoặc cập nhật
}