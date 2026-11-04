console.Write("> ");
if(application.Has("SwitchMode")){
	let d = console.ReadLine();
	if(d !== null && d.trim().toLowerCase() == "switchmode") application.Display(application.Engine.Execute(d));
	else application.Engine.Compile(d).Content;
}
else application.Display(application.Engine.Execute(console.ReadLine()));