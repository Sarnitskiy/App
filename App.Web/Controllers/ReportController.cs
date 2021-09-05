using App.DbAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IAccount _account;
        private readonly IProductReport _productReport;
        private readonly int userId;

        public ReportController(IAccount account, IProductReport productReport,
            IHttpContextAccessor contextAccessor)
        {
            _account = account;
            _productReport = productReport;

            userId = Convert.ToInt32(contextAccessor.HttpContext
                .User.FindFirst(c => c.Type == "id")?.Value);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _account.GetUserRolesAsync(userId));
        }

        public async Task ChangeRole(int roleId, bool isChecked)
        {
            if (isChecked)
            {
                await _account.AddUserRoleAsync(userId, roleId);
            }
            else
            {
                await _account.DeleteUserRoleAsync(userId, roleId);
            }
        }

        public async Task<IActionResult> RefreshButtons()
        {
            return PartialView("_Buttons", await _account.GetUserRolesAsync(userId));
        }

        public async Task<IActionResult> GetData(string request)
        {
            return PartialView("_Products", await _productReport.GetProductsAsync(request));
        }
    }
}
