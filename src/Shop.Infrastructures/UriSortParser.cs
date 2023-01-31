using System.Linq.Expressions;
using System.Reflection;

namespace Shop.Infrastructures
{
    public class UriSortParser
    {
        public ISort<T> Parse<T>(string expression)
        {
            var sortExprs = expression.Trim().Split(',');
            var sorts = sortExprs.Select(ExpressionToSort<T>);
            return sorts.Aggregate(
                (ISort<T>)null, 
                (previous, current) => previous != null ? previous.And(current) : current);
        }

        private ISort<T> ExpressionToSort<T>(string expression)
        {
            var trimmedExpression = expression.Trim();
            var prefix = trimmedExpression[0];
            var propertyName = trimmedExpression.TrimStart('+', '-');

            if (prefix == '+')
                return Sort<T>.By(propertyName, SortDirection.Ascending);

            return Sort<T>.By(propertyName, SortDirection.Descending);
        }
    }

    public interface ISort<T>
    {
        ISort<T> And(ISort<T> sort);
        (MemberInfo Property, SortDirection Direction)[] OrdersToArray();
        Dictionary<MemberInfo, SortDirection> GetOrders();
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class Sort<T> : ISort<T>
    {
        private readonly Dictionary<MemberInfo, SortDirection> _orders;

        public (MemberInfo Property, SortDirection Direction)[] OrdersToArray()
        {
            return _orders.Select(_ => (_.Key, _.Value)).ToArray();
        }

        public Dictionary<MemberInfo, SortDirection> GetOrders()
        {
            return _orders;
        }

        private Sort(IEnumerable<KeyValuePair<MemberInfo, SortDirection>> orders)
        {
            _orders = orders.GroupBy(
                _ => _.Key).ToDictionary(_ => _.Key, _ => _.Last().Value);
        }

        public ISort<T> And(ISort<T> sort)
        {
            return new Sort<T>(_orders.Concat(sort.GetOrders()));
        }

        public static Sort<T> By(string propertyName, SortDirection direction)
        {
            var property = ResolveProperty(propertyName);
            return new Sort<T>(new[]
            {
            new KeyValuePair<MemberInfo, SortDirection>(property, direction)
        });
        }

        private static MemberInfo ResolveProperty(string propertyName)
        {
            var type = typeof(T);
            var searchFlags = BindingFlags.Public |
                              BindingFlags.Instance |
                              BindingFlags.IgnoreCase;
            return (MemberInfo)type.GetProperty(
                propertyName,
                searchFlags) ?? type.GetField(propertyName, searchFlags);
        }
    }

    public interface IPageResult<T>
    {
        public IEnumerable<T> Elements { get; }
        public int TotalElements { get; }
    }

    public static class SortHelper
    {
        private static readonly Lazy<MethodInfo> _queryableOrderByMethod;

        private static readonly Lazy<MethodInfo> _queryableThenByMethod;

        private static readonly Lazy<MethodInfo> _queryableOrderByDescendingMethod;

        private static readonly Lazy<MethodInfo> _queryableThenByDescendingMethod;

        static SortHelper()
        {
            _queryableThenByDescendingMethod = new Lazy<MethodInfo>(
                ()
                => ResolveQueryableMethod(nameof(Queryable.ThenByDescending),
                parameterCount: 2));

            _queryableOrderByDescendingMethod = new Lazy<MethodInfo>(
                ()
                => ResolveQueryableMethod(nameof(Queryable.OrderByDescending),
                parameterCount: 2));

            _queryableThenByMethod = new Lazy<MethodInfo>(
                ()
                => ResolveQueryableMethod(nameof(Queryable.ThenBy),
                parameterCount: 2));

            _queryableOrderByMethod = new Lazy<MethodInfo>(
                ()
                => ResolveQueryableMethod(nameof(Queryable.OrderBy),
                parameterCount: 2));
        }

        private static MethodInfo ResolveQueryableMethod(
            string name,
            int parameterCount)
        {
            var classType = typeof(Queryable);
            var searchFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;
            return classType
                .GetMember(name, searchFlags)
                .OfType<MethodInfo>()
                .Single(_ => _.GetParameters().Length == parameterCount);
        }

        public static IQueryable<T> Sort<T>(
            this IQueryable<T> source,
            ISort<T>? sort)
        {
            var query = source;

            var sortOrders = sort!.OrdersToArray();
            for (var propertyIndex = 0;
                 propertyIndex < sortOrders.Length;
                 propertyIndex++)
            {
                var (property, direction) = sortOrders[propertyIndex];
                var secondLevel = propertyIndex > 0;
                query = SortSingleProperty(
                                query,
                                property,
                                direction,
                                secondLevel);
            }

            return query;
        }

        private static IQueryable<T> SortSingleProperty<T>(
            IQueryable<T> query,
            MemberInfo member,
            SortDirection sortDirection,
            bool isSecondLevel)
        {
            var sourceType = typeof(T);
            var parameter = Expression.Parameter(sourceType, name: "_");
            var propertyExpr = Expression.MakeMemberAccess(parameter, member);
            var lambdaPropertyExpr = Expression.Lambda(propertyExpr, parameter);

            var sortMethod = (sortDirection, isSecondLevel) switch
            {
                (SortDirection.Ascending, Item2: false)
                    => _queryableOrderByMethod.Value,
                (SortDirection.Ascending, Item2: true)
                    => _queryableThenByMethod.Value,
                (SortDirection.Descending, Item2: false)
                    => _queryableOrderByDescendingMethod.Value,
                (SortDirection.Descending, Item2: true)
                    => _queryableThenByDescendingMethod.Value,
            };

            query = (IQueryable<T>)sortMethod
                .MakeGenericMethod(sourceType, GetMemberType(member))
                .Invoke(
                    null,
                    new object[] { query, lambdaPropertyExpr });

            return query;
        }

        private static Type GetMemberType(MemberInfo member)
        {
            return member switch
            {
                PropertyInfo property => property.PropertyType,
                FieldInfo field => field.FieldType,

                _ => throw new ArgumentException(
                $"member '{member.DeclaringType.Name}.{member.Name}'" +
                $" not found.")
            };
        }
    }

    public static class PaginationHelper
    {
        public static IQueryable<T> Page<T>(
            this IQueryable<T> source, Pagination? pagination)
        {
            var query = source;

            if (pagination != null && pagination.PageNumber > 1)
            {
                query = query.Skip(
                    (pagination.PageNumber - 1) * pagination.PageSize);
            }

            if (pagination != null)
                query = query.Take(pagination.PageSize);

            return query;
        }
    }

    public class PageResult<T> : IPageResult<T>
    {
        public IEnumerable<T> Elements { get; private set; }
        public int TotalElements { get; private set; }

        public PageResult(IEnumerable<T> elements, int totalElements)
        {
            Elements = elements;
            TotalElements = totalElements;
        }
    }
}
