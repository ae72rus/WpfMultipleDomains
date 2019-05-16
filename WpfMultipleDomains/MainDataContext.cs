using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommonAPI;
using CommonAPI.PluginData;

namespace WpfMultipleDomains
{
    public class MainDataContext : DataContextBase
    {
        private const string _pluginsDir = "Plugins";
        private IReadOnlyList<IPlugin> _loadedPlugins;
        private ICommand _loadPluginsCommand;
        public ObservableCollection<FrameworkElement> PluginsViews { get; } = new ObservableCollection<FrameworkElement>();
        public string AppDomainName { get; } = AppDomain.CurrentDomain.FriendlyName;
        public ICommand LoadPluginsCommand => _loadPluginsCommand ?? (_loadPluginsCommand = new RelayCommand(loadPlugins));

        public MainDataContext()
        {
            if (!Directory.Exists(_pluginsDir))
            {
                Directory.CreateDirectory(_pluginsDir);
            }
        }

        private void loadPlugins()
        {
            var pluginsDirs = Directory.GetDirectories(_pluginsDir);
            _loadedPlugins = pluginsDirs.Select(loadPlugin).Where(plugin => plugin != null).ToList();
            foreach (var loadedPlugin in _loadedPlugins)
            {
                var contractSource = loadedPlugin.GetPluginViewContract();
                var hostAdapter = new PluginViewHostAdapter(contractSource);
                var fe = hostAdapter.GetAddInUI();
                if (fe != null)
                {
                    PluginsViews.Add(fe);
                }
            }
        }

        private IPlugin loadPlugin(string pluginDir)
        {
            var dlls = Directory.GetFiles(pluginDir, "*.dll");
            foreach (var dll in dlls)
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dll);
                var domainName = Path.GetFileNameWithoutExtension(dll);
                var domain = getPluginDomain(domainName, pluginDir);
                var wrapperType = typeof(PluginWrapper);
                var pluginWrapper = (PluginWrapper)domain.CreateInstanceAndUnwrap(wrapperType.Assembly.FullName,
                                                                                    wrapperType.FullName ?? string.Empty,
                                                                                    true,
                                                                                    BindingFlags.Default,
                                                                                    null,
                                                                                    new object[] { fullPath },
                                                                                    CultureInfo.InvariantCulture,
                                                                                    new object[0]);
                if (pluginWrapper.IsLoaded)
                {
                    return pluginWrapper;
                }

                AppDomain.Unload(domain);
            }

            return null;
        }

        private AppDomain getPluginDomain(string name, string pluginDir)
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.PrivateBinPath = pluginDir;
            var retv = AppDomain.CreateDomain(name, null, setup);
            return retv;
        }
    }
}