using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Dnx.Runtime;

namespace WeebDoCMFTest
{
    public class Startup : WeebDoCMF.Startup
    {
        public Startup(IApplicationEnvironment env) : base(env)
        {
        }
    }
}
