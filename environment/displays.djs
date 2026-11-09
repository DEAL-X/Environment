use deal;

// To clear screen
command CLS() => console.Clear();

// To convert then write an object
command PRINT(object = null) => application.DisplayHybrid(object);

// To write a message
command WRITE(message = null) => console.Write(message??"");
// To write a message line
command WRITELINE(message = null) => console.WriteLine(message??"");

// To read a character
command READ() => console.ReadKey();
// To read a line
command READLINE() => console.ReadLine();