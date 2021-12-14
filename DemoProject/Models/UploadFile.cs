using Microsoft.AspNetCore.Http;

public class UploadFile
{
    public string Name { get; set; }
    public IFormFile File { get; set; }
}