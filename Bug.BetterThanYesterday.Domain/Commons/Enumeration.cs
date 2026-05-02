using Bug.BetterThanYesterday.Domain.Extensions;

namespace Bug.BetterThanYesterday.Domain.Commons;

public abstract class Enumeration<T> : IComparable
	where T : Enumeration<T>
{
    public int Id { get; }
    public string Name { get; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

	public static T Get(string idOrName)
	{
		if(int.TryParse(idOrName, out var id))
			return Get(id);
			
		return GetByName(idOrName);
	}

    public static T Get(int id)
    {
        var found = GetAll().FirstOrDefault(x => x.Id == id);

        if (found is not null) return found;

        throw new ArgumentException(
            $"ID '{id}' inválido para {typeof(T).Name}. Use: {GetStringOptions()}"
        );
    }

    public static IReadOnlyList<T> GetAll()
    {
        return typeof(T)
            .GetFields(System.Reflection.BindingFlags.Public |
                       System.Reflection.BindingFlags.Static |
                       System.Reflection.BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => (T)f.GetValue(null)!)
            .ToList();
    }

    public static string GetStringOptions()
    {
        return string.Join(", ", GetAll().Select(x => $"{x.Id}={x.Name}"));
    }

	public int CompareTo(object obj) => Id.CompareTo(((Enumeration<T>)obj).Id);

    private static T GetByName(string name)
    {
        var found = GetAll().FirstOrDefault(x => x.Name == name);

        if (found is not null) return found;

        throw new ArgumentException(
            $"Nome '{name}' inválido para {typeof(T).Name}. Use: {GetStringOptions()}"
        );
    }
}
