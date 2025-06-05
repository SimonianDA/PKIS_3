using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_client.Services
{
    /// <summary>
    /// Интерфейс сервиса для отправки файлов на сервер.
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Отправляет указанный файл на сервер по сети.
        /// </summary>
        /// <param name="filePath">Полный путь к отправляемому файлу.</param>
        /// <returns>Асинхронная задача отправки файла.</returns>
        Task SendFileAsync(string filePath);
    }
}
