use deal;

command tempFolder = 'temp\\';
command tempFile = 'temp.tmp';
command snippetPath = 'Library\\snippet.djs';

command compile(value = null) => application.Engine.Compile(value ?? snippetPath).Content;
command evaluate(value = null) => application.Engine.Execute(value ?? snippetPath);
command execute(value = null) => print(application.Engine.Execute(value ?? snippetPath));
command snippet(path = null) => {
	if(path == null) local create(path = path ?? snippetPath, "// Write your snippet code here...");
	if(isEmpty(path)) path = prompt("Please indicate an address for the snippet file:", snippetPath);
	if(!local exists(path) and local create(path))
		writeline(`The file \"${path}\" is created successfully!`);
	dialog("notepad", path);
	if(confirm(`Are you ready to execute your snippet on \"${Local.name(path)}\"?`)) return execute(path);
}

// To restart application
command reset() => application.Initial();

// To open an application
command #open::string() => Core.System.Diagnostics.Process.Start(...arguments);

// To open an application as a dialog
command #dialog::string() => {
	open(...arguments).WaitForExit();
}

// To close the application
command close() => arguments.length > 0? Core.System.Diagnostics.Process.GetProcessById(...arguments).Kill() : application.Close();

// To save an object
command save(object = null, path = null) => new Promise((s,r)=>s(local write(path ?? tempFile, object)));
command append(object = null, path = null) => new Promise((s,r)=>s(local append(path ?? tempFile, object)));
command prepend(object = null, path = null) => new Promise((s,r)=>s(local prepend(path ?? tempFile, object)));