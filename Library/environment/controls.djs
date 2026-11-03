use deal;

command snippetPath = 'Library\\snippet.djs';

command evaluate(value = null) => application.Engine.Execute(value ?? snippetPath);
command compile(value = null) => application.Engine.Compile(value ?? snippetPath).Content;
command execute(value = null) => print(application.Engine.Execute(value ?? snippetPath));
command snippet() => {
	let path = null;
	let mode = 0;
	if(arguments.length > 1){
		if("string" === typeof arguments[0]){
			path = arguments[0];
			mode = arguments[1];
		}
		else {
			mode = arguments[0];
			path= arguments[1];
		}
	} else if(arguments.length > 0){
		if("string" === typeof arguments[0])
			path = arguments[0];
		else mode = arguments[0];
	}
	if(path == null) local create(path = path ?? snippetPath, "// Write your snippet code here...");
	if(isEmpty(path)) path = prompt("Please indicate an address for the snippet file:", snippetPath);
	if(!local exists(path) and local create(path))
		writeline(`The file \"${path}\" is created successfully!`);
	dialog("notepad", path);
	if(mode > 8 || (mode < 1 && confirm(`Are you sure to execute your snippet on \"${Local.name(path)}\"?`))) return execute(path);
	else if(mode > 0 || (mode < 1 && confirm(`Do you want to compile that snippet instead?`))) return compile(path);
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
command save(object = null, path = null) => will local write(path ?? temp.file, object);
command append(object = null, path = null) => will local append(path ?? temp.file, object);
command prepend(object = null, path = null) => will local prepend(path ?? temp.file, object);