using CommonAPI.PluginData;

namespace WpfMultipleDomains
{
    public interface IPluginWrapper : IPlugin
    {
        bool IsLoaded { get; }
    }
}