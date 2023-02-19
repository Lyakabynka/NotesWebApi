﻿using AutoMapper;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.WebApi.Models
{
    public class UpdateNoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNoteDto,UpdateNoteCommand>()
                .ForMember(n=>n.Id,
                    opt=>opt.MapFrom(noteDto=>noteDto.Id))
                .ForMember(n=>n.Title,
                    opt=>opt.MapFrom(noteDto=>noteDto.Title))
                .ForMember(n=>n.Details,
                    opt=>opt.MapFrom(noteDto=>noteDto.Details));
        }
    }
}
