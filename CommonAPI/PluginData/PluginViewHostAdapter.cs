using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Windows;

namespace CommonAPI.PluginData
{
    [HostAdapter]
    public class PluginViewHostAdapter : IAddInHostView
    {
        readonly IPluginAddInContract _wpfAddInContract;
        ContractHandle _wpfAddInContractHandle;

        public PluginViewHostAdapter(IPluginAddInContract wpfAddInContract)
        {
            // Adapt the contract (IWPFAddInContract) to the host application's
            // view of the contract (IWPFAddInHostView)
            _wpfAddInContract = wpfAddInContract;

            // Prevent the reference to the contract from being released while the
            // host application uses the add-in
            _wpfAddInContractHandle = new ContractHandle(wpfAddInContract);
        }

        public FrameworkElement GetAddInUI()
        {
            // Convert the INativeHandleContract that was passed from the add-in side
            // of the isolation boundary to a FrameworkElement
            var inhc = _wpfAddInContract.QueryContract(typeof(INativeHandleContract).AssemblyQualifiedName) as INativeHandleContract;
            var fe = FrameworkElementAdapters.ContractToViewAdapter(inhc);
            return fe;
        }
    }

    public interface IAddInHostView
    {
        FrameworkElement GetAddInUI();
    }
}