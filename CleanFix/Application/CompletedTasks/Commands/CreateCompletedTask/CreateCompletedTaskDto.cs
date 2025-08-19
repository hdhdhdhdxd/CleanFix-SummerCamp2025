using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;
public class CreateCompletedTaskDto
{
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "El CompanyId es obligatorio.")]
    public int CompanyId { get; set; }
    [Required(ErrorMessage = "El ApartmentId es obligatorio.")]
    public int ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateCompletedTaskDto, CompletedTask>();
        }
    }
}
