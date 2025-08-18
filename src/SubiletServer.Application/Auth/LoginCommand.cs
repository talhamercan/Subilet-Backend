using FluentValidation;
using MediatR;
using SubiletServer.Application.Services;
using SubiletServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;


namespace SubiletServer.Application.Auth;

public sealed record LoginCommand(
    string EmailOrUsername,
    string password) : IRequest<Result<string>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUsername)
            .NotEmpty()
            .WithMessage("Geçerli bir mail ya da kullanıcı adı girin");
        RuleFor(p => p.password)
            .NotEmpty()
            .WithMessage("Geçerli bir şifre girin");
    }
}



public sealed class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(
            p => p.Email == request.EmailOrUsername || p.Username == request.EmailOrUsername);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış");
        }
        
        var checkPassword = user.VerifyPasswordHash(request.password);
        if (!checkPassword)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış");
        }
        
        if (!user.IsActive)
        {
            return Result<string>.Failure("Hesabınız aktif değil");
        }
        
        var token = jwtProvider.CreateToken(user);
        return Result<string>.Succeed(token);
    }
}
