﻿namespace Playground.Tests.Repositories;

using NSubstitute;
using Playground.Models;
using Playground.Repositories;
using Serilog;

public class NoteRepositoryTests
{
    [Fact]
    public void RaiseNoteEvent_Should_Call_Logger()
    {
        // Arrange
        var mockLogger = Substitute.For<ILogger>();
        var noteRepository = new NoteRepository(mockLogger);

        // Act
        noteRepository.RaiseNoteEvent();

        // Assert
        mockLogger.Received(1).Debug("Note", Arg.Any<Note>());
    }
}