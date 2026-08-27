using NotesApp.Domain.Models;
using NotesApp.Dtos;

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

        //public static List<TagDto> ToTagDtoList(this List<Tag> tags)
        //{
        //    return tags.Select(tag => tag.ToTagDto()).ToList();
        //}

        // Simpler syntax using lambda expression (arrow function)
        public static List<TagDto> ToTagDtoList(this List<Tag> tags) => tags.Select(tag => tag.ToTagDto()).ToList();
    }
}
