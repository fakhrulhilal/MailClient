using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Mail.Library.Test
{
	[TestFixture]
	public class CollectionExtensionTest
	{
		[Test]
		[TestCaseSource(nameof(ComplexSource), Category = "Collection")]
		public Complex[] ComplexCollectionTest(IEnumerable<Complex> collection)
		{
			var r = collection.Unique(element => element.Name).ToArray();
			return r;
		}

		[Test]
		[TestCaseSource(nameof(SimpleSource), Category = "Collection")]
		public IEnumerable<string> SimpleCollectionTest(IEnumerable<string> collection) => collection.Unique();

		#region non test

		public static IEnumerable SimpleSource
		{
			get
			{
				yield return new TestCaseData(new List<string> { "one", "One", "Two", "TWO", "three" })
							 .Returns(new[] { "One", "TWO", "three" })
							 .SetName("Simple_Duplicate");
				yield return new TestCaseData(new List<string> { "one", "Two", "three" })
							 .Returns(new[] { "one", "Two", "three" })
							 .SetName("Simple_NoDuplicate");
				yield return new TestCaseData(new List<string> { "one", "  ", "Two", "", "three" })
							 .Returns(new[] { "one", "Two", "three" })
							 .SetName("Simple_ContainEmptyElement");
				yield return new TestCaseData(new List<string> { "one", null, "Two", "three" })
							 .Returns(new[] { "one", "Two", "three" })
							 .SetName("Simple_ContainNullElement");
				yield return new TestCaseData(new List<string> { "one", null, "Two", "three", "another", "Another   " })
							 .Returns(new[] { "one", "Two", "three", "Another   " })
							 .SetName("Simple_ContainSpace");
				yield return new TestCaseData(new List<string> { "one", null, "Two", "three", "another", "\tAnother   " })
							 .Returns(new[] { "one", "Two", "three", "\tAnother   " })
							 .SetName("Simple_ContainWhiteSpace");
			}
		}

		public static IEnumerable ComplexSource
		{
			get
			{
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = "One", Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "TWO", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "One", Value = "Dummy" }, new Complex { Name = "TWO", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).SetName("Complex_Duplicate");
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "one", Value = "Dummy" }, new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).SetName("Complex_NoDuplicate");
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = "  ", Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "one", Value = "Dummy" }, new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).SetName("Complex_ContainEmptyElement");
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = null, Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "one", Value = "Dummy" }, new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }
				}).SetName("Complex_ContainNullElement");
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = null, Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" },
					new Complex { Name = "another", Value = "Dummy" },
					new Complex { Name = "Another   ", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "one", Value = "Dummy" }, new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }, new Complex { Name = "Another   ", Value = "Dummy" }
				}).SetName("Complex_ContainSpace");
				yield return new TestCaseData(new List<Complex>
				{
					new Complex { Name = "one", Value = "Dummy" },
					new Complex { Name = null, Value = "Dummy" },
					new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" },
					new Complex { Name = "another", Value = "Dummy" },
					new Complex { Name = "\tAnother   ", Value = "Dummy" }
				}).Returns(new[]
				{
					new Complex { Name = "one", Value = "Dummy" }, new Complex { Name = "Two", Value = "Dummy" },
					new Complex { Name = "three", Value = "Dummy" }, new Complex { Name = "\tAnother   ", Value = "Dummy" }
				}).SetName("Complex_ContainWhiteSpace");
			}
		}

		public class Complex
		{
			public string Name { get; set; }
			public string Value { get; set; }

			public override bool Equals(object obj)
			{
				var comparer = obj as Complex;
				if (comparer == null) return false;
				return Name == comparer.Name && Value == comparer.Value;
			}

			public override int GetHashCode() => ToString().GetHashCode();

			public override string ToString() => $"{Name} - {Value}";
		}

		#endregion
	}
}