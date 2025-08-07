using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Apartments.Commands.PostApartments;
using AutoMapper;
using MediatR;
using WebApi.Entidades;
using WebApi.Interfaces;

namespace Application.Apartments.Commands.DeleteAparment;

public record DeleteApartmentsQuery : IRequest
{
    public DeleteApartmentDto Apartment { get; init; } = new DeleteApartmentDto();
}

public class DeleteApartmentHandler
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public DeleteApartmentHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task Handle(DeleteApartmentsQuery request, CancellationToken cancellationToken)
    {
        var result = _mapper.Map<Apartment>(request.Apartment);
        _apartmentRepository.Add(result);

        await Task.FromResult(result);
    }
}
