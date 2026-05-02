using Bug.BetterThanYesterday.Domain.Commons;

namespace Bug.BetterThanYesterday.Domain.Users.ValueObjects;

public sealed class PixKeyType : Enumeration
{
	public static readonly PixKeyType TaxIdentification = new(1, nameof(TaxIdentification));
	public static readonly PixKeyType Email = new(2, nameof(Email));
	public static readonly PixKeyType PhoneNumber = new(3, nameof(PhoneNumber));
	public static readonly PixKeyType EVP = new(4, nameof(EVP));

	private PixKeyType(int id, string name) : base(id, name) { }

	public static PixKeyType FromId(int id) =>
		GetAll<PixKeyType>().FirstOrDefault(type => type.Id == id)
		?? throw new ArgumentOutOfRangeException(nameof(id), $"ID do PixKeyType inválido: {id}");
	public static PixKeyType FromName(string name) =>
		GetAll<PixKeyType>().FirstOrDefault(type => type.Name == name)
		?? throw new ArgumentOutOfRangeException(nameof(name), $"Nome do PixKeyType inválido: {name}");
}
