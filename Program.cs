using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using MiMFa.Engine.Put;

namespace MiMFa.Engine.DEAL.Environment
{
    public static class Program
    {
        public static int DisplayMode { get; set; } = 9;
        public static bool Survive { get; set; } = false;
        public static JavaScript.Engine Engine { get; set; }
        public static Dictionary<string, object> Environment { get; set; }  = new Dictionary<string, object>();
        public static object Add(string key, object value = null) => Environment[key] = value;
        public static bool Remove(string key) => Environment.Remove(key);
        public static bool Has(string key) => Environment.ContainsKey(key);


        /// <summary>W
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Console.OutputEncoding =
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Initial();
            if (args.Length > 0)
                foreach (var arg in args)
                    ExecuteFile(arg);
            else do
            {
                Survival();
            } while (Survive);
            Final();
        }

        public static void Initial()
        {
            Engine = new JS.Engine();

            Survive = true;
            var interpretor = new V8ScriptEngine(
                V8ScriptEngineFlags.EnableArrayConversion |
                V8ScriptEngineFlags.EnableDateTimeConversion |
                V8ScriptEngineFlags.EnableDynamicModuleImports |
                V8ScriptEngineFlags.EnableStringifyEnhancements |
                V8ScriptEngineFlags.MarshalAllInt64AsBigInt |
                V8ScriptEngineFlags.SetTimerResolution |
                V8ScriptEngineFlags.UseCaseInsensitiveMemberBinding |
                V8ScriptEngineFlags.UseSynchronizationContexts |
                V8ScriptEngineFlags.EnableValueTaskPromiseConversion |
                V8ScriptEngineFlags.EnableTaskPromiseConversion
            );

            interpretor.DocumentSettings.SearchPath = System.Environment.CurrentDirectory + Path.DirectorySeparatorChar;
            interpretor.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;

            interpretor.AddHostType("application", typeof(Program));
            interpretor.AddHostType("console", typeof(Console));

            interpretor.AddHostObject("Core", new HostTypeCollection(
                    "mscorlib",
                    "System",
                    "System.Core",
                    "System.Xml",
                    "System.Linq",
                    "System.Data",
                    "System.Threading",
                    "System.Collections",
                    "System.Reflection",
                    "System.Web",
                    "System.Windows",
                    "Newtonsoft.Json",
                    "MiMFa DEAL - Engine",
                    "MiMFa DEAL.JS - Engine"
               )
            );

            Engine.ExecuteStage = new Stage.Stage((object input, Engine compiler) => interpretor.Evaluate((string)input));

            Engine.Logged += Logging;
            string title = $"MiMFa DEAL.JS [Version {Engine.Version}]";
            Console.Title = title;
            Console.WriteLine(title);
            Console.WriteLine("©️ MiMFa Corporation. All rights reserved.");
            Console.WriteLine("Visit the DEAL project at https://www.deal.mimfa.net.");
            Console.WriteLine("");

            ExecuteFile(Engine.ResourceProvider.Resolve("initial.djs"));
        }
        public static void Survival()
        {
            var path = Engine.ResourceProvider.Resolve("survival.djs");
            if (Engine.ResourceProvider.Exists(path)) ExecuteFile(path);
            else
            {
                Console.Write("> ");
                Execute(Console.ReadLine());
            }
        }
        public static void Final()
        {
            var path = Engine.ResourceProvider.Resolve("final.djs");
            if (Engine.ResourceProvider.Exists(path)) ExecuteFile(path);
            else
            {
                Engine.Dispose();
            }
        }

        public static void ExecuteFile(string path)
        {
            string code;
            if (File.Exists(path) && !string.IsNullOrWhiteSpace(code = File.ReadAllText(path))) Execute(code, path);
        }
        public static void Execute(string code, string path = null)
        {
            try
            {
                Display(Engine.Execute(new Input(code, path)));
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }

        public static void Display(object value)
        {
            switch (DisplayMode)
            {
                case 0:
                    break;
                case 1:
                    Console.WriteLine(value);
                    break;
                case 2:
                    DisplayObject(value);
                    break;
                case 3:
                    DisplayJson(value, Newtonsoft.Json.Formatting.None);
                    break;
                case 4:
                    DisplayJson(value, Newtonsoft.Json.Formatting.Indented);
                    break;
                case 9:
                default:
                    DisplayHybrid(value);
                    break;
            }
        }
        public static void DisplayObject(object value)
        {
            if (!(value is Undefined) && !(value is VoidResult))
                Console.WriteLine(value);
        }
        public static void DisplayJson(object value, Newtonsoft.Json.Formatting? flags = null)
        {
            if (value != null && !(value is Undefined) && !(value is VoidResult))
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value, flags??Newtonsoft.Json.Formatting.Indented));
        }
        public static void DisplayHybrid(object value)
        {
            if (value is string) DisplayObject(value);
            else DisplayJson(value);
        }

        public static void Close()
        {
            Survive = false;
        }

        private static void Logging(Engine engine, Core.LogEventArgs e)
        {
            switch (e.Status)
            {
                case Core.LogStatus.Error:
                    Console.Error.WriteLine(e);
                    break;
                default:
                    break;
            }
        }

    }
}
