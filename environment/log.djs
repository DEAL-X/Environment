use deal;
use environment/console;

command Log = {};

Log.message = function (title = null, message = null) {
	write((new Date()).toLocaleString() + "\t");
	if(title != null) write(title);
	if(message != null) write(message);
	writeLine();
}
Log.subject = function (message = null) => Log.message(message, "");
Log.error = function (message = null) => Log.message("Error:\t", message);
Log.success = function (message = null) => Log.message("Success:\t", message);
Log.warn = function (message = null) => Log.message("Warning:\t", message);
Log.warning = function (message = null) => Log.warn(message);
Log.info = function (message = null) => Log.message(null, message);