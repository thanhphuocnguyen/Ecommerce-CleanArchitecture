using Ecommerce.Domain.Shared.Primitives;
using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Domain.Common.Mailing;

public record SendMailRequest(
    List<string> To,
    string Subject,
    string? Body = null,
    string? From = null,
    string? DisplayName = null,
    string? ReplyTo = null,
    string? ReplyToName = null,
    List<string>? Bcc = null,
    List<string>? Cc = null,
    IDictionary<string, byte[]>? AttachmentData = null,
    IDictionary<string, string>? Headers = null) : IRequest<Result>;