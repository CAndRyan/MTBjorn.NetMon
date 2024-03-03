using MTBjorn.NetMon.Web.Services;

namespace MTBjorn.NetMon.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var dbFilePath = builder.Configuration.GetValue<string>("NetworkMonitorDbFilePath");
			var networkServiceDriver = new NetworkServiceDriver(dbFilePath);
			builder.Services.AddSingleton<NetworkServiceDriver>(networkServiceDriver);

			builder.Services.AddControllers();
			builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseMvcWithDefaultRoute();

			app.MapControllers();

			await app.RunAsync();
		}
	}
}