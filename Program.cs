using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using MiMFa.Engine.Core;
using MiMFa.Engine.Put;

namespace MiMFa.Engine.DEAL.Environment
{
    public static class Program
    {
        public static int DisplayMode { get; set; } = 9;
        public static bool IsAlive { get; set; } = false;
        public static JavaScript.Engine Engine { get; set; }
        public static ScriptEngine Interpreter { get; set; }
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
                    Execute(arg);
            else do
            {
                Survival();
            } while (IsAlive);
            Final();
        }

        public static void Initial()
        {
            Engine = new JS.Engine();

            IsAlive = true;
            Interpreter = new V8ScriptEngine(
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

            Interpreter.DocumentSettings.SearchPath = System.Environment.CurrentDirectory + Path.DirectorySeparatorChar;
            Interpreter.DocumentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;

            Interpreter.AddHostType("application", typeof(Program));
            Interpreter.AddHostType("console", typeof(Console));

            Interpreter.AddHostObject("core", new HostTypeCollection(
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

            Engine.ExecuteStage = new Stage.Stage((object input, Engine compiler) => Interpreter.Evaluate((string)input));

            Engine.Logged += Logging;
            string title = $"MiMFa DEAL.JS [Version {Engine.Version}]";
            Console.Title = title;
            Console.WriteLine(title);
            Console.WriteLine("©️ MiMFa Corporation. All rights reserved.");
            Console.WriteLine("Visit the DEAL project at https://www.deal.mimfa.net.");

            var path = Engine.Resource.Normalize("initial.djs");
            if (Engine.Resource.Exists(path)) Execute(path);
            Console.WriteLine("");
        }
        public static void Survival()
        {
            var path = Engine.Resource.Normalize("survival.djs");
            if (Engine.Resource.Exists(path)) Execute(path);
            else
            {
                Console.Write("> ");
                Execute(Console.ReadLine());
            }
        }
        public static void Final()
        {
            var path = Engine.Resource.Normalize("final.djs");
            if (Engine.Resource.Exists(path)) Execute(path);
            else
            {
                Interpreter?.CollectGarbage(true);
                Interpreter?.Dispose();
                Engine?.Dispose();
            }
        }

        public static void Restart()
        {
            Clear();
            Reset();
        }
        public static void Reset()
        {
            Final();
            Initial();
        }
        public static void Clear()
        {
            Console.Clear();
        }
        public static void Stop()
        {
            IsAlive = false;
        }

        public static string Compile(string value)
        {
            try
            {
                return Engine.Compile(value).Content;
            }
            catch (Exception ex) { Engine.OnLogged(ex.Message, LogStatus.Error); }
            return null;
        }
        public static object Evaluate(string value)
        {
            try
            {
                return Engine.Execute(value);
            }
            catch (Exception ex) { Engine.OnLogged(ex.Message, LogStatus.Error); }
            return null;
        }
        public static void Execute(string value)
        {
            try
            {
                Display(Engine.Execute(value));
            }
            catch (Exception ex) { Engine.OnLogged(ex.Message, LogStatus.Error); }
        }

        public static void Display(object value)
        {
            // Resolve JavaScript Promises converted to .NET Tasks.
            while (value is Task task && DisplayMode > 0)
            {
                task.GetAwaiter().GetResult();

                var result = task.GetType().GetProperty("Result");
                value = result?.GetValue(task);
            }

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
                if (value is LogEventArgs)
                    Console.WriteLine(((LogEventArgs)value).ToString());
                else if (value is IEnumerable<LogEventArgs>)
                    foreach (var item in value as IEnumerable<LogEventArgs>)
                        Console.WriteLine(item.ToString());
                else
                    Console.WriteLine(value);
        }
        public static void DisplayJson(object value, Newtonsoft.Json.Formatting? flags = null)
        {
            if (value != null && !(value is Undefined) && !(value is VoidResult))
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value, flags??Newtonsoft.Json.Formatting.Indented));
        }
        public static void DisplayHybrid(object value)
        {
            if (
                value is string ||
                value is LogEventArgs ||
                value is IEnumerable<LogEventArgs>
            ) DisplayObject(value);
            else DisplayJson(value);
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
