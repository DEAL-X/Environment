use deal/data;
use deal/make;
use deal/select;
use environment;

print("DEAL's core, data, make and select modules are used!");

command switchMode(name = "SwitchMode") {
	if(application.Has(name)) return application.Remove(name);
	else application.Add(name, true);
	return false;
}

print(application.Engine.Errors);