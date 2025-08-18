using SubiletBackend.Application;
using MediatR;

namespace SubiletBackend.Application
{
    public class RegisterUserCommand : IRequest<AuthResponse>
    {
        public RegisterRequest Request { get; set; }
        public RegisterUserCommand(RegisterRequest request)
        {
            Request = request;
        }
    }

    public class LoginUserCommand : IRequest<AuthResponse>
    {
        public LoginRequest Request { get; set; }
        public LoginUserCommand(LoginRequest request)
        {
            Request = request;
        }
    }
} 