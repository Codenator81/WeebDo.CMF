using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Dnx.Runtime;

namespace WeebDoCMSTest
{
    public class Startup : WeebDoCMS.Startup
    {
        public Startup(IApplicationEnvironment env) : base(env)
        {
        }
    }
}
