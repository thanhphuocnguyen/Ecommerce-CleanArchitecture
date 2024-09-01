using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Domain.Errors;

public static class DomainErrors
{
    public static class General
    {
        public static Error ArgumentIsNull(string argumentName) => Error.InvalidValue("argument_is_null", $"Argument '{argumentName}' is null.");
    }

    public static class Identity
    {
        public static class Token
        {
            public static readonly Error InvalidEmailOrPassword = Error.InvalidValue("invalid_email_or_password", "Invalid email or password.");
            public static readonly Error UserIsNotActive = Error.InvalidValue("user_is_not_active", "User is not active.");
            public static readonly Error EmailNotConfirmed = Error.InvalidValue("email_not_confirmed", "Email is not confirmed.");
            public static readonly Error InvalidRefreshToken = Error.InvalidValue("invalid_refresh_token", "Invalid refresh token.");
            public static readonly Error UserNotFound = Error.NotFound("user_not_found", "User not found.");
            public static readonly Error InvalidToken = Error.InvalidValue("invalid_token", "Invalid token.");
        }

        public static class User
        {
            public static readonly Error UserNotFound = Error.NotFound("user_not_found", "User not found.");
            public static readonly Error UserAlreadyExists = Error.Conflict("user_already_exists", "User already exists.");
            public static readonly Error UserIsNotActive = Error.InvalidValue("user_is_not_active", "User is not active.");
            public static readonly Error EmailNotConfirmed = Error.InvalidValue("email_not_confirmed", "Email is not confirmed.");
            public static readonly Error PhoneNumberNotProvided = Error.InvalidValue("phone_number_not_provided", "Phone number is not provided.");
            public static readonly Error InvalidPhoneNumberConfirmationCode = Error.InvalidValue("invalid_phone_number_confirmation_code", "Invalid phone number confirmation code.");
            public static readonly Error UserIdIsNotValid = Error.InvalidValue("user_id_is_not_valid", "User ID is not valid.");
            public static readonly Error CannotChangeStatusOfAdmin = Error.InvalidValue("cannot_change_status_of_admin", "Cannot change status of admin.");
        }

        public static class Role
        {
            public static readonly Error RoleNotFound = Error.NotFound("role_not_found", "Role not found.");
            public static readonly Error RolesNotFound = Error.NotFound("roles_not_found", "Roles not found.");
            public static readonly Error RoleAlreadyExists = Error.Conflict("role_already_exists", "Role already exists.");
            public static readonly Error RoleIsAssignedToUser = Error.Conflict("role_is_assigned_to_user", "Role is assigned to user.");
            public static readonly Error RoleIsNotAssignedToUser = Error.Conflict("role_is_not_assigned_to_user", "Role is not assigned to user.");
            public static readonly Error DefaultRoleCannotBeUpdated = Error.InvalidValue("default_role_cannot_be_updated", "Default role cannot be updated.");
            public static readonly Error DefaultRoleCannotBeDeleted = Error.InvalidValue("default_role_cannot_be_deleted", "Default role cannot be deleted.");
            public static readonly Error RoleIsAssignedToDefaultUser = Error.InvalidValue("role_is_assigned_to_default_user", "Role is assigned to default user.");
            public static readonly Error RoleHasUsers = Error.InvalidValue("role_has_users", "Role has users.");
        }
    }

    public static class Product
    {
        public static readonly Error ProductNotFound = Error.NotFound("product_not_found", "Product not found.");
        public static readonly Error ProductAlreadyExists = Error.Conflict("product_already_exists", "Product already exists.");
    }

    public static class Order
    {
        public static readonly Error OrderNotFound = Error.NotFound("order_not_found", "Order not found.");
        public static readonly Error OrderAlreadyExists = Error.Conflict("order_already_exists", "Order already exists.");
    }

    public static class Category
    {
        public static readonly Error CategoryNotFound = Error.NotFound("Category.category_not_found", "Category not found.");
        public static readonly Error CategoryAlreadyExists = Error.Conflict("Category.category_already_exists", "Category already exists.");
    }

    public static class Customer
    {
        public static readonly Error CustomerNotFound = Error.NotFound("customer_not_found", "Customer not found.");
        public static readonly Error CustomerAlreadyExists = Error.Conflict("customer_already_exists", "Customer already exists.");
    }

    public static class Supplier
    {
        public static readonly Error SupplierNotFound = Error.NotFound("supplier_not_found", "Supplier not found.");
        public static readonly Error SupplierAlreadyExists = Error.Conflict("supplier_already_exists", "Supplier already exists.");
    }

    public static class ProductCategory
    {
        public static readonly Error ProductCategoryNotFound = Error.NotFound("product_category_not_found", "Product category not found.");
        public static readonly Error ProductCategoryAlreadyExists = Error.Conflict("product_category_already_exists", "Product category already exists.");
    }

