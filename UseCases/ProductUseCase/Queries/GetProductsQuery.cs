using MediatR;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Entities;

namespace MyWebAPI.UseCases.ProductUseCase.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly DekDueShopContext dekDueShopContext;

    public GetProductsQueryHandler(DekDueShopContext dekDueShopContext) => this.dekDueShopContext = dekDueShopContext;

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
            await dekDueShopContext.Products
                .Include(product => product.ProductCategory)
                .OrderByDescending(order => order.ProductId)
                .ToListAsync();
}
