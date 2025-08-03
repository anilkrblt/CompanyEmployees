namespace Shared.DataTransferObjects
{
    //[Serializable] xml için serializable yapıyor ama nesne isimler tırt geliyor
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string? Name { get; set; }
        public string? FullAddress { get; set; }
    }
}
