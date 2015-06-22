using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Brace FREE Library")]
[assembly: AssemblyProduct("Brace Shared Library")]
[assembly: AssemblyCopyright("GNU General Public License")]
[assembly: AssemblyTrademark("")]
[assembly: NeutralResourcesLanguage("en-US")]

// TODO: only while other warnings are not fixed
[assembly: CLSCompliant(false)]

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.00606.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
