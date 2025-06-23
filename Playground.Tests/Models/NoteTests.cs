namespace Playground.Tests.Models;

using Playground.Models;
using Shouldly;

public class NoteTests
{
    [Fact]
    public void Note_If_OwnerType_Is_Claim_Then_Correct_AlternativeKeys_Are_Returned()
    {
        // Arrange
        const OwnerTypes ownerType = OwnerTypes.Claim;
        const string ownerRef = "AA123456";

        var note = CreateSubjectUnderTest(ownerType, ownerRef);

        // Act
        note.AdditionalProperties.Add(ValidAdditionalPropertyKeys.WebSafeClaimNo, "abcd-efgh-ijkl-mnop");

        // Assert
        note.AlternativeKeys.Count.ShouldBe(3);
        note.AlternativeKeys["OwnerRef"].ShouldBe("AA123456");
        note.AlternativeKeys["OwnerType"].ShouldBe("Claim");
        note.AlternativeKeys["OwnerWebSafeNo"].ShouldBe("abcd-efgh-ijkl-mnop");
    }

    [Fact]
    public void Note_If_OwnerType_Is_Claimant_Then_Correct_AlternativeKeys_Are_Returned()
    {
        // Arrange
        const OwnerTypes ownerType = OwnerTypes.Claimant;
        const string ownerRef = "ClaimantRef";

        var note = CreateSubjectUnderTest(ownerType, ownerRef);

        // Act
        note.AdditionalProperties.Add(ValidAdditionalPropertyKeys.WebSafeClaimNo, "abcd-efgh-ijkl-mnop");
        note.AdditionalProperties.Add(ValidAdditionalPropertyKeys.ClaimRefNo, "123456");

        // Assert
        note.AlternativeKeys.Count.ShouldBe(5);
        note.AlternativeKeys["OwnerRef"].ShouldBe("ClaimantRef");
        note.AlternativeKeys["OwnerType"].ShouldBe("Claimant");
        note.AlternativeKeys["ParentOwnerWebSafeNo"].ShouldBe("abcd-efgh-ijkl-mnop");
        note.AlternativeKeys["ParentOwnerRef"].ShouldBe("123456");
        note.AlternativeKeys["ParentOwnerType"].ShouldBe("Claim");
    }

    [Fact]
    public void Note_If_Two_Identical_Property_Keys_Are_Added_Then_ArgumentException_Is_Thrown()
    {
        // Arrange
        const ValidAdditionalPropertyKeys actualKey = ValidAdditionalPropertyKeys.WebSafeClaimNo;
        const string firstValue = "firstValue";
        const string secondValue = "secondValue";
        const string expectedMessage =
            $"An item with the same key has already been added. Key: {nameof(ValidAdditionalPropertyKeys.WebSafeClaimNo)}";

        var note = CreateSubjectUnderTest(OwnerTypes.Claim, string.Empty);
        note.AdditionalProperties.Add(actualKey, firstValue);

        // Act & Arrange
        Should.Throw<ArgumentException>(() => { note.AdditionalProperties.Add(actualKey, secondValue); })
            .Message.ShouldBe(expectedMessage);
    }

    private static Note CreateSubjectUnderTest(OwnerTypes ownerType, string ownerRef) => new(ownerType, ownerRef);
}