using System.Collections.Concurrent;
using System.Text.Json;
using is_it_sus.DTO;

namespace is_it_sus;

public class SuspectFinderService
{
    //ensemble des ext pour tester les fichiers inconnus
    HashSet<string> _extensions = [];
    //le bucket des extensions deja listées et le langage correspondant pour tester dans le file
    public ConcurrentDictionary<string, Language> FoundExtentions {get; private set;} = [];
    public  List<Language> Languages {get; private set;} = [];
    
//INIT
    public void LoadExtensionsData()
    {
        string jsonFilePath =  AppData.Instance.SuspectKeywordsJsonFile;

        string content = File.ReadAllText(jsonFilePath);

        JsonSerializerOptions jsonOptions = new() {PropertyNameCaseInsensitive = true};
        var json = JsonSerializer.Deserialize<Dictionary<string, LanguageData>>(content,  jsonOptions);

        if(json is null)
            throw new Exception("Le fichier json est vide");

        foreach(var item in json)
        {
            var lang = new Language(){
                Name = item.Key, 
                Keywords = item.Value.Keywords, 
                Extensions = item.Value.Extensions
                };
            Languages.Add(lang);

            foreach (var ext in lang.Extensions)
            {
                _extensions.Add(ext);
            }
        }

    }
    

    public void FindExtension(string ext)
    {
        if(FoundExtentions.ContainsKey(ext))
            return;

        if(HasExtension(ext))
        {
            foreach(var lang  in Languages)
            {
                if(lang.Extensions.Contains(ext))
                {
                    FoundExtentions.TryAdd(ext, lang);
                    break;
                }
            }
        }
    }

//HELPERS & GETTERS
    public bool HasExtension(string ext)
    {
        return _extensions.Contains(ext);
    }
    public IEnumerable<string> GetAllFoundExtensions()
    {
        return FoundExtentions.Keys.AsEnumerable();
    }
    public IEnumerable<string> ListAllExtensions()
    {
        return _extensions.AsEnumerable();
    }

    public IEnumerable<string> ListAllFoundLanguages()
    {
        HashSet<string> langName = [];
        foreach(var l in FoundExtentions.Values)
        {
            langName.Add(l.Name);
        }
        return langName.AsEnumerable();
    }
}