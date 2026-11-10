use deal/core;

// To open an application
command #run::string() => Core.System.Diagnostics.Process.Start(...arguments);

// To open an application as a dialog
command #dialog::string() => run(...arguments).WaitForExit();

// To close the application
command exit() => arguments.length > 0? (arguments[0].Kill !== "undefined"? arguments[0] : Core.System.Diagnostics.Process.GetProcessById(...arguments)).Kill() : application.Close();