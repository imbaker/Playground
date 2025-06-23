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
    private const string OwnerRefKey = "OwnerRef";
    private const string OwnerTypeKey = "OwnerType";
    private const string OwnerWebSafeNoKey = "OwnerWebSafeNo";
    private const string ParentOwnerRef = "ParentOwnerRef";
    private const string ParentOwnerTypeKey = "ParentOwnerType";
    private const string ParentOwnerWebSafeNo = "ParentOwnerWebSafeNo";

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
            alternativeKeys.Add(OwnerTypeKey, OwnerType.ToString());
            alternativeKeys.Add(OwnerRefKey, OwnerRef);
            switch (OwnerType)
            {
                case OwnerTypes.Claim:
                    alternativeKeys.Add(OwnerWebSafeNoKey, AdditionalProperties[ValidAdditionalPropertyKeys.WebSafeClaimNo]);
                    return alternativeKeys;
                case OwnerTypes.Claimant:
                    alternativeKeys.Add(ParentOwnerTypeKey, nameof(OwnerTypes.Claim));
                    alternativeKeys.Add(ParentOwnerRef, AdditionalProperties[ValidAdditionalPropertyKeys.ClaimRefNo]);
                    alternativeKeys.Add(ParentOwnerWebSafeNo, AdditionalProperties[ValidAdditionalPropertyKeys.WebSafeClaimNo]);
                    return alternativeKeys;
            }

            return new Dictionary<string, string>();
        }
    }

    private string OwnerRef { get; }

    private OwnerTypes OwnerType { get; }
}