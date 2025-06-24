namespace Playground.Tests.Models;

using Playground.Models;
using Shouldly;

public class NoteTests
{
    [Theory]
    [InlineData(OwnerTypes.Claim)]
    [InlineData(OwnerTypes.Claimant)]
    public void Note_If_No_AdditionalProperty_Values_Are_Added_Then_AlternativeKeys_Is_Empty(OwnerTypes actualOwnerType)
    {
        // Arrange
        // Act
        var note = CreateSubjectUnderTest(actualOwnerType, string.Empty);

        // Assert
        note.AlternativeKeys.Count.ShouldBe(0);
    }

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
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.OwnerRefKey, "AA123456");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.OwnerTypeKey, "Claim");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.OwnerWebSafeNoKey, "abcd-efgh-ijkl-mnop");
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
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.OwnerRefKey, "ClaimantRef");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.OwnerTypeKey, "Claimant");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.ParentOwnerWebSafeNoKey, "abcd-efgh-ijkl-mnop");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.ParentOwnerRefKey, "123456");
        note.AlternativeKeys.ShouldContainKeyAndValue(Note.ParentOwnerTypeKey, "Claim");
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

        // Act & Assert
        Should.Throw<ArgumentException>(() => { note.AdditionalProperties.Add(actualKey, secondValue); })
            .Message.ShouldBe(expectedMessage);
    }

    private static Note CreateSubjectUnderTest(OwnerTypes ownerType, string ownerRef) => new(ownerType, ownerRef);
}