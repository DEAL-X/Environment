use deal;

command cle() => #clearEngine();
command compilemode() => #notExecute();

command exit() => #closeScreen();
command cls() => #clearScreen();
command print(message = null) {
	if(message) return #printScreen(message);
	return print;
}