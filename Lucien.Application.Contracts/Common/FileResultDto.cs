namespace Lucien.Application.Contracts.Common
{
    public class FileResultDto
    {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }

        public FileResultDto(byte[] fileContent, string contentType, string fileName)
        {
            FileContent = fileContent;
            ContentType = contentType;
            FileName = fileName;
        }
    }

}
