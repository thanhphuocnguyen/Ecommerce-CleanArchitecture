﻿namespace Ecommerce.Infrastructure.Authentication;

public class JwtOptions
{
    public string Secret { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;
}