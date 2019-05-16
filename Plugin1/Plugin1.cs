using CommonAPI.PluginData;

namespace Plugin1
{
    public class Plugin1 : PluginBase
    {
        public override IPluginAddInContract GetPluginViewContract()
        {
            return new AddInAdapter();
        }
    }
}