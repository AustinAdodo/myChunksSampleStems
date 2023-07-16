using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApiExtension;

namespace TestForFruitsApi
{
    public class StemServiceFactory : WebApplicationFactory<Program>
    {
        public StemServiceFactory()
        {
            //adtional settings
          //mocking
        }
    }
}
