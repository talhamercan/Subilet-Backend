using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Repositories;
using GenericRepository;

namespace SubiletServer.WebAPI.Extensions
{
    public static class AdminUserExtensions
    {
        public static async Task CreateAdminUser(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            
            // Admin kullanıcısının var olup olmadığını kontrol et
            var existingAdmin = await userRepository.FirstOrDefaultAsync(u => u.Username == "admin");
            
            if (existingAdmin == null)
            {
                // Admin kullanıcısını oluştur
                var adminUser = new User(
                    firstName: "Admin",
                    lastName: "User",
                    email: "admin@subilet.com",
                    username: "admin",
                    password: "admin123",
                    role: "Admin"
                );
                
                userRepository.Add(adminUser);
                await unitOfWork.SaveChangesAsync();
                
                Console.WriteLine("Admin kullanıcısı başarıyla oluşturuldu!");
                Console.WriteLine("Kullanıcı adı: admin");
                Console.WriteLine("Şifre: admin123");
                Console.WriteLine("Role: Admin");
            }
            else
            {
                Console.WriteLine("Admin kullanıcısı zaten mevcut!");
                // Eğer mevcut admin kullanıcısının role'ü Admin değilse güncelle
                if (existingAdmin.Role != "Admin")
                {
                    existingAdmin.SetRole("Admin");
                    await unitOfWork.SaveChangesAsync();
                    Console.WriteLine("Mevcut admin kullanıcısının role'ü Admin olarak güncellendi!");
                }
            }
        }
    }
} 