namespace Playground.Models;

using Serilog;

public class NoteRepository
{
    private readonly ILogger _logger;

    public NoteRepository(ILogger logger)
    {
        _logger = logger;
    }
    
    public void RaiseNoteEvent()
    {
        var note = new Note("123", "Claimant");
        note.AlternativeKeys.Add("OwnerWebSafeNo", "abcd-efgh");
        note.AlternativeKeys.Add("ParentOwnerType", "Claim");
        note.AlternativeKeys.Add("ParentOwnerRef", "123456");
        note.AlternativeKeys.Add("ParentOwnerWebSafeNo", "abcd-efgh-ijkl");
        
        _logger.Debug("Note", note);
    }
}