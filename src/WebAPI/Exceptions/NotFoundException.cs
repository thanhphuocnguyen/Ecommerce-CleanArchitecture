﻿using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.WebAPI;

public class NotFoundException : Exception
{
    public NotFoundException(string? message, Error[]? errors)
        : base(message)
    {
        Errors = errors ?? [];
    }

    public Error[] Errors { get; }
}