using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;
public class CreateCompletedTaskDto
{
    public int? SolicitationId { get; set; } 
    public int? IncidenceId { get; set; } 
    public int CompanyId { get; set; }
    public int[] MaterialIds { get; set; } = [];
    public bool IsSolicitation { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateCompletedTaskDto, CompletedTask>();
        }
    }
}
