using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Apartments.Queries.GetApartments;
using AutoMapper;
using MediatR;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace Application.Apartments.Commands.PostApartments;

public record PostApartmentsQuery : IRequest
{
    public PostApartmentDto Apartment { get; init; } = new PostApartmentDto();
}
public class PostApartmentHandler
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public PostApartmentHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task Handle(PostApartmentsQuery request, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<Apartment>(request.Apartment);
        _apartmentRepository.Add(result);    

        await Task.FromResult(result);
    }
}
