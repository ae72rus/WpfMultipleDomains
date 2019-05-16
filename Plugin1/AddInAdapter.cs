using System.AddIn.Pipeline;
using CommonAPI.PluginData;

namespace Plugin1
{
    [AddInAdapter]
    public class AddInAdapter: PluginAddInAdapterBase<PluginView>
    {
        public AddInAdapter() : base(new PluginView())
        {
        }
    }
}