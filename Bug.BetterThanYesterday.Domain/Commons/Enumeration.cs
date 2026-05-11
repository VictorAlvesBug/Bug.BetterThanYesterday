using Bug.BetterThanYesterday.Domain.Extensions;
using Microsoft.VisualBasic;

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

	public static T Get(string? idOrName)
	{
		if(int.TryParse(idOrName, out var id))
			return Get(id);
			
		return GetByName(idOrName);
	}

	public static bool TryGet(string? idOrName, out T? found, out string? errorMessage)
	{
		if(int.TryParse(idOrName, out var id))
            return TryGet(id, out found, out errorMessage);
			
        return TryGetByName(idOrName, out found, out errorMessage);
	}

    public static T Get(int id)
    {
        if(TryGet(id, out var found, out var errorMessage))
            return found!;

        throw new ArgumentException(errorMessage);
    }

    public static bool TryGet(int id, out T? found, out string? errorMessage)
    {
        found = GetAll().FirstOrDefault(x => x.Id == id);
        errorMessage = found is null
            ? $"ID '{id}' inválido para {typeof(T).Name}. Use: {GetStringOptions()}"
            : null;
        return found is not null;
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

	public int CompareTo(object obj)
    {
        return ((Enumeration<T>)obj).Id;
    }

    private static T GetByName(string? name)
    {
        if(TryGetByName(name, out var found, out var errorMessage))
            return found!;

        throw new ArgumentException(errorMessage);
    }
    private static bool TryGetByName(string? name, out T? found, out string? errorMessage)
    {
        found = GetAll().FirstOrDefault(x => x.Name == name);
        errorMessage = found is null 
            ? $"Nome '{name}' inválido para {typeof(T).Name}. Use: {GetStringOptions()}"
            : null;
        return found is not null;
    }
}
