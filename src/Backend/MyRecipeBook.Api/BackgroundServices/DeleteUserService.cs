using Azure.Messaging.ServiceBus;
using MyRecipeBook.Application.UseCases.UserManagement.Delete.DeleteAccount;
using MyRecipeBook.Infrastructure.Services.ServiceBus;

namespace MyRecipeBook.Api.BackgroundServices;

public class DeleteUserService : BackgroundService
{
  private readonly IServiceProvider _services;
  private readonly ServiceBusProcessor _processor;

  public DeleteUserService(IServiceProvider services, DeleteUserProcessor processor)
  {
    _processor = processor.GetProcessor();
    _services = services;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _processor.ProcessMessageAsync += ProccessMessageAsync;
    _processor.ProcessErrorAsync += ExceptionReceivedHandler;

    await _processor.StartProcessingAsync(stoppingToken);
  }

  private async Task ProccessMessageAsync(ProcessMessageEventArgs args)
  {
    var message = args.Message.Body.ToString();
    var userId = Guid.Parse(message);
    var scope = _services.CreateScope();
    var deleteUser = scope.ServiceProvider.GetRequiredService<IDeleteAccountUser>();

    await deleteUser.Execute(userId);
  }

  private Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

  ~DeleteUserService() => Dispose();

  public override void Dispose()
  {
    base.Dispose();

    GC.SuppressFinalize(this);
  }
}
