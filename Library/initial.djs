use deal;
use environment;

command switchmode(name = "SwitchMode") {
	if(application.Environment.ContainsKey(name)) {
		application.Environment.Remove(name);
		return true;
	}
	else application.Environment.Add(name, true);
	return false;
}