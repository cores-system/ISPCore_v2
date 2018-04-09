using System;
using Microsoft.AspNetCore.Mvc;
using KoenZomers.OneDrive.Api;
using System.Threading.Tasks;

namespace ISPCore.Controllers
{
    public class SyncBackupToAuthorizeController : Controller
    {
        [HttpGet]
        async public Task<IActionResult> OneDrive(string client_id, string code)
        {
            ViewBag.code = code;
            ViewBag.client_id = client_id;

            // Получаем TokenRefresh
            if (!string.IsNullOrWhiteSpace(client_id) && !string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    // Создаем API с нужным приложением
                    var OneDriveApi = new OneDriveGraphApi(client_id);

                    // Получаем токен
                    var AuthorizationTokenFromUrl = OneDriveApi.GetAuthorizationTokenFromUrl("https://login.microsoftonline.com/common/oauth2/nativeclient?code=" + code);
                    await OneDriveApi.GetAccessToken();

                    // Выводим токен
                    ViewBag.RefreshToken = OneDriveApi.AccessToken.RefreshToken;
                }
                catch { }
            }

            return View("~/Views/SyncBackup/Authorize/OneDrive.cshtml");
        }
    }
}
