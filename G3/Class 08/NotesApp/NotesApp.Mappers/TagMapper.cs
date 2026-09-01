using NotesApp.Domain.Models;
using NotesApp.DTOs;

namespace NotesApp.Mappers
{
    public static class TagMapper
    {
        public static TagDto ToTagDto(this Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Color = tag.Color,
                Name = tag.Name,
            };
        }
    }
}
