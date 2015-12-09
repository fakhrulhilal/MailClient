using System;
using NUnit.Framework;

namespace Mail.Library.Test
{
	[TestFixture]
	class AttributeHelperTest
	{
		[Test]
		public void ClassTest()
		{
			var classNames = AttributeHelper.Class<ClassWithAttribute, ClassMultipleAttribute, string>(attr => attr.Name);
			Assert.That(classNames.Length, Is.EqualTo(2));
			Assert.That(classNames, Is.EquivalentTo(new[] { "dummy", "dummy2" }));

			var classNumbers = AttributeHelper.Class<ClassWithAttribute, ClassMultipleAttribute, int>(attr => attr.Number);
			Assert.That(classNumbers.Length, Is.EqualTo(2));
			Assert.That(classNumbers, Is.EquivalentTo(new[] { 2, default(int) }));

			var dummyObject = new ClassWithAttribute();
			classNames = dummyObject.GetType().ClassAttribute<ClassMultipleAttribute, string>(attr => attr.Name);
			Assert.That(classNames.Length, Is.EqualTo(2));
			Assert.That(classNames, Is.EquivalentTo(new[] { "dummy", "dummy2" }));

			classNumbers = dummyObject.GetType().ClassAttribute<ClassMultipleAttribute, int>(attr => attr.Number);
			Assert.That(classNumbers.Length, Is.EqualTo(2));
			Assert.That(classNumbers, Is.EquivalentTo(new[] { 2, default(int) }));
		}

		[Test]
		public void MemberTest()
		{
			var memberNames = AttributeHelper.Member<ClassWithAttribute, MemberMultipleAttribute, string>(@class => @class.Data, attr => attr.Key);
			Assert.That(memberNames.Length, Is.EqualTo(2));
			Assert.That(memberNames, Is.EquivalentTo(new[] { "10", default(string) }));

			var memberNumbers = AttributeHelper.Member<ClassWithAttribute, MemberMultipleAttribute, double>(@class => @class.Data, attr => attr.Value);
			Assert.That(memberNumbers.Length, Is.EqualTo(2));
			Assert.That(memberNumbers, Is.EquivalentTo(new[] { 10d, 5d }));

			var dummyObject = new ClassWithAttribute();
			memberNames = dummyObject.GetType().MemberAttribute<MemberMultipleAttribute, string>("Data", attr => attr.Key);
			Assert.That(memberNames.Length, Is.EqualTo(2));
			Assert.That(memberNames, Is.EquivalentTo(new[] { "10", default(string) }));

			memberNumbers = dummyObject.GetType().MemberAttribute<MemberMultipleAttribute, double>("Data", attr => attr.Value);
			Assert.That(memberNumbers.Length, Is.EqualTo(2));
			Assert.That(memberNumbers, Is.EquivalentTo(new[] { 10d, 5d }));
		}

		#region Attribute definition

		[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
		internal class ClassMultipleAttribute : Attribute
		{
			public string Name { get; private set; }
			public int Number { get; set; }

			public ClassMultipleAttribute(string name)
			{
				if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
				Name = name;
			}
		}

		[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = true)]
		internal class MemberMultipleAttribute : Attribute
		{
			public double Value { get; set; }
			public string Key { get; set; }

			public MemberMultipleAttribute(double value)
			{
				Value = value;
			}
		}

		#endregion

		#region Class definition

		[ClassMultiple("dummy", Number = 2), ClassMultiple("dummy2")]
		internal class ClassWithAttribute
		{
			[MemberMultiple(10d, Key = "10"), MemberMultiple(5d)]
			public string Data { get; set; }
		}

		#endregion
	}
}
