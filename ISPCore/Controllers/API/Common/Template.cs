using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class ApiCommonTemplate : Controller
    {
        public string Export(int Id) => new RequestsFilterToTemplateController().Export(Id);
        public JsonResult Import() => Json(new RequestsFilterToTemplateController().Import(HttpContext));
    }
}
