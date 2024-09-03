using Ecommerce.Application.Common.Commands;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    : ICommandHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Price,
            request.Sku,
            "USD",
            request.Description,
            request.ComparePrice,
            request.CreatorId);

        if (product.IsFailure)
        {
            return Result.Failure(product.Error);
        }

        _productRepository.Insert(product.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}