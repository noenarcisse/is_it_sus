using System.Text.RegularExpressions;
using System.Text;
using is_it_sus;
using System.Collections.Concurrent;


var suspectFinder = new SuspectFinderService();
suspectFinder.LoadExtensionsData();


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

//REFACTO
// logs in one file, concurrent bag before saving
Directory.CreateDirectory("./logs");
var logDir = Directory.CreateDirectory($@"./logs/Logs_{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}");


await Parallel.ForEachAsync(files, async(file, cancelToken) =>
{
    bool isSusFile = false;
    var content = new StringBuilder();

    string ext = Path.GetExtension(file);
    suspectFinder.FindExtension(ext);


    var escapedKw = suspectFinder.FoundExtentions[ext].Keywords.Select(k => Regex.Escape(k));
    string keywordsPattern = string.Join("|", escapedKw);

    Regex susRegex = new(keywordsPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    try
    {
        int lineCounter = 1;

        await foreach (var currentLine in File.ReadLinesAsync(file, cancelToken))
        {
            if(susRegex.IsMatch(currentLine))
            {
                content.AppendLine($"ON LINE {lineCounter}");
                content.AppendLine($"{currentLine}");

                isSusFile = true;
            }
            lineCounter++;
        }

    }
    catch (OperationCanceledException) { }
    catch (Exception except)
    {
        Console.WriteLine("Could not read the content in file "+Path.GetFileNameWithoutExtension(file));
        Console.WriteLine(except.Message);
    }

    if(isSusFile)
    {
        Console.WriteLine($"{file} \n {content}");
        Interlocked.Increment(ref susFileCounter);

        //Faut ConcurrentBag<T>
        //ou ConcurrentDictionary<k,v> pour prepa un result en multithread
        //comme ca on evite l'IO continues de fichiers qui rend l'antivirus fou

        //File.WriteAllText($@"{logDir.FullName}/susReport_{Path.GetFileNameWithoutExtension(file)}_{Guid.NewGuid()}.txt", $"{file} \n {content}");
    }

    Interlocked.Increment(ref totalFilesCounter);

});


//puis ici on ecrit une seule fois le log en sortie
//File.WriteAllText($@"{logDir.FullName}/susReport_{Path.GetFileNameWithoutExtension(file)}_{Guid.NewGuid()}.txt", $"UN GROS LOG");


Console.WriteLine($"""
Langages détectés : {string.Join(", ", suspectFinder.ListAllFoundLanguages())}
{susFileCounter} / {totalFilesCounter} fichiers avec des lignes suspectes.
""");




return 0;