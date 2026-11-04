use deal;
use environment;

command switchmode(name = "SwitchMode") {
	if(application.Has(name)) return application.Remove(name);
	else application.Add(name, true);
	return false;
}