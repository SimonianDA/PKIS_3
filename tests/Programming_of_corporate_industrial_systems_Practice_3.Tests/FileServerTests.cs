using Moq;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Programming_of_corporate_industrial_systems_Practice_3_server;
using Programming_of_corporate_industrial_systems_Practice_3_server.Models;
using Programming_of_corporate_industrial_systems_Practice_3_server.Services;

namespace Programming_of_corporate_industrial_systems_Practice_2.Tests
{
    public class FileServerTests
    {

        [Fact]
        public async Task HandleClientStreamAsync_ProcessesFileAndSendsResponse()
        {
            // Arrange
            var fileName = "test.txt";
            var fileContent = "Hello world\nSecond line";
            var filePath = Path.Combine("ReceivedFiles", fileName);
            Directory.CreateDirectory("ReceivedFiles");
            File.WriteAllText(filePath, fileContent);

            var mockService = new Mock<IFileProcessingService>();
            mockService.Setup(x => x.ProcessFile(filePath)).Returns(new FileAnalysisResult
            {
                FileName = fileName,
                LineCount = 2,
                WordCount = 4,
                CharacterCount = fileContent.Length
            });

            var server = new FileServer(5000, mockService.Object);

            var fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            var fileNameLength = BitConverter.GetBytes(fileNameBytes.Length);
            var fileContentBytes = Encoding.UTF8.GetBytes(fileContent);

            using var input = new MemoryStream();
            input.Write(fileNameLength);
            input.Write(fileNameBytes);
            input.Write(fileContentBytes);
            input.Position = 0;

            using var testStream = new NetworkStream(new MemoryStream(), ownsSocket: false);
            var buffer = new MemoryStream();
            await input.CopyToAsync(buffer);
            buffer.Position = 0;

            using var memoryStream = new NetworkStream(buffer, ownsSocket: false);

            // Act
            await server.TestHandleStreamAsync(memoryStream);

            // Assert
            mockService.Verify(x => x.ProcessFile(It.IsAny<string>()), Times.Once);
        }
    }
}
