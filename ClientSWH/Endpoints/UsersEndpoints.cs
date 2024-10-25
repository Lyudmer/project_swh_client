﻿using ClientSWH.Application.Services;
using ClientSWH.Contracts;
using ClientSWH.Core.Abstraction.Services;


namespace ClientSWH.Endpoints
{
    public static class UsersEndpoints
    {
        public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
           
            return app;
        }
        private static async Task<IResult> Register(RegisterUserRequest request, UsersService usersService)
        {
            await ((IUsersService)usersService).Register(request.UserName, request.PasswordHash, request.Email);
            return Results.Ok();
        }
        private static async Task<IResult> Login(LoginUserRequest request, UsersService usersService,HttpContext context)
        {
            var token = await ((IUsersService)usersService).Login(request.passwordHash, request.email);

            context.Response.Cookies.Append("tu-cookes", token);

            return Results.Ok(token);
        }
       
    }
}
