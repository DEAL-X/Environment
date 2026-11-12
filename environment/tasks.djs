use deal/core;

// To open an application
command #start::string() {
	if(arguments.length < 1) return;
	Core.System.Diagnostics.Process.Start(...arguments);
}

// To open an application as a dialog
command #stay::string() {
	if(arguments.length < 1) return;
	start(...arguments).WaitForExit();
}

// To close the application
command stop() {
	if(arguments.length > 0)
		arguments[0].Kill !== "undefined"? arguments[0].Kill() :
		arguments[0].kill !== "undefined"? arguments[0].kill() :
		arguments[0].Stop !== "undefined"? arguments[0].Stop() :
		arguments[0].stop !== "undefined"? arguments[0].stop() :
		arguments[0].Dispose !== "undefined"? arguments[0].Dispose() :
		arguments[0].dispose !== "undefined"? arguments[0].dispose() :
		Core.System.Diagnostics.Process.GetProcessById(...arguments).Kill();
	else application.Stop();
}