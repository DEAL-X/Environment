use deal;

// To open an application
command #RUN::string() => Core.System.Diagnostics.Process.Start(...arguments);

// To open an application as a dialog
command #DIALOG::string() => run(...arguments).WaitForExit();

// To close the application
command EXIT() => arguments.length > 0? (arguments[0].Kill !== "undefined"? arguments[0] : Core.System.Diagnostics.Process.GetProcessById(...arguments)).Kill() : application.Close();