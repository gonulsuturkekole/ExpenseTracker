using ExpenseTracker.Base;

namespace Api;

public class LocalFileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public LocalFileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadFileAsync(string fileName, MemoryStream memoryStream)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        // create the directory if it does not exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        /*
        
        fileName: gonul.pdf

        -- string extension = Path.GetExtension(fileName);

        extension: .pdf

        -- string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        fileNameWithoutExtension: gonul

        -- string newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";

        newFileName: gonul_<GUID>.pdf

        -- string filePath = Path.Combine(folderPath, fileName);

        filePath: uploads/gonul_<GUID>.pdf

         */

        // extract the file extension for some specific purpose - like validation
        string extension = Path.GetExtension(fileName);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";
        string filePath = Path.Combine(folderPath, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await memoryStream.CopyToAsync(stream);
        }

        return filePath;
    }
}