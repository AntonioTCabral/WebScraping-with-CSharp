namespace WebScraping.Core.Entities
{
    public class FileInformation : BaseEntity
    {
        public string Name { get; set; }
        public string Lines { get; set; }
        public string Length { get; set; }
        public string Extension { get; set; }
    }
}