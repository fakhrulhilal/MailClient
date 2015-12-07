using System;
using System.Collections.Generic;
using System.Linq;

namespace Mail.Library
{
	public static class CollectionExtension
	{
		/// <summary>
		/// Select unique string elements.
		/// Similar to <code>IEnumerable&lt;T&gt;.Distinct()</code> but incase sensitive.
		/// Last element will be chosen if found duplicates.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">Collection to be processed</param>
		/// <param name="keySelector">Key to be unique</param>
		/// <returns></returns>
		public static IEnumerable<T> Unique<T>(this IEnumerable<T> collection, Func<T, string> keySelector) where T : class
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			return collection
				.Where(element => !string.IsNullOrWhiteSpace(keySelector(element)))
				.GroupBy(element => keySelector(element).Trim().ToLower())
				.Select(element => element.Last());
		}

		/// <summary>
		/// Select unique string elements.
		/// Similar to <code>IEnumerable&lt;T&gt;.Distinct()</code> but incase sensitive.
		/// Last element will be chosen if found duplicates.
		/// </summary>
		/// <returns>Unique elements</returns>
		public static IEnumerable<string> Unique(this IEnumerable<string> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			return collection
				.Where(element => !string.IsNullOrWhiteSpace(element))
				.GroupBy(element => element.Trim().ToLower())
				.Select(element => element.Last());
		}
	}
}
