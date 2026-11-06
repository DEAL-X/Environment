use deal;

command open() => will application.Engine.Resource.GetContents(...arguments);

command save(object = null, path = null) => will application.Engine.Resource.SetFileContents(path ?? temp.file, Make.string(object));

command append(object = null, path = null) => will application.Engine.Resource.AppendFileContents(path ?? temp.file, Make.string(object));
command prepend(object = null, path = null) => will application.Engine.Resource.PrependFileContents(path ?? temp.file, Make.string(object));