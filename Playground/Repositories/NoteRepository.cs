namespace Playground.Repositories;

using Playground.Models;
using Serilog;

public class NoteRepository
{
    private readonly ILogger _logger;

    public NoteRepository(ILogger logger) => _logger = logger;

    public void RaiseNoteEvent()
    {
        var note = new Note(OwnerTypes.Claim, "Claimant");
        note.AdditionalProperties.Add(ValidAdditionalPropertyKeys.WebSafeClaimNo, "abcd-efgh");

        _logger.Debug("Note", note);
    }
}