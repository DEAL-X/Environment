use deal;

command snippetPath = 'snippet.djs';

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
		writeLine(`The file \"${path}\" is created successfully!`);
	run("notepad", path);

	if(mode > 1 || (mode < 1 && confirm(`Are you sure to evaluate your snippet on \"${Local.name(path)}\"?`))) return evaluate(path);
	else if(mode > 0 || (mode < 1 && confirm(`Do you want to compile that snippet instead?`))) return compile(path);
}

// To restart the application
command reset() => {
	application.Final();
	application.Initial();
}