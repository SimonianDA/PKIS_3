using Programming_of_corporate_industrial_systems_Practice_2_client.ErrorHandling;
using Programming_of_corporate_industrial_systems_Practice_2_client.ErrorHandling.ErrorsTypes;
using System;

public class ErrorHandler : IErrorHandler
{
    private const string _systemErrorMessage = "Ошибка системы";

    public void ExecuteWithErrorHandling(Func<Task> action)
    {
        try
        {
            action().Wait();
        }
        catch (CommonException exception)
        {
            Console.WriteLine(exception.Message);
        }
        catch (Exception)
        {
            Console.WriteLine(_systemErrorMessage);
        }
    }
}