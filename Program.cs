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
        public static bool Survive = false;
        public static bool CompileMode = false;
        public static Engine Engine;

        /// <summary>W
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Console.OutputEncoding =
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Initial();
            do
            {
                Survival();
            } while (Survive);
            Final();
        }

        public static void Initial()
        {
            Engine = new JS.Engine();

            Survive = true;
            var interpretor = new V8ScriptEngine();

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

            ExecuteFile(Engine.SourceDirectory + "initial.djs");
        }
        public static void Survival()
        {
            ExecuteFile(Engine.SourceDirectory + "survival.djs");
        }
        public static void Final()
        {
            ExecuteFile(Engine.SourceDirectory + "final.djs");
        }

        public static void ExecuteFile(string path)
        {
            string code;
            if (File.Exists(path) && !string.IsNullOrWhiteSpace(code = File.ReadAllText(path))) Execute(code);
        }
        public static void Execute(string code)
        {
            try
            {
                if (CompileMode) Console.Error.WriteLine(Engine.Compile(new Input(code)).Content);
                else Show(Engine.Execute(new Input(code)));
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }

        public static void Show(object value)
        {
            if (value != null && value.ToString() != "" && !(value is Undefined) && !(value is VoidResult))
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented));
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
