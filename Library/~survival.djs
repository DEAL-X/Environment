console.Write("> ");
if(application.Environment.ContainsKey("SwitchMode")){
	let d = console.ReadLine();
	if(d !== null && d.trim().toLowerCase() == "switchmode") application.Show(application.Engine.Execute(d));
	else application.Engine.Compile(d).Content;
}
else application.Show(application.Engine.Execute(readline()));