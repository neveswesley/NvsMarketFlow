using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }

    public string CorporateName { get; private set; }
    public string FantasyName { get; private set; }
    public string CNPJ { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }

    public Status Status { get; private set; }

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Supplier()
    {
    }

    public Supplier(string corporateName, string fantasyName, string cnpj, string phone, string email,
        string address, Status status)
    {
        if (string.IsNullOrWhiteSpace(corporateName))
            throw new ArgumentException("Corporate name cannot be empty;");

        if (string.IsNullOrWhiteSpace(fantasyName))
            throw new ArgumentException("Fantasy name cannot be empty;");

        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ cannot be empty;");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty;");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty;");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty;");

        Id = Guid.NewGuid();
        CorporateName = corporateName;
        FantasyName = fantasyName;
        CNPJ = cnpj;
        Phone = phone;
        Email = email;
        Address = address;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateInfo(string corporateName, string fantasyName, string cnpj, string phone, string email,
        string address)
    {
        if (string.IsNullOrWhiteSpace(corporateName))
            throw new ArgumentException("Corporate name cannot be empty;");

        if (string.IsNullOrWhiteSpace(fantasyName))
            throw new ArgumentException("Fantasy name cannot be empty;");

        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ cannot be empty;");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty;");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty;");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty;");

        CorporateName = corporateName;
        FantasyName = fantasyName;
        CNPJ = cnpj;
        Phone = phone;
        Email = email;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == Status.Inactive)
            throw new InvalidOperationException("Supplier is already inactive.");

        Status = Status.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == Status.Active)
            throw new InvalidOperationException("Supplier is already active.");

        Status = Status.Active;
        UpdatedAt = DateTime.UtcNow;
    }
}