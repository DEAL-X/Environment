use deal\make;

command open() => will application.Engine.Resource.GetFileText(...arguments);

command save(object = null, path = null) => will application.Engine.Resource.SetFileText(path ?? temp.file, Make.string(object));

command append(object = null, path = null) => will application.Engine.Resource.AppendFileText(path ?? temp.file, Make.string(object));
command prepend(object = null, path = null) => will application.Engine.Resource.PrependFileText(path ?? temp.file, Make.string(object));