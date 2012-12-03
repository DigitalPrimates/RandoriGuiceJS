using System.Reflection;
using System.Runtime.InteropServices;
using SharpKit.JavaScript;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("RandoriGuiceJS")]
[assembly: AssemblyDescription("JavaScript implemention of the Google Guice project to work with Randori metadata")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("LTN Consulting, Inc. /dba Digital Primates®")]
[assembly: AssemblyProduct("RandoriGuiceJS")]
[assembly: AssemblyCopyright("Copyright ©  2012 LTN Consulting dba/ Digital Primates® 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("50e155a0-0382-4422-afb3-663a58b1243d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: JsExport(GenerateSourceMaps = false)]
[assembly: JsType(JsMode.Prototype, OmitCasts = true, NativeJsons = true)]

