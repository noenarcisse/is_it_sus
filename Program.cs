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

// suspectFinder.FindExtension(".php");
// suspectFinder.FindExtension(".jar");
// suspectFinder.FindExtension(".txt");
// suspectFinder.FindExtension(".css");
// suspectFinder.FindExtension(".cs");

// Console.WriteLine($@"""
// {string.Join(",", suspectFinder.FoundExtentions.Keys)}
// Regex : /{string.Join("|",suspectFinder.FoundExtentions[".php"].Keywords)}/gi
// """);
//END TEST ZONE


// on passe isitsus
// ou isitsus [path]

string path;

if(args.Length <= 0 || string.IsNullOrWhiteSpace(args[0]))
    path = Directory.GetCurrentDirectory();

path = args[0];

if(!Path.Exists(path))
    return 1;




IEnumerable<string> files = Directory   .EnumerateFiles(path, $"*.*", SearchOption.AllDirectories)
                                        .Where(f => suspectFinder.HasExtension(Path.GetExtension(f)));

int totalFilesCounter = 0;
int susFileCounter = 0;


// Regex regRefacto = new(@"//\s*(refacto|todo)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
// Regex addComment = new(@"^//", RegexOptions.IgnoreCase | RegexOptions.Compiled);

//multi threading edition
await Parallel.ForEachAsync(files, async(file, cancelToken) =>
{
    bool isSusFile = false;
    var content = new StringBuilder();

    string ext = Path.GetExtension(file);
    suspectFinder.FindExtension(ext);

    string keywordsPattern = Regex.Escape(string.Join("|", suspectFinder.FoundExtentions[ext].Keywords));

    Regex susRegex = new(keywordsPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    try
    {
        int lineCounter = 1;

        await foreach (var line in File.ReadLinesAsync(file, cancelToken))
        {
            //hmm?
            var currentLine = line;

            if(susRegex.IsMatch(currentLine))
            {
                content.AppendLine($"ON LINE {lineCounter}");
                content.AppendLine($"{currentLine}");

                isSusFile = true;
            }
            lineCounter++;
        }

    }
    catch (Exception except)
    {
        Console.WriteLine("Could not read the content in file "+Path.GetFileNameWithoutExtension(file));
        Console.WriteLine(except.Message);
    }

    if(isSusFile)
    {
        Console.WriteLine($"{file} \n {content}");
        Interlocked.Increment(ref susFileCounter);
    }

    Interlocked.Increment(ref totalFilesCounter);

});



Console.WriteLine($"""
Langages détectés : {string.Join(", ", suspectFinder.ListAllFoundLanguages())}
{susFileCounter} / {totalFilesCounter} fichiers avec des refactos
""");

return 0;