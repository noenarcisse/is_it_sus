namespace is_it_sus;

public class AppData
{
    public static AppData Instance {get;} = new();
    public string SuspectKeywordsJsonFile { get; set; }

    
    private AppData()
    {
        SuspectKeywordsJsonFile = "./sus_keywords.json";
    }
}