    public static class OrderItem
    {
        public static readonly Error OrderItemNotFound = Error.NotFound("order_item_not_found", "Order item not found.");
        public static readonly Error OrderItemAlreadyExists = Error.Conflict("order_item_already_exists", "Order item already exists.");
    }

    public static class ProductSupplier
    {
        public static readonly Error ProductSupplierNotFound = Error.NotFound("product_supplier_not_found", "Product supplier not found.");
        public static readonly Error ProductSupplierAlreadyExists = Error.Conflict("product_supplier_already_exists", "Product supplier already exists.");
    }

    // Value Objects Errors
    public static class Email
    {
        public static readonly Error Empty = Error.InvalidValue("email_empty", "Email should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("email_max_length", "Email should not be longer than 256 characters.");
        public static readonly Error Invalid = Error.InvalidValue("email_invalid", "Email is invalid.");
    }

    public static class PhoneNumber
    {
        public static readonly Error Empty = Error.InvalidValue("phone_number_empty", "Phone number should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("phone_number_max_length", "Phone number should not be longer than 15 characters.");
    }

    public static class Name
    {
        public static readonly Error Empty = Error.InvalidValue("name_empty", "Name should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("name_max_length", "Name should not be longer than 256 characters.");
    }

    public static class Description
    {
        public static readonly Error Empty = Error.InvalidValue("description_empty", "Description should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("description_max_length", "Description should not be longer than 256 characters.");
    }

    public static class Price
    {
        public static readonly Error Empty = Error.InvalidValue("price_empty", "Price should not be empty.");
        public static readonly Error Invalid = Error.InvalidValue("price_invalid", "Price is invalid.");
    }

    public static class Sku
    {
        public static readonly Error Empty = Error.InvalidValue("sku_empty", "SKU should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("sku_max_length", "SKU should not be longer than 16 characters.");
        public static readonly Error SeparatorCount = Error.InvalidValue("sku_separator_count", "SKU must have more 3 separators.");
    }

    public static class Quantity
    {
        public static readonly Error Empty = Error.InvalidValue("quantity_empty", "Quantity should not be empty.");
        public static readonly Error Invalid = Error.InvalidValue("quantity_invalid", "Quantity is invalid.");
    }

    public static class ImageUrl
    {
        public static readonly Error Empty = Error.InvalidValue("image_url_empty", "Image URL should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("image_url_max_length", "Image URL should not be longer than 256 characters.");
    }

    public static class Password
    {
        public static readonly Error Empty = Error.InvalidValue("password_empty", "Password should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("password_max_length", "Password should not be longer than 256 characters.");
    }

    public static class Money
    {
        public static readonly Error Empty = Error.InvalidValue("money_empty", "Money should not be empty.");
        public static readonly Error Invalid = Error.InvalidValue("money_invalid", "Money is invalid.");
    }

    public static class Address
    {
        public static readonly Error Empty = Error.InvalidValue("address_empty", "Address should not be empty.");
        public static readonly Error MaxLength = Error.InvalidValue("address_max_length", "Address should not be longer than 256 characters.");

        public static class City
        {
            public static readonly Error Empty = Error.InvalidValue("city_empty", "City should not be empty.");
            public static readonly Error MaxLength = Error.InvalidValue("city_max_length", "City should not be longer than 256 characters.");
        }

        public static class Street
        {
            public static readonly Error Empty = Error.InvalidValue("street_empty", "Street should not be empty.");
            public static readonly Error MaxLength = Error.InvalidValue("street_max_length", "Street should not be longer than 256 characters.");
        }

        public static class ZipCode
        {
            public static readonly Error Empty = Error.InvalidValue("zip_code_empty", "Zip code should not be empty.");
            public static readonly Error MaxLength = Error.InvalidValue("zip_code_max_length", "Zip code should not be longer than 256 characters.");
        }

        public static class Country
        {
            public static readonly Error Empty = Error.InvalidValue("country_empty", "Country should not be empty.");
            public static readonly Error MaxLength = Error.InvalidValue("country_max_length", "Country should not be longer than 256 characters.");
        }

        public static class State
        {
            public static readonly Error Empty = Error.InvalidValue("state_empty", "State should not be empty.");
            public static readonly Error MaxLength = Error.InvalidValue("state_max_length", "State should not be longer than 256 characters.");
        }
    }

    public static class FileStorage
    {
        public static readonly Error FileUploadRequestIsInvalid = Error.InvalidValue("file_upload_request_is_invalid", "File upload request is invalid.");
        public static readonly Error FileExtensionIsInvalid = Error.InvalidValue("file_extension_is_invalid", "File extension is invalid.");
        public static readonly Error FileNameIsInvalid = Error.InvalidValue("file_name_is_invalid", "File name is invalid.");
    }
}