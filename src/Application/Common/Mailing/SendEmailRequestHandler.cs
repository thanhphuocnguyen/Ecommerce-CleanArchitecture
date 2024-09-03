using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Application.Common.Mailing;

public class SendEmailRequestHandler : IRequestHandler<SendMailRequest, Result>
{
    public Task<Result> Handle(SendMailRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}