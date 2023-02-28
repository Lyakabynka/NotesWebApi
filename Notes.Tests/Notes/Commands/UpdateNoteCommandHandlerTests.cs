using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var newTitle = "new title";
            var newDetails = "new details";
            //Act
            await handler.Handle(
                new UpdateNoteCommand()
                {
                    Id = NotesContextFactory.NoteIdForUpdate,
                    Title = newTitle,
                    Details = newDetails,
                    UserId = NotesContextFactory.UserBId,
                },
                CancellationToken.None
            );
            //Assert
            Assert.NotNull(Context.Notes.SingleOrDefaultAsync(note=>
                note.Id == NotesContextFactory.NoteIdForUpdate &&
                note.Title == newTitle && note.Details == newDetails &&
                note.UserId == NotesContextFactory.UserBId));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            //Act
            //+
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateNoteCommand()
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserBId,
                    },
                    CancellationToken.None
                    )
                );
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
        {
            //Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            //Act
            //+
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateNoteCommand()
                    {
                        Id = NotesContextFactory.NoteIdForUpdate,
                        UserId = NotesContextFactory.UserAId,
                    },
                    CancellationToken.None
                    )
                );
        }
    }
}
