namespace Ecommerce.Application.Identity.Users.Contracts
{
    public record UserDetailsDto
    {
        public required string Id { get; init; }

        public required string Name { get; init; }

        public required string Email { get; init; }

        public required string PhoneNumber { get; init; }

        public required string Status { get; init; }

        public required string CreatedAt { get; init; }

        public required string UpdatedAt { get; init; }
    }
}