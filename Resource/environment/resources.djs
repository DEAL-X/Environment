use deal;

command open() => will application.Engine.ResourceProvider.GetContents(...arguments);

command save(object = null, path = null) => will application.Engine.ResourceProvider.SetFileContents(path ?? temp.file, Make.string(object));

command append(object = null, path = null) => will application.Engine.ResourceProvider.AppendFileContents(path ?? temp.file, Make.string(object));
command prepend(object = null, path = null) => will application.Engine.ResourceProvider.PrependFileContents(path ?? temp.file, Make.string(object));