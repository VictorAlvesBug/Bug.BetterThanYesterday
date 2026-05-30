using Bug.BetterThanYesterday.Domain.Extensions;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace Bug.BetterThanYesterday.Application.Mocks
{
	public class MockBuilder<T> where T : class
	{
		private readonly T _instance;

		public MockBuilder(T seed)
		{
			_instance = seed.Copy();
		}

		public MockBuilder<T> With<TProp>(
			Expression<Func<T, TProp>> propertyExpression,
			TProp value)
		{
			if (propertyExpression.Body is not MemberExpression memberExpression)
				throw new ArgumentException("Expression must point to a property.");

			if (memberExpression.Member is not PropertyInfo propertyInfo)
				throw new ArgumentException("Expression must pount to a property.");

			if (!propertyInfo.CanWrite)
				throw new ArgumentException($"Property '{propertyInfo.Name}' is read-only.");

			propertyInfo.SetValue(_instance, value);
			return this;
		}

		public T Build() => _instance;
	}
}
