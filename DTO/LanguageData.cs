namespace is_it_sus.DTO;

public class LanguageData
{
    public HashSet<string> Keywords  {get; set;} = [];
    public HashSet<string> Extensions {get; set;} = [];
}

public class JsonLanguageData
{
    public Dictionary<string, LanguageData> Data = [];
}