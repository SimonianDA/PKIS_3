using Programming_of_corporate_industrial_systems_Practice_2_client.ErrorHandling;
using Programming_of_corporate_industrial_systems_Practice_2_client.Services;

namespace Programming_of_corporate_industrial_systems_Practice_2_client
{
    internal class Program
    {
        private static readonly IFileUploadService _fileUploadService = new FileUploadService("127.0.0.1", 5000);
        private static readonly IErrorHandler _errorHandler = new ErrorHandler();

        static async Task Main()
        {
            _errorHandler.ExecuteWithErrorHandling(async () =>
            {
                string filePath = GetFilePathFromUser();
                if (!ValidateFilePath(filePath)) return;

                await _fileUploadService.SendFileAsync(filePath);
            });

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        private static string GetFilePathFromUser()
        {
            Console.Write("Введите путь к файлу: ");
            return Console.ReadLine() ?? string.Empty;
        }

        private static bool ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Путь к файлу не может быть пустым.");
                return false;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден.");
                return false;
            }

            return true;
        }
    }
}
