using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public static class AuthController
{
	public static async Task<IResult> Register(DevInitContext db , UserRegisterDTO userDTO)
	{
		if(string.IsNullOrWhiteSpace(userDTO.Email)) return Results.BadRequest(new {error = "your email is empty"});
		bool isEmailExist = await db.Users.AnyAsync(u => u.Email == userDTO.Email);
		if(isEmailExist) return Results.BadRequest(new {error = "email is alredy exist"});

		string password = userDTO.Password;
		string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

		var user = new User()
		{
			Name = userDTO.Name.Trim(),
			Email = userDTO.Email.Trim(),
			Password = passwordHash
		};

		db.Users.Add(user);
		await db.SaveChangesAsync();
		
		return Results.Ok(user);
	}
}