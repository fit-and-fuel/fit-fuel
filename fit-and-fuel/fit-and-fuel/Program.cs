using fit_and_fuel.Data;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using fit_and_fuel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddControllers().AddNewtonsoftJson(
              option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
              );

// Add services to the container.

builder.Services.AddScoped<JwtTokenService>();
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

builder.Services.AddTransient<IRating, RatingService>();

builder.Services.AddTransient<INotification, NotificationService>();



builder.Services.AddAuthorization(options =>
{
    // Add "Name of Policy", and the Lambda returns a definition
    options.AddPolicy("create", policy => policy.RequireClaim("permissions", "create"));
    options.AddPolicy("update", policy => policy.RequireClaim("permissions", "update"));
    options.AddPolicy("delete", policy => policy.RequireClaim("permissions", "delete"));
});




builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
   .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddSignalR();

builder.Services
.AddDbContext<AppDbContext>
(opions => opions

.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
