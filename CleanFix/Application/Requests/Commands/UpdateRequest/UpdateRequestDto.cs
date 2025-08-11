using AutoMapper;
using Domain.Entities;

namespace Application.Requests.Commands.UpdateRequest;
public class UpdateRequestDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public string? Status { get; set; }
    public double MaintenanceCost { get; set; }
    public IssueType Type { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateRequestDto, Request>();
        }
    }
}
