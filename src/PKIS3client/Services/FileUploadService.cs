using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_client.Services
{
    class FileUploadService : IFileUploadService
    {
        private readonly string _serverAddress;
        private readonly int _serverPort;

        public FileUploadService(string serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
        }

        public async Task SendFileAsync(string filePath)
        {
            try
            {
                using var client = new TcpClient(_serverAddress, _serverPort);
                using var networkStream = client.GetStream();

                await SendFileNameAsync(networkStream, filePath);
                await SendFileContentAsync(networkStream, filePath);

                client.Client.Shutdown(SocketShutdown.Send);

                string result = await ReadServerResponseAsync(networkStream);
                Console.WriteLine("Результаты анализа:\n" + result);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка сети: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}");
            }
        }

        private async Task SendFileNameAsync(NetworkStream stream, string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
            byte[] lengthBytes = BitConverter.GetBytes(nameBytes.Length);

            await stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
            await stream.WriteAsync(nameBytes, 0, nameBytes.Length);
        }

        private async Task SendFileContentAsync(NetworkStream stream, string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            await fileStream.CopyToAsync(stream);
            await stream.FlushAsync();
        }

        private async Task<string> ReadServerResponseAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }
}
