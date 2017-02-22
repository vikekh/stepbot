using System.Collections.Generic;
using Vikekh.Stepbot.Common;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Clients.Base
{
    public abstract class BaseClient<TConfig> : IClient where TConfig : IConfig
    {
        protected TConfig Config { get; set; }

        private IDictionary<string, IModule> Modules { get; set; }

        protected string Version
        {
            get { return Utils.GetVersionString();  }
        }

        public abstract bool SendMessage(string channelId, string message);

        public BaseClient()
        {
            Config = Common.Config.GetConfig<TConfig>();
        }

        protected IModule GetModule(string name)
        {
            if (Modules == null) Modules = new Dictionary<string, IModule>();

            if (Modules.ContainsKey(name) && Modules[name] != null)
            {
                return Modules[name];
            }

            IModule module = null;

            if (name.Equals("WhereIs"))
            {
                module = new Modules.WhereIs.WhereIsModule();
            }

            if (module != null)
            {
                Modules[name] = module;
            }

            return module;
        }
    }
}
