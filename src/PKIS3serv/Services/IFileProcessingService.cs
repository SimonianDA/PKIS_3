using Programming_of_corporate_industrial_systems_Practice_2_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_server.Services
{
    /// <summary>
    /// Сервис для анализа содержимого файла.
    /// </summary>
    public interface IFileProcessingService
    {
        /// <summary>
        /// Анализирует файл: считает строки, слова и символы.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Результаты анализа</returns>
        FileAnalysisResult ProcessFile(string filePath);
    }
}
