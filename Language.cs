namespace is_it_sus;

public class Language
{
    public string Name {get; set;} = "";
    public HashSet<string> Keywords {get; set;} = [];
    public HashSet<string> Extensions {get; set;} = [];


    public override string ToString()
    {
        return $"""
        {Name}
        {string.Join(",", Extensions)}
        {string.Join(",", Keywords)}
        """;
    }
}