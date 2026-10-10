using FluentValidation;

public class FluentValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        
        if (validator is null)
        {
            return await next(context);
        }

        var entity = context.Arguments.OfType<T>().FirstOrDefault();
        if (entity is null)
        {
            return Results.BadRequest("Invalid data format");
        }

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}