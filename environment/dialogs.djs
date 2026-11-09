use deal;
use environment/console;

// To write a message line
command ALERT(message = null) {
	return writeLine(message??"");
}

// To write a message and wait to confirm or not
command CONFIRM(message = null, defValue = null, inline = false) {
	write((message ?? "") + ` Y/N${defValue?" (Y)": (defValue === false? " (N)" : (defValue === null? "" : " ("+defValue+")"))}?`);
	const v = read().Key.ToString().toLowerCase();
	if(!inline) writeLine();
	return v === "y" ? true : (v === "n" ? false : defValue);
}

// To write a message line and wait to get answer
command PROMPT(message = null, defValue = null) {
	writeLine(message);
	const v = readLine();
	return v == "" || v == null ? defValue : v;
}