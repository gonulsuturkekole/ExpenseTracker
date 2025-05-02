namespace ExpenseTracker.Base;

public interface IFileService
{
    Task<string> UploadFileAsync(string fileName, MemoryStream memoryStream);
}