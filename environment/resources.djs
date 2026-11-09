use deal;

command OPEN() => will application.Engine.Resource.GetContents(...arguments);

command SAVE(object = null, path = null) => will application.Engine.Resource.SetFileContents(path ?? temp.file, MAKE.string(object));

command APPEND(object = null, path = null) => will application.Engine.Resource.AppendFileContents(path ?? temp.file, MAKE.string(object));
command PREPEND(object = null, path = null) => will application.Engine.Resource.PrependFileContents(path ?? temp.file, MAKE.string(object));