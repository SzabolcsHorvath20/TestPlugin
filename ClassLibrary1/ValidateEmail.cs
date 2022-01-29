using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace Plugins
{
    public class ValidateEmail : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
            serviceProvider.GetService(typeof(IPluginExecutionContext));
            Entity entity = (Entity)context.InputParameters["Target"];
            string name = (string)entity["lastname"];
            if ("Test".Equals(name))
            {
                entity["cr1f8_valid"] = true;
            }
        }
    }
}
