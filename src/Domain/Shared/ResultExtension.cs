namespace Ecommerce.Domain.Shared;

public static class ResultExtension
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }

        return onFailure(result.Error!);
    }
}