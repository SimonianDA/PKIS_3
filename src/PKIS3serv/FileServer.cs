using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Programming_of_corporate_industrial_systems_Practice_2_server.Services;

namespace Programming_of_corporate_industrial_systems_Practice_2_server
{
    public class FileServer
    {
        private readonly int _port;
        private readonly IFileProcessingService _fileProcessingService;

        public FileServer(int port, IFileProcessingService? processingService = null)
        {
            _port = port;
            _fileProcessingService = processingService ?? new FileProcessingService();
        }

        public async Task StartAsync()
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Console.WriteLine($"Сервер запущен на порту {_port}");

            EnsureDirectoryExists("ReceivedFiles");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            Console.WriteLine("Клиент подключен.");
            using var networkStream = client.GetStream();

            try
            {
                string fileName = await ReadFileNameAsync(networkStream);
                string filePath = Path.Combine("ReceivedFiles", fileName);

                Console.WriteLine($"Получен файл: {fileName}");
                await ReceiveFileAsync(networkStream, filePath);

                var analysisResult = _fileProcessingService.ProcessFile(filePath);
                string resultText = FormatResult(analysisResult);

                Console.WriteLine(resultText);
                await SendResponseAsync(networkStream, resultText);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция была прервана из-за таймаута.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Клиент отключен.\n");
            }
        }

        private async Task<string> ReadFileNameAsync(NetworkStream stream)
        {
            var lengthBuffer = new byte[4];
            await stream.ReadExactlyAsync(lengthBuffer);
            int fileNameLength = BitConverter.ToInt32(lengthBuffer, 0);

            var nameBuffer = new byte[fileNameLength];
            await stream.ReadExactlyAsync(nameBuffer);

            return Encoding.UTF8.GetString(nameBuffer);
        }

        private async Task ReceiveFileAsync(NetworkStream stream, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var cts = new CancellationTokenSource(30000);
            await stream.CopyToAsync(fileStream, 81920, cts.Token);
        }

        private async Task SendResponseAsync(NetworkStream stream, string message)
        {
            var responseBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

        private static string FormatResult(Models.FileAnalysisResult result)
        {
            return $"Имя файла: {result.FileName}\nСтрок: {result.LineCount}, Слов: {result.WordCount}, Символов: {result.CharacterCount}";
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}