use deal;

command compilemode() { #notExecute(); }

command cle() { #clearEngine(); }
command #compile::string(value) { #compileEngine(value); }
command #execute::string(value) { #executeEngine(value); }

command exit() { #closeScreen(); }
command cls() { #clearScreen(); }
command print(message = null) {
	if(message) return #printScreen(message);
	return print;
}