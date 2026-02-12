namespace Derify.Core.Reponse;

public record Result<TValue> (bool IsSuccess, string Message, TValue? Value, Exception? Exception, string? ErrorMessage) where TValue : class
{
	public static Result<TValue> Success(TValue value, string message = "") 
		=> new Result<TValue>(true, message, value, null, null);

	public static Result<TValue> Failure(string errorMessage, Exception? exception = null, string message = "") 
		=> new Result<TValue>(false, message, null, exception,errorMessage);
}
