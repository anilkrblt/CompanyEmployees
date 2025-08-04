namespace Shared.DataTransferObjects
{
    //[Serializable] xml için serializable yapıyor ama nesne isimler tırt geliyor
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string? Name { get; set; }
        public string? FullAddress { get; set; }
    }

    public record CompanyForCreationDto(
        string Name,
        string Address,
        string Country,
        IEnumerable<EmployeeForCreationDto> Employees
    );
}
