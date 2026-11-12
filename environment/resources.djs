use deal\make;

command open() => will application.Engine.Resource.GetFileText(...arguments);

command save(path = null, object = null) => will application.Engine.Resource.SetFileText(path ?? temp.file, Make.string(object));

command append(path = null, object = null) => will application.Engine.Resource.AppendFileText(path ?? temp.file, Make.string(object));
command prepend(path = null, object = null) => will application.Engine.Resource.PrependFileText(path ?? temp.file, Make.string(object));