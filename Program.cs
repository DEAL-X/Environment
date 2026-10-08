using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace MiMFa.Engine.DEAL.Environment
{
    static class Program
    {
        public static bool IsAlive = false;
        public static bool CompileMode = false;
        /// <summary>W
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Console.OutputEncoding =
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var compiler = new JS.Engine();
            InitializeCompiler(compiler);
            compiler.Logged += Logging;
            string title = $"MiMFa DEAL.JS [Version {compiler.Version}]";
            Console.Title = title;
            Console.WriteLine(title);
            Console.WriteLine("©️ MiMFa Corporation. All rights reserved.");
            Console.WriteLine("Visit the DEAL project at https://www.deal.mimfa.net.");
            Console.WriteLine("");

            var execute = new Action<string>((code) =>
            {
                try
                {
                    if(CompileMode) Console.Error.WriteLine(compiler.Compile(new Input(code)).Content);
                    else Show(compiler.Execute(new Input(code)));
                }
                catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
            });
            var executeFile = new Action<string>((path) =>
            {
                string code;
                if (File.Exists(path) && !string.IsNullOrWhiteSpace(code = File.ReadAllText(path))) execute(code);
            });

            executeFile(compiler.SourceDirectory+"initial.djs");
            do
            {
                Console.Write("> ");
                execute(Console.ReadLine());
                executeFile(compiler.SourceDirectory + "manual.djs");
            } while (IsAlive);
            executeFile(compiler.SourceDirectory + "final.djs");
        }

        private static void InitializeCompiler(MiMFa.Engine.Engine compiler)
        {
            IsAlive = true;
            var interpretor = new V8ScriptEngine();

            interpretor.AddHostObject("clearEngine", new Action(() => InitializeCompiler(compiler)));

            interpretor.AddHostObject("notExecute", new Action(() => CompileMode = true));

            interpretor.AddHostObject("closeScreen", new Action(() => IsAlive = false));
            interpretor.AddHostObject("clearScreen", new Action(() => Console.Clear()));
            interpretor.AddHostObject("printScreen", new Action<object>((object message) => Console.WriteLine(message)));

            interpretor.AddHostObject("compiler", compiler);

            interpretor.AddHostType("Compiler", typeof(MiMFa.Engine.Engine));
            interpretor.AddHostType("console", typeof(Console));

            compiler.ExecuteStage = new Stage((object input, MiMFa.Engine.Engine compiler) => interpretor.Evaluate((string)input));
        }

        private static void Show(object value)
        {
            if (value != null && !(value is Undefined) && !(value is VoidResult))
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented));
        }

        private static void Logging(MiMFa.Engine.Engine compiler, Core.LogEventArgs e)
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
