using Programming_of_corporate_industrial_systems_Practice_2_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_server.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        public FileAnalysisResult ProcessFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            int lines = content.Split('\n').Length;
            int words = content.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int characters = content.Length;

            return new FileAnalysisResult
            {
                FileName = Path.GetFileName(filePath),
                LineCount = lines,
                WordCount = words,
                CharacterCount = characters
            };
        }
    }
}
