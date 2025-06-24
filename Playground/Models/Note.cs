namespace Playground.Models;

public enum OwnerTypes
{
    Claim,
    Claimant,
}

public enum ValidAdditionalPropertyKeys
{
    WebSafeClaimNo,
    ClaimRefNo,
}

public class Note
{
    public const string OwnerRefKey = "OwnerRef";
    public const string OwnerTypeKey = "OwnerType";
    public const string OwnerWebSafeNoKey = "OwnerWebSafeNo";
    public const string ParentOwnerRefKey = "ParentOwnerRef";
    public const string ParentOwnerTypeKey = "ParentOwnerType";
    public const string ParentOwnerWebSafeNoKey = "ParentOwnerWebSafeNo";

    public Note(OwnerTypes ownerType, string ownerRef)
    {
        OwnerType = ownerType;
        OwnerRef = ownerRef;
        AdditionalProperties = new Dictionary<ValidAdditionalPropertyKeys, string>();
    }

    public Dictionary<ValidAdditionalPropertyKeys, string> AdditionalProperties { get; }

    public Dictionary<string, string> AlternativeKeys
    {
        get
        {
            var alternativeKeys = new Dictionary<string, string>();
            if (AdditionalProperties.Count == 0)
            {
                return alternativeKeys;
            }

            alternativeKeys.Add(OwnerTypeKey, OwnerType.ToString());
            alternativeKeys.Add(OwnerRefKey, OwnerRef);
            switch (OwnerType)
            {
                case OwnerTypes.Claim:
                    alternativeKeys.Add(OwnerWebSafeNoKey, AdditionalProperties[ValidAdditionalPropertyKeys.WebSafeClaimNo]);
                    return alternativeKeys;
                case OwnerTypes.Claimant:
                    alternativeKeys.Add(ParentOwnerTypeKey, nameof(OwnerTypes.Claim));
                    alternativeKeys.Add(ParentOwnerRefKey, AdditionalProperties[ValidAdditionalPropertyKeys.ClaimRefNo]);
                    alternativeKeys.Add(ParentOwnerWebSafeNoKey, AdditionalProperties[ValidAdditionalPropertyKeys.WebSafeClaimNo]);
                    return alternativeKeys;
            }

            return new Dictionary<string, string>();
        }
    }

    private string OwnerRef { get; }

    private OwnerTypes OwnerType { get; }
}