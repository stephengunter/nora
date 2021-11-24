using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces
{
    public interface IRemoteApi
    {
        Task<string> GetAsync(string urlpart);

        Task<string> PostAsync(string urlpart, string data);

        Task<string> PutAsync(string urlpart, string data);

        Task<string> DeleteAsync(string urlpart);

        Task<HttpResponseMessage> GetFileAsync(string urlpart);

        Task<string> PostFilesAsync(string urlpart, IFormFileCollection filesToUpload);

        Task<string> PostFileStream(string urlpart, string fileName, MemoryStream fileStreamContent);
    }
}
