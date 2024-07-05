using System.Runtime.Serialization;

namespace UnityExtension
{
    public interface IFileSystem
    {
        bool WriteFile(string content, string filePath);
        bool WriteFile(IFormatter formatter, string content, string filePath);
        string ReadFile(string filePath);
        string ReadFile(IFormatter formatter, string filePath);
        void WriteFileAsync(string filePath);
        void ReadFileAsync(IFormatter formatter, string filePath);
        bool CheckFileExist(string filePath);
    }
}