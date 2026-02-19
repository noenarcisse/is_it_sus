using System.Text.Json;
using is_it_sus.DTO;

namespace is_it_sus;

public class SuspectFinderService
{
    //ensemble des ext pour tester les fichiers inconnus
    HashSet<string> _extensions = [];
    //le bucket des extensions deja listées et le langage correspondant pour tester dans le file
    public Dictionary<string, Language> FoundExtentions {get; private set;} = [];
    public  List<Language> Languages {get; private set;} = [];
    
    
    public void FindExtension(string ext)
    {
        if(FoundExtentions.ContainsKey(ext))
            return;

        if(_extensions.Contains(ext))
        {
            foreach(var lang  in Languages)
            {
                if(lang.Extensions.Contains(ext))
                {
                    FoundExtentions.Add(ext, lang);
                    break;
                }
            }
        }
    }
    public bool HasExtension(string ext)
    {
        return _extensions.Contains(ext);
    }

    public void ListAllExtensions(string path)
    {
        //
    }

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
}