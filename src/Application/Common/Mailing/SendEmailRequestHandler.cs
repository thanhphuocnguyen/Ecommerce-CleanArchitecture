using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Domain.Common.Mailing;

public class SendEmailRequestHandler : IRequestHandler<SendMailRequest, Result>
{
    public Task<Result> Handle(SendMailRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}