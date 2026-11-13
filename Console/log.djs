use deal/data/log;
use console;

Log.display = function (title = null, message = null) => writeLine(Log.message(title, message));
Log.subject = function (message = null) => Log.display(message, "");
Log.error = function (message = null) => Log.display("Error", message);
Log.success = function (message = null) => Log.display("Success", message);
Log.warn = function (message = null) => Log.display("Warning", message);
Log.info = function (message = null) => Log.display(null, message);