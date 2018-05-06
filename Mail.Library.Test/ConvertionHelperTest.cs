using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local - called by NUnit
// ReSharper disable UnusedMember.Global - used for testing

namespace Mail.Library.Test
{
	[TestFixture]
	[Parallelizable(ParallelScope.Children)]
	internal class ConvertionHelperTest
	{
		[OneTimeSetUp]
		public void AllSetup()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		}

		[Test]
		[TestCase("", null, Category = "Boolean", TestName = "Nullable_BooleanEmptyConvertionTest")]
		[TestCase("true", true, Category = "Boolean", TestName = "Nullable_BooleanTrueValueTest")]
		[TestCase("TRUE", true, Category = "Boolean", TestName = "Nullable_BooleanTrueUppercaseValueTest")]
		[TestCase("false", false, Category = "Boolean", TestName = "Nullable_BooleanFalseValueTest")]
		[TestCase("FALSE", false, Category = "Boolean", TestName = "Nullable_BooleanFalseUppercaseValueTest")]
		[TestCase("1", true, Category = "Boolean", TestName = "Nullable_BooleanIntegerOneTest")]
		[TestCase("0", false, Category = "Boolean", TestName = "Nullable_BooleanIntegerZeroTest")]
		public void BooleanNullableTest(string value, bool? expected)
		{
			Assert.That(ConvertionHelper.ToBoolean(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<bool?>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(bool?), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("", false, Category = "Boolean", TestName = "NonNullable_BooleanEmptyConvertionTest")]
		[TestCase("true", true, Category = "Boolean", TestName = "NonNullable_BooleanTrueValueTest")]
		[TestCase("TRUE", true, Category = "Boolean", TestName = "NonNullable_BooleanTrueUppercaseValueTest")]
		[TestCase("false", false, Category = "Boolean", TestName = "NonNullable_BooleanFalseValueTest")]
		[TestCase("FALSE", false, Category = "Boolean", TestName = "NonNullable_BooleanFalseUppercaseValueTest")]
		[TestCase("1", true, Category = "Boolean", TestName = "NonNullable_BooleanIntegerOneTest")]
		[TestCase("0", false, Category = "Boolean", TestName = "NonNullable_BooleanIntegerZeroTest")]
		public void BooleanTest(string value, bool expected)
		{
			Assert.That(ConvertionHelper.ToBoolean(value, default(bool)), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<bool>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(bool), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCaseSource(nameof(DecimalNullableSource), Category = "Decimal")]
		public void DecimalNullableTest(string value, decimal? expected)
		{
			Assert.That(ConvertionHelper.ToDecimal(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<decimal?>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(decimal?), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCaseSource(nameof(DecimalSource), Category = "Decimal")]
		public void DecimalTest(string value, decimal expected)
		{
			Assert.That(ConvertionHelper.ToDecimal(value, default(decimal)), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<decimal>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(decimal), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("abc", null, Category = "Double", TestName = "Nullable_DoubleInvalidValueTest")]
		[TestCase("a1b2c3", null, Category = "Double", TestName = "Nullable_DoubleMixValueTest")]
		[TestCase("", null, Category = "Double", TestName = "Nullable_DoubleEmptyConvertionTest")]
		[TestCase("0", 0d, Category = "Double", TestName = "Nullable_DoubleZeroTest")]
		[TestCase("1", 1d, Category = "Double", TestName = "Nullable_DoubleOneTest")]
		[TestCase("1234", 1234d, Category = "Double", TestName = "Nullable_DoubleThousandTest")]
		[TestCase("1,234", 1234d, Category = "Double", TestName = "Nullable_DoubleThousandSeparatorTest")]
		[TestCase("1234.56", 1234.56d, Category = "Double", TestName = "Nullable_DoubleNormalTest")]
		[TestCase("1,234.56", 1234.56d, Category = "Double", TestName = "Nullable_DoubleThousandWithDoubleTest")]
		public void DoubleNullableTest(string value, double? expected)
		{
			Assert.That(ConvertionHelper.ToDouble(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<double?>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(double?), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("abc", 0, Category = "Double", TestName = "NonNullable_DoubleInvalidValueTest")]
		[TestCase("a1b2c3", 0, Category = "Double", TestName = "NonNullable_DoubleMixValueTest")]
		[TestCase("", 0, Category = "Double", TestName = "NonNullable_DoubleEmptyConvertionTest")]
		[TestCase("0", 0, Category = "Double", TestName = "NonNullable_DoubleZeroTest")]
		[TestCase("1", 1, Category = "Double", TestName = "NonNullable_DoubleOneTest")]
		[TestCase("1234", 1234, Category = "Double", TestName = "NonNullable_DoubleThousandTest")]
		[TestCase("1,234", 1234, Category = "Double", TestName = "NonNullable_DoubleThousandSeparatorTest")]
		[TestCase("1234.56", 1234.56, Category = "Double", TestName = "NonNullable_DoubleNormalTest")]
		[TestCase("1,234.56", 1234.56, Category = "Double", TestName = "NonNullable_DoubleThousandWithDoubleTest")]
		public void DoubleTest(string value, double expected)
		{
			Assert.That(ConvertionHelper.ToDouble(value, default(double)), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<double>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(double), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("1", ExpectedResult = Number.One, Category = "Enumeration", TestName = "Nullable_ByValue")]
		[TestCase("Two", ExpectedResult = Number.Two, Category = "Enumeration",
			TestName = "Nullable_ByMemberNameCaseSensitive")]
		[TestCase("three", ExpectedResult = Number.Three, Category = "Enumeration",
			TestName = "Nullable_ByMemberNameCaseInsensitive")]
		[TestCase(" four ", ExpectedResult = Number.Four, Category = "Enumeration",
			TestName = "Nullable_ByMemberNameContainingSpace")]
		[TestCase("unknown", ExpectedResult = null, Category = "Enumeration", TestName = "Nullable_OutOfScope")]
		public Number? EnumNullableTest(string value) => ConvertionHelper.To<Number?>(value);

		[Test]
		[TestCase("1", ExpectedResult = Number.One, Category = "Enumeration", TestName = "NonNullable_ByValue")]
		[TestCase("Two", ExpectedResult = Number.Two, Category = "Enumeration",
			TestName = "NonNullable_ByMemberNameCaseSensitive")]
		[TestCase("three", ExpectedResult = Number.Three, Category = "Enumeration",
			TestName = "NonNullable_ByMemberNameCaseInsensitive")]
		[TestCase(" four ", ExpectedResult = Number.Four, Category = "Enumeration",
			TestName = "NonNullable_ByMemberNameContainingSpace")]
		[TestCase("unknown", ExpectedResult = default(Number), Category = "Enumeration", TestName = "NonNullable_OutOfScope")]
		public Number EnumTest(string value) => ConvertionHelper.To<Number>(value);

		[Test]
		[TestCase("abc", null, Category = "Integer", TestName = "Nullable_IntegerInvalidValueTest")]
		[TestCase("a1b2c3", null, Category = "Integer", TestName = "Nullable_IntegerMixValueTest")]
		[TestCase("", null, Category = "Integer", TestName = "Nullable_IntegerEmptyConvertionTest")]
		[TestCase("0", 0, Category = "Integer", TestName = "Nullable_IntegerZeroTest")]
		[TestCase("1", 1, Category = "Integer", TestName = "Nullable_IntegerOneTest")]
		[TestCase("1234", 1234, Category = "Integer", TestName = "Nullable_IntegerThousandTest")]
		[TestCase("1,234", 1234, Category = "Integer", TestName = "Nullable_IntegerThousandSeparatorTest")]
		[TestCase("1234.56", 1235, Category = "Integer", TestName = "Nullable_IntegerDecimalTest")]
		[TestCase("1,234.56", 1235, Category = "Integer", TestName = "Nullable_IntegerThousandWithDecimalTest")]
		public void IntegerNullableTest(string value, int? expected)
		{
			Assert.That(ConvertionHelper.ToInteger(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<int?>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(int?), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("abc", 0, Category = "Integer", TestName = "NonNullable_IntegerInvalidValueTest")]
		[TestCase("a1b2c3", 0, Category = "Integer", TestName = "NonNullable_IntegerMixValueTest")]
		[TestCase("", 0, Category = "Integer", TestName = "NonNullable_IntegerEmptyConvertionTest")]
		[TestCase("0", 0, Category = "Integer", TestName = "NonNullable_IntegerZeroTest")]
		[TestCase("1", 1, Category = "Integer", TestName = "NonNullable_IntegerOneTest")]
		[TestCase("1234", 1234, Category = "Integer", TestName = "NonNullable_IntegerThousandTest")]
		[TestCase("1,234", 1234, Category = "Integer", TestName = "NonNullable_IntegerThousandSeparatorTest")]
		[TestCase("1234.56", 1235, Category = "Integer", TestName = "NonNullable_IntegerDecimalTest")]
		[TestCase("1,234.56", 1235, Category = "Integer", TestName = "NonNullable_IntegerThousandWithDecimalTest")]
		public void IntegerTest(string value, int expected)
		{
			Assert.That(ConvertionHelper.ToInteger(value, default(int)), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To<int>(value), Is.EqualTo(expected));
			Assert.That(ConvertionHelper.To(typeof(int), value), Is.EqualTo(expected));
		}

		[Test]
		[TestCase("abc", "abc", Category = "String", TestName = "String_ValidValueTest")]
		[TestCase("a1b2c3", "a1b2c3", Category = "String", TestName = "String_AlphaNumericTest")]
		[TestCase("", "", Category = "String", TestName = "String_EmptyConvertionTest")]
		[TestCase("  ", "", Category = "String", TestName = "String_ContainingSpaceTest")]
		[TestCase("\t Word\n", "Word", Category = "String", TestName = "String_ContainingWhiteSpaceTest")]
		public void StringTest(string value, string expected)
		{
			Assert.That(ConvertionHelper.To<string>(value), Is.EqualTo(expected));
		}

		[Test]
		[Category("Convertion")]
		public void UnSupportedTest()
		{
			string[] keys = { "satu", "Satu", "dua", "DUA" };
			keys.Unique();
			var unique = keys.GroupBy(key => key.ToLower()).Select(key => key.Last()).ToArray();
			Assert.That(unique.Length, Is.EqualTo(2));

			Assert.That(() => ConvertionHelper.To<DateTime>("2015-11-10"), Throws.TypeOf<NotSupportedException>());
			Assert.That(() => ConvertionHelper.To<TimeSpan>("20:30"), Throws.TypeOf<NotSupportedException>());
		}

		#region non test

		private static IEnumerable DecimalSource
		{
			get
			{
				yield return new TestCaseData("abc", 0m).SetName("NonNullable_DecimalInvalidValueTest");
				yield return new TestCaseData("a1b2c3", 0m).SetName("NonNullable_DecimalMixValueTest");
				yield return new TestCaseData("", 0m).SetName("NonNullable_DecimalEmptyConvertionTest");
				yield return new TestCaseData("0", 0m).SetName("NonNullable_DecimalZeroTest");
				yield return new TestCaseData("1", 1m).SetName("NonNullable_DecimalOneTest");
				yield return new TestCaseData("1234", 1234m).SetName("NonNullable_DecimalThousandTest");
				yield return new TestCaseData("1,234", 1234m).SetName("NonNullable_DecimalThousandSeparatorTest");
				yield return new TestCaseData("1234.56", 1234.56m).SetName("NonNullable_DecimalNormalTest");
				yield return new TestCaseData("1,234.56", 1234.56m).SetName("NonNullable_DecimalThousandWithDecimalTest");
			}
		}

		private static IEnumerable DecimalNullableSource
		{
			get
			{
				yield return new TestCaseData("abc", null).SetName("Nullable_DecimalInvalidValueTest");
				yield return new TestCaseData("a1b2c3", null).SetName("Nullable_DecimalMixValueTest");
				yield return new TestCaseData("", null).SetName("Nullable_DecimalEmptyConvertionTest");
				yield return new TestCaseData("0", 0m).SetName("Nullable_DecimalZeroTest");
				yield return new TestCaseData("1", 1m).SetName("Nullable_DecimalOneTest");
				yield return new TestCaseData("1234", 1234m).SetName("Nullable_DecimalThousandTest");
				yield return new TestCaseData("1,234", 1234m).SetName("Nullable_DecimalThousandSeparatorTest");
				yield return new TestCaseData("1234.56", 1234.56m).SetName("Nullable_DecimalNormalTest");
				yield return new TestCaseData("1,234.56", 1234.56m).SetName("Nullable_DecimalThousandWithDecimalTest");
			}
		}

		public enum Number
		{
			One = 1,
			Two = 2,
			Three = 3,
			Four = 4,
			Five = 5
		}

		#endregion
	}
}