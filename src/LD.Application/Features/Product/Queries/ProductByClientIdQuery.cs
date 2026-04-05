using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using LD.Contracts.DTOs;
using LD.Contracts.Product;
using MediatR;

namespace LD.Application.Features.Product.Queries
{
    public class ProductByClientIdQuery : IRequest<Result<List<DropDownDto>?>>
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
    }

    public class ProductByClientIdQueryHandler : IRequestHandler<ProductByClientIdQuery, Result<List<DropDownDto>?>>
    {
        private readonly IProductRepository _productRepository;
        private readonly AutoMapper.IMapper _mapper;

        public ProductByClientIdQueryHandler(IProductRepository productRepository, AutoMapper.IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<DropDownDto>?>> Handle(ProductByClientIdQuery request, CancellationToken cancellationToken)
        {
            var asn = await _productRepository.GetProductByClientAsync(request.ClientId, request.ProjectId);
            if (asn is null)
                return Result<List<DropDownDto>?>.Failure("No existe el ProductDto", new System.Collections.Generic.List<string> { "No existe el ProductDto" }, 404);

            var dto = _mapper.Map<List<DropDownDto>>(asn);
            return Result<List<DropDownDto>?>.Success(dto, "ProductDto obtenido correctamente");
        }
    }
}
