using System;
using Microsoft.AspNet.Mvc.Razor.Precompilation;
using Microsoft.Dnx.Runtime;

namespace WeebDoCMF
{
    public class RazorPreCompilation : RazorPreCompileModule
    {
        public RazorPreCompilation(IApplicationEnvironment applicationEnvironment)
        {
            GenerateSymbols = string.Equals(applicationEnvironment.Configuration,
                                            "debug",
                                            StringComparison.OrdinalIgnoreCase);
        }
    }

    public class TagHelpersRazorPreCompilation : RazorPreCompileModule
    {
    }
}
