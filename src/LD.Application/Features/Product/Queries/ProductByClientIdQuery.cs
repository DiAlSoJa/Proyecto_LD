using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using LD.Contracts.DTOs;
using LD.Contracts.Product;
using MediatR;

namespace LD.Application.Features.Product.Queries
{
    public class ProductByClientIdQuery : IRequest<Result<List<ProductAutocompleteDto>?>>
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
    }

    public class ProductByClientIdQueryHandler : IRequestHandler<ProductByClientIdQuery, Result<List<ProductAutocompleteDto>?>>
    {
        private readonly IProductRepository _productRepository;
        private readonly AutoMapper.IMapper _mapper;

        public ProductByClientIdQueryHandler(IProductRepository productRepository, AutoMapper.IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductAutocompleteDto>?>> Handle(ProductByClientIdQuery request, CancellationToken cancellationToken)
        {
            var asn = await _productRepository.GetProductByClientAsync(request.ClientId, request.ProjectId);
            if (asn is null)
                return Result<List<ProductAutocompleteDto>?>.Failure("No existe el Producto", new System.Collections.Generic.List<string> { "No existe el Producto" }, 404);

            var dto = _mapper.Map<List<ProductAutocompleteDto>>(asn);
            return Result<List<ProductAutocompleteDto>?>.Success(dto, "ProductDto obtenido correctamente");
        }
    }
}
