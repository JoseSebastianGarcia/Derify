using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Derify.Core.Services
{
    public interface IDerifyService
    {
        string GetCode(HttpContext context);
    }
}
