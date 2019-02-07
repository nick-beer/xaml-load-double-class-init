using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;

namespace XamlLoad
{
    public class Program
    {
        // To reproduce the bug, put a breakpoint in the registration of the dependency property
        // in the Numeric<TData> class of the ControlsBase project.  You'll see that when we try
        // to get the Style out of the resource dictionary below, we run the static initializer
        // on the Numeric<TData> class twice - twice for the same type in the same ALC.

        [STAThread]
        public static void Main()
        {
            var cxt = new LoadAllAssembliesContext();

            var assembly = cxt.LoadFromAssemblyPath(typeof(Program).Assembly.Location);
            var type = assembly.GetType(typeof(Program).FullName);
            type.GetMethod(nameof(Program.DoXamlLoad)).Invoke(null, null);
        }

        public static void DoXamlLoad()
        {
            var n = new CustomControls.DialogNumeric();

            var uri = new Uri("Resources.xaml", UriKind.Relative);
            var dict = new ResourceDictionary { Source = uri };
            var style = dict[n.GetType()];
        }
    }

    public sealed class LoadAllAssembliesContext : AssemblyLoadContext
    {
        public LoadAllAssembliesContext() { }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            return LoadFromAssemblyPath(assembly.Location);
        }
    }
}