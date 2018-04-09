using ISPCore.Engine;
using ISPCore.Engine.Base.SqlAndCache;
using ISPCore.Models.Databases;
using ISPCore.Models.Security.AntiDdos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace ISPCore.Controllers
{
    public class ApiListAntiDdos : ControllerToDB
    {
        public JsonResult StatsDay()
        {
            var DtNumberOfRequestDay = coreDB.AntiDdos_NumberOfRequestDays.AsNoTracking().ToList();
            if (memoryCache.TryGetValue(KeyToMemoryCache.AntiDdosNumberOfRequestDay(DateTime.Now), out NumberOfRequestDay dataHour))
                DtNumberOfRequestDay.Add(dataHour);

            return Json(DtNumberOfRequestDay);
        }


        public JsonResult StatsMonth() => Json(coreDB.AntiDdos_NumberOfRequestMonths.AsNoTracking().AsEnumerable().Reverse());
        public JsonResult Jurnal(int page = 1, int pageSize = 20) => Json(coreDB.AntiDdos_Jurnals.AsNoTracking().AsEnumerable().Reverse().Skip((page * pageSize) - pageSize).Take(pageSize));
    }
}
