using GKCustomerPortal.Components;
using GKCustomerPortal.Data;
using GKCustomerPortal.Handlers;
using GKCustomerPortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- CORE SERVICES ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GK Customer Portal API", Version = "v1" });

    // Define the Basic Auth scheme for Swagger UI
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

// --- DATABASE ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- CUSTOM SERVICES & AUTH STATE ---
builder.Services.AddScoped<ICustomerService, CustomerService>();
// Use the SimpleAuthStateProvider to bridge the UI login logic
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();

// --- IDENTITY CORE ---
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// --- CONSOLIDATED AUTHENTICATION (The Fix for the crash) ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
    .AddIdentityCookies();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// --- MIDDLEWARE ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// --- SEED DATA ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    if (!context.Users.Any())
    {
        var admin = new IdentityUser
        {
            UserName = "admin@gk.com",
            Email = "admin@gk.com",
            NormalizedEmail = "ADMIN@GK.COM",
            NormalizedUserName = "ADMIN@GK.COM"
        };
        var hasher = new PasswordHasher<IdentityUser>();
        admin.PasswordHash = hasher.HashPassword(admin, "Password123!");

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

app.Run();