using System;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return Content(@"<html>
<body>
hello

<script type='text/javascript' src='/statics/signalr-clientES5-1.0.0-alpha2-final.min.js'></script>
<script type='text/javascript' src='/core/gen/antibot.js'></script>
</body>
</html>" , "text/html");

        }

    }
}
