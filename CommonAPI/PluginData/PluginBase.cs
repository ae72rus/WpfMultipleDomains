namespace CommonAPI.PluginData
{
    public abstract class PluginBase : IPlugin
    {
        public abstract IPluginAddInContract GetPluginViewContract();
    }
}