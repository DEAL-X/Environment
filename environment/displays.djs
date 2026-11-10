use deal/core;

// To clear screen
command cls() => console.Clear();

// To convert then write an object
command print(object = null) => application.DisplayHybrid(object);

// To write a message
command write(message = null) => console.Write(message??"");
// To write a message line
command writeLine(message = null) => console.WriteLine(message??"");

// To read a character
command read() => console.ReadKey();
// To read a line
command readLine() => console.ReadLine();