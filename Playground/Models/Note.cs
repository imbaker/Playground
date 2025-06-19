namespace Playground.Models;

public class Note
{
    public Note(string ownerType, string ownerRef)
    {
        OwnerType = ownerType;
        OwnerRef = ownerRef;
        this.AlternativeKeys = new Dictionary<string, string>();
    }

    public string OwnerType { get; set; }
    public string OwnerRef { get; set; }    
    public Dictionary<string, string> AlternativeKeys { get; set; }
}