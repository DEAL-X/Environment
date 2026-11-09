use deal;
use environment/console;

LOG.display = function (title = null, message = null) => writeLine(LOG.message(title, message));
LOG.subject = function (message = null) => LOG.display(message, "");
LOG.error = function (message = null) => LOG.display("Error", message);
LOG.success = function (message = null) => LOG.display("Success", message);
LOG.warn = function (message = null) => LOG.display("Warning", message);
LOG.info = function (message = null) => LOG.display(null, message);