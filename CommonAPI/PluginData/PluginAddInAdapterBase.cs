using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Security.Permissions;

namespace CommonAPI.PluginData
{
    [AddInAdapter]
    public abstract class PluginAddInAdapterBase<T> : ContractBase, IPluginAddInContract where T : BasePluginView
    {
        private readonly T _addInView;

        protected PluginAddInAdapterBase(T addInView)
        {
            _addInView = addInView ?? throw new ArgumentNullException(nameof(addInView));
        }

        public override IContract QueryContract(string contractIdentifier)
        {
            return contractIdentifier.Equals(typeof(INativeHandleContract).AssemblyQualifiedName)
                ? FrameworkElementAdapters.ViewToContractAdapter(_addInView)
                : base.QueryContract(contractIdentifier);
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public IntPtr GetHandle()
        {
            return FrameworkElementAdapters.ViewToContractAdapter(_addInView)?.GetHandle() ?? IntPtr.Zero;
        }
    }
}