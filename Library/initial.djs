use deal;

command reset() => application.Initial();
command compile(value = null) => application.Engine.Compile(value).Content;
command execute(value = null) => print(application.Engine.Execute(value));

command compilemode() => application.CompileMode = true;

// To clear screen
command cls() => console.Clear();

// To close the application
command exit() => application.Close();

// To convert then write an object
command print(object = null) => application.Show(object??"");

// To write a message
command write(message = null) => console.Write(message??"");
// To write a message line
command writeline(message = null) => console.WriteLine(message??"");

// To read a character
command read() => console.ReadKey();
// To read a line
command readline() => console.ReadLine();

// To write a message line
command alert(message = null) {
	return writeline(message??"");
}

// To write a message and wait to confirm or not
command confirm(message = null, defValue = null, inline = false) {
	write((message ?? "") + ` Y/N${defValue?" (Y)": (defValue === false? " (N)" : (defValue === null? "" : " ("+defValue+")"))}?`);
	const v = read().Key.ToString().toLowerCase();
	if(!inline) writeline();
	return v === "y" ? true : (v === "n" ? false : defValue);
}

// To write a message line and wait to get answer
command prompt(message = null, defValue = null) {
	writeline(message);
	const v = readline();
	return v == "" || v == null ? defValue : v;
}