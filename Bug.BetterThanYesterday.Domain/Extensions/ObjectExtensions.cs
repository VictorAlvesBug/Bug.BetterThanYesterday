using Newtonsoft.Json;
using System.Reflection;

namespace Bug.BetterThanYesterday.Domain.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToJson<T>(this T obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			return JsonConvert.SerializeObject(obj);
		}
		public static T Deserialize<T>(this string json)
		{
			var obj = JsonConvert.DeserializeObject<T>(json);

			if (obj == null)
				throw new Exception($"Não foi possível deserializar o objeto do tipo '{typeof(T).Name}'. Objeto serializado: {json}");

			return obj;
		}

		public static T Copy<T>(this T obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			var json = obj.ToJson();
			var copiedObj = json.Deserialize<T>();

			if (copiedObj == null)
				throw new Exception($"Não foi possível copiar o objeto do tipo '{typeof(T).Name}'. Objeto serializado: {json}");

			return copiedObj!;
		}

		public static string ToQueryString<T>(this T obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

			var parts = props
				.Where(p => p.CanRead)
				.Select(p =>
				{
					var key = Uri.EscapeDataString(p.Name);

					var value = p.GetValue(obj)?.ToString() ?? string.Empty;

					if (p.PropertyType == typeof(DateTime))
						value = ((DateTime)p.GetValue(obj)).ToString("yyyy-MM-dd");
					else if (p.PropertyType == typeof(DateTime?))
						value = ((DateTime?)p.GetValue(obj))?.ToString("yyyy-MM-dd") ?? string.Empty;

					return $"{key}={Uri.EscapeDataString(value)}";
				});

			return string.Join("&", parts);
		}
	}
}