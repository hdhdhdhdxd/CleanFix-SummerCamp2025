using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Apartments.Commands.PostApartments;
using Application.Apartments.Queries.GetApartments;
using AutoMapper;
using MediatR;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace Application.Apartments.Commands.PutApartments;

public record PutApartmentQuery : IRequest
{
    public PutApartmentDto Apartment { get; init; } = new PutApartmentDto();
}

public class PutApartmentHandler
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public PutApartmentHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task Handle(PutApartmentQuery request, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<Apartment>(request.Apartment);
        _apartmentRepository.Add(result);

        await Task.FromResult(result);
    }
}
