using System;
using System.Linq;
using System.Reflection;
using CommonAPI.PluginData;

namespace WpfMultipleDomains
{
    public class PluginWrapper : MarshalByRefObject, IPluginWrapper
    {
        private readonly IPlugin _nativePlugin;
        public bool IsLoaded { get; }

        public PluginWrapper(string dllFullPath)
        {
            var asm = Assembly.LoadFile(dllFullPath);
            var pluginImplType = asm.GetExportedTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
            if (pluginImplType == null)
            {
                return;
            }

            _nativePlugin = Activator.CreateInstance(pluginImplType) as IPlugin;
            IsLoaded = true;
        }

        public IPluginAddInContract GetPluginViewContract()
        {
            return _nativePlugin?.GetPluginViewContract();
        }

    }
}