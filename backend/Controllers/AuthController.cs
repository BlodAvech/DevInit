using backend.Extensions;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

public static class AuthController
{
	public static async Task<IResult> Register(DevInitContext db , JWTService jwt , HttpContext http , UserRegisterDTO userDTO)
	{
		userDTO.Name = userDTO.Name.Trim();
		userDTO.Email = userDTO.Email.Trim();
		userDTO.Password = userDTO.Password.Trim();

		if(string.IsNullOrWhiteSpace(userDTO.Email)) return Results.BadRequest(new {error = "your email is empty"});
		bool isEmailExist = await db.Users.AnyAsync(u => u.Email == userDTO.Email);
		if(isEmailExist) return Results.BadRequest(new {error = "email is alredy exist"});

		string password = userDTO.Password;
		string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

		var user = new User()
		{
			Name = userDTO.Name,
			Email = userDTO.Email,
			Password = passwordHash
		};

		db.Users.Add(user);
		await db.SaveChangesAsync();

		var token = jwt.GenerateToken(user.Id , user.Name);

			

		return Results.Ok(token);
	}

	public static async Task<IResult> Login(DevInitContext db , JWTService jwt , UserLoginDTO userDTO)
	{
		userDTO.Email = userDTO.Email.Trim();
		userDTO.Password = userDTO.Password.Trim();
		
		var user = await db.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
		if(user == null) return Results.BadRequest(new {error = "no account with this email"});

		if(string.IsNullOrEmpty(user.Password)) return Results.BadRequest(new {error = "login with oAuth"});

		bool isPasswordsFits = BCrypt.Net.BCrypt.EnhancedVerify(userDTO.Password , user.Password);
		if(!isPasswordsFits) return Results.BadRequest(new {error = "password is incorrect"});

		var token = jwt.GenerateToken(user.Id , user.Name ?? "");
		return Results.Ok(token);
	}
}