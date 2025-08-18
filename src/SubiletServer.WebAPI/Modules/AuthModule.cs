using MediatR;
using SubiletServer.Application.Auth;
using TS.Result;
using Microsoft.AspNetCore.Builder;


namespace SubiletServer.WebAPI.Modules
{
    public static class AuthModule
    {
        public static void MapAuth(this IEndpointRouteBuilder builder)
        {
            var app = builder.MapGroup("/auth");

            app.MapPost("/login",
                async (LoginCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var res = await sender.Send(request, cancellationToken);
                    return res.IsSuccessful ? Results.Ok(res) : Results.BadRequest(res);
                })
                .Produces<Result<string>>();
        }
    }
}
