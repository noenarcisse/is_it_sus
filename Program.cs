using System.Text.RegularExpressions;
using System.Text;
using is_it_sus;


//TEST ZONE

var suspectFinder = new SuspectFinderService();
suspectFinder.LoadExtensionsData();
// foreach(var l in suspectFinder.Languages)
// {
//     Console.WriteLine(l);
// }

suspectFinder.FindExtension(".php");
suspectFinder.FindExtension(".jar");
suspectFinder.FindExtension(".txt");
suspectFinder.FindExtension(".css");
suspectFinder.FindExtension(".cs");

Console.WriteLine($"""
{string.Join(",", suspectFinder.FoundExtentions.Keys)}
""");

//END TEST ZONE



// if(args.Length <= 0 || string.IsNullOrWhiteSpace(args[0]))
// {
//     Console.WriteLine("Usage is: isitsus");
//     return 1; 
// }

// string extension = args[0];
// string currentPath = Directory.GetCurrentDirectory();

// IEnumerable<string> files = Directory.EnumerateFiles(currentPath, $"*.{extension}", SearchOption.AllDirectories);

// int totalFilesCounter = 0;
// int fileWithRefactoCounter = 0;


// //Regex reg = new(@"//\s*(refacto|todo).*(\r?\n\s*//.*)*", RegexOptions.IgnoreCase);
// Regex regRefacto = new(@"//\s*(refacto|todo)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
// Regex addComment = new(@"^//", RegexOptions.IgnoreCase | RegexOptions.Compiled);

// //multi threading edition
// await Parallel.ForEachAsync(files, async(file, cancelToken) =>
// {
//     bool hasRefactoInFile = false;
//     var content = new StringBuilder();

//     try
//     {
//         bool isSearchingForAdditionalComments = false;
//         int lineCounter = 1;

//         await foreach (var line in File.ReadLinesAsync(file, cancelToken))
//         {
//             var currentLine = line.TrimStart();

//             if(isSearchingForAdditionalComments && addComment.IsMatch(currentLine))
//             {
//                 content.AppendLine($"{currentLine}");
//             }
//             else
//             {
//                 isSearchingForAdditionalComments = false;
//             }

//             if(regRefacto.IsMatch(currentLine))
//             {
//                 content.AppendLine($"ON LINE {lineCounter}");
//                 content.AppendLine($"{currentLine}");

//                 isSearchingForAdditionalComments = true;
//                 hasRefactoInFile = true;
//             }

//             lineCounter++;
  
//         }

//     }
//     catch (Exception except)
//     {
//         Console.WriteLine("Could not read the content in file "+Path.GetFileNameWithoutExtension(file));
//         Console.WriteLine(except.Message);
//     }

//     if(hasRefactoInFile)
//     {
//         Console.WriteLine($"{file} \n {content}");
//         Interlocked.Increment(ref fileWithRefactoCounter);
//     }

//     Interlocked.Increment(ref totalFilesCounter);

// });

// Console.WriteLine($"{fileWithRefactoCounter} / {totalFilesCounter} fichiers avec des refactos");

return 0;