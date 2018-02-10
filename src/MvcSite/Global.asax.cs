using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSite;

[assembly: PreApplicationStartMethod(typeof(PluginHelper), "UpdatePlugins")]
namespace MvcSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class PluginHelper
    {
        static PluginHelper()
        {
            var appSetting = ConfigurationManager.AppSettings["PluginFolder"];
            if (!string.IsNullOrWhiteSpace(appSetting))
            {
                if (!Directory.Exists(appSetting))
                {
                    Log("Bad PluginFolder: " + PluginFolder);
                    throw new Exception("Bad PluginFolder: " + PluginFolder);
                }

                PluginFolder = appSetting;
                return;
            }

            var pluginPath = Path.Combine(ApplicationPath, "Plugins");
            var exists = Directory.Exists(pluginPath);
            if (!exists)
            {
                Directory.CreateDirectory(pluginPath);
            }
            PluginFolder = pluginPath;
        }

        public static string PluginFolder { get; set; }

        public static string ApplicationPath
        {
            get
            {
                if (String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
                {
                    return AppDomain.CurrentDomain.BaseDirectory; //exe folder for WinForms, Consoles, Windows Services
                }
                else
                {
                    return AppDomain.CurrentDomain.RelativeSearchPath; //bin folder for Web Apps 
                }
            }
        }

        public static void UpdatePlugins()
        {
            Log("UpdatePlugins");

            if (string.IsNullOrWhiteSpace(PluginFolder) || !Directory.Exists(PluginFolder))
            {
                Log("Bad PluginFolder: " + PluginFolder);
                throw new Exception("Bad PluginFolder: " + PluginFolder);
            }

            var binDirectory = ApplicationPath;
            var pluginFiles = Directory.GetFiles(PluginFolder);
            if (pluginFiles.Length == 0)
            {
                Log("Find no plugins: " + PluginFolder);
                return;
            }

            foreach (var file in pluginFiles)
            {
                var fileInfo = new FileInfo(file);
                var targetFilePath = Path.Combine(binDirectory, fileInfo.Name);
                var exists = File.Exists(targetFilePath);
                if (exists)
                {
                    var existFileInfo = new FileInfo(targetFilePath);
                    if (fileInfo.LastWriteTime <= existFileInfo.LastWriteTime)
                    {
                        var message = string.Format("Plugin Version Same, Return: {0} {1}", existFileInfo.LastWriteTime, fileInfo.Name);
                        Log(message);
                        return;
                    }

                    Log(string.Format("Deleting Plugin : {0} {1}", existFileInfo.LastWriteTime, existFileInfo.Name));
                    File.Delete(targetFilePath);
                    Log(string.Format("Deleted Plugin : {0} {1}", existFileInfo.LastWriteTime, existFileInfo.Name));
                }


                Log(string.Format("Copying Plugin : {0} {1}", fileInfo.LastWriteTime, fileInfo.Name));
                File.Copy(file, targetFilePath);
                Log(string.Format("Copyed Plugin : {0} {1}", fileInfo.LastWriteTime, fileInfo.Name));
            }
        }

        private static void Log(object message)
        {
            Debug.WriteLine("[Plugin]" + message);
        }
    }
}
