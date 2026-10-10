using backend.Models.DTOs;

public static class AuthRoute
{
	public static void MapAuthRoute(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/auth");

		group.MapPost("/reg" , AuthController.Register)
		.AddEndpointFilter<FluentValidationFilter<UserRegisterDTO>>();

		group.MapPost("/login" , AuthController.Login)
		.AddEndpointFilter<FluentValidationFilter<UserLoginDTO>>();
	}
}