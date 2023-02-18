using MediatR;

namespace Notes.Application.Notes.Queries.GetNoteLite
{
    public class GetNoteListQuery : IRequest<NoteListVm>
    {
        public Guid UserId { get; set; }
    }
}
