using fit_and_fuel.Data;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using fit_and_fuel.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
.AddDbContext<AppDbContext>
(opions => opions
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.


builder.Services.AddControllers().AddNewtonsoftJson(
              option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
              );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
   .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.

//builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddTransient<IUserService, IdentityUserService>();
builder.Services.AddTransient<ILike, LikeService>();
builder.Services.AddTransient<IComment, CommentService>();
builder.Services.AddTransient<INutritionists, NutritionistService>();
builder.Services.AddTransient<IPatients, PatientService>();
builder.Services.AddTransient<IDietPlan, DietPlanService>();
builder.Services.AddTransient<IHealthRecord, HealthRecordService>();
builder.Services.AddTransient<IChatMessage, ChatMessageService>();
builder.Services.AddTransient<IDays, DayService>();
builder.Services.AddTransient<IMeals, MealService>();
builder.Services.AddTransient<IPost, PostService>();
builder.Services.AddTransient<IAvailableTime, AvailableTimeService>();
builder.Services.AddTransient<IPayment, PaymentService>();
builder.Services.AddTransient<IClinic, ClinicService>();
builder.Services.AddTransient<IAppoitments, AppoitmentService>();
//builder.Services.AddScoped<IChatMessage,ChatMessageService>();
builder.Services.AddTransient<IRating, RatingService>();
builder.Services.AddScoped<IEmailSender, SendEmailService>();
builder.Services.AddTransient<INotification, NotificationService>();
builder.Services.AddTransient<IPrice, PriceService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:StorageAccount:blob"]);
    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:StorageAccount:queue"]);
});

builder.Services.AddAuthorization(options =>
{
    // Add "Name of Policy", and the Lambda returns a definition
    options.AddPolicy("create", policy => policy.RequireClaim("permissions", "create"));
    options.AddPolicy("update", policy => policy.RequireClaim("permissions", "update"));
    options.AddPolicy("delete", policy => policy.RequireClaim("permissions", "delete"));
});


builder.Services.AddSignalR();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/User/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


//app.MapHub<ChatHub>("/chatHub");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");
    // Other route configurations...
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
