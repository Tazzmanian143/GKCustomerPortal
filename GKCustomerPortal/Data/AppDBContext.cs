using GKCustomerPortal.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration; // Add this using statement
using System.Security.Cryptography;
using System.Text;

namespace GKCustomerPortal.Data;

public class AppDbContext : IdentityDbContext
{
    private readonly string _encryptionKey;
    private readonly string _encryptionIV;

    // Inject IConfiguration to read from appsettings.json
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _encryptionKey = configuration["EncryptionSettings:Key"] ?? throw new InvalidOperationException("Encryption Key missing");
        _encryptionIV = configuration["EncryptionSettings:IV"] ?? throw new InvalidOperationException("Encryption IV missing");
    }

    public DbSet<CustomerModel> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var encryptionConverter = new ValueConverter<string?, string?>(
            v => Encrypt(v),
            v => Decrypt(v)
        );

        // Apply to PII fields
        modelBuilder.Entity<CustomerModel>().Property(c => c.FirstName).HasConversion(encryptionConverter);
        modelBuilder.Entity<CustomerModel>().Property(c => c.LastName).HasConversion(encryptionConverter);
        modelBuilder.Entity<CustomerModel>().Property(c => c.Email).HasConversion(encryptionConverter);
    }

    // Remove the 'static' keyword from this method
    private string? Encrypt(string? plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
        aes.IV = Encoding.UTF8.GetBytes(_encryptionIV);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        return Convert.ToBase64String(encryptedBytes);
    }

    // Remove the 'static' keyword from this method
    private string? Decrypt(string? cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(_encryptionIV);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] inputBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch
        {
            return cipherText;
        }
    }
}