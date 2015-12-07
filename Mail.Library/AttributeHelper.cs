using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mail.Library
{
	/// <summary>
	/// Helper for getting attribute value
	/// </summary>
	public static class AttributeHelper
	{
		/// <summary>
		/// Get class attribute value
		/// </summary>
		/// <typeparam name="TAttribute">Attribute type</typeparam>
		/// <typeparam name="TExpected">Data type of attribute's member from <paramref name="attributeSelector"/></typeparam>
		/// <param name="classType">Class type</param>
		/// <param name="attributeSelector">Select member of <typeparamref name="TAttribute"/></param>
		/// <returns>The value of attribute's member from class attribute</returns>
		public static TExpected[] ClassAttribute<TAttribute, TExpected>(this Type classType,
			Expression<Func<TAttribute, TExpected>> attributeSelector)
		{
			if (classType == null) throw new ArgumentNullException(nameof(classType));
			var memberExpression = attributeSelector.Body as MemberExpression;
			if (memberExpression == null) throw new ArgumentException("Please supply expression to field/property", nameof(attributeSelector));
			TAttribute[] attributes = classType
				.GetCustomAttributes(typeof(TAttribute), false)
				.Cast<TAttribute>().ToArray();
			return attributes.Length < 1 ? new TExpected[0] : attributes.Select(attributeSelector.Compile()).ToArray();
		}

		/// <summary>
		/// Get class attribute value
		/// </summary>
		/// <typeparam name="TClass">Class type</typeparam>
		/// <typeparam name="TAttribute">Attribute type</typeparam>
		/// <typeparam name="TExpected">Data type of attribute's member from <paramref name="attributeSelector"/></typeparam>
		/// <param name="attributeSelector"></param>
		/// <returns>The value of attribute's member from class attribute</returns>
		public static TExpected[] ClassAttribute<TClass, TAttribute, TExpected>(
			Expression<Func<TAttribute, TExpected>> attributeSelector)
			=> ClassAttribute(typeof(TClass), attributeSelector);

		/// <summary>
		/// Get class member's (property/field) attribute value
		/// </summary>
		/// <typeparam name="TAttribute">Attribute type</typeparam>
		/// <typeparam name="TExpected">Data type of attribute's member from <paramref name="attributeSelector"/></typeparam>
		/// <param name="classType">Class type</param>
		/// <param name="memberName">Select member name from <paramref name="classType"/></param>
		/// <param name="attributeSelector">Select member of <typeparamref name="TAttribute"/></param>
		/// <returns>The attribute's value from class member</returns>
		public static TExpected[] MemberAttribute<TAttribute, TExpected>(this Type classType, string memberName, Expression<Func<TAttribute, TExpected>> attributeSelector)
		{
			if (classType == null) throw new ArgumentNullException(nameof(classType));
			if (string.IsNullOrEmpty(memberName)) throw new ArgumentNullException(nameof(memberName));
			var attributeExpression = attributeSelector.Body as MemberExpression;
			if (attributeExpression == null) throw new ArgumentException("Please supply expression to field/property of attribute member", nameof(attributeExpression));
			MemberInfo[] memberInfos = classType.GetMember(memberName);
			if (memberInfos.Length < 1) throw new ArgumentException($"'{memberName}' is not valid member of {classType.Name}");
			TAttribute[] attributes = memberInfos[0].GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>().ToArray();
			return attributes.Length < 1 ? new TExpected[0] : attributes.Select(attributeSelector.Compile()).ToArray();
		}

		/// <summary>
		/// Get class member's (property/field) attribute value
		/// </summary>
		/// <typeparam name="TClass"></typeparam>
		/// <typeparam name="TAttribute">Attribute type</typeparam>
		/// <typeparam name="TExpected">Data type of attribute's member from <paramref name="attributeSelector"/></typeparam>
		/// <param name="memberName">Select member name from <typeparamref name="TClass"/></param>
		/// <param name="attributeSelector">Select member of <typeparamref name="TAttribute"/></param>
		/// <returns>The attribute's value from class member</returns>
		public static TExpected[] MemberAttribute<TClass, TAttribute, TExpected>(string memberName, Expression<Func<TAttribute, TExpected>> attributeSelector) 
			=> MemberAttribute(typeof(TClass), memberName, attributeSelector);

		/// <summary>
		/// Get class member's (property/field) attribute value
		/// </summary>
		/// <typeparam name="TClass"></typeparam>
		/// <typeparam name="TAttribute">Attribute type</typeparam>
		/// <typeparam name="TExpected">Data type of attribute's member from <paramref name="attributeSelector"/></typeparam>
		/// <param name="memberSelector"></param>
		/// <param name="attributeSelector">Select member of <typeparamref name="TAttribute"/></param>
		/// <returns>The attribute's value from class member</returns>
		public static TExpected[] MemberAttribute<TClass, TAttribute, TExpected>(
			Expression<Func<TClass, object>> memberSelector,
			Expression<Func<TAttribute, TExpected>> attributeSelector)
		{
			var classExpression = memberSelector.Body as MemberExpression;
			if (classExpression == null) throw new ArgumentException("Please supply expression to field/property of class member", nameof(memberSelector));
			return MemberAttribute<TClass, TAttribute, TExpected>(classExpression.Member.Name, attributeSelector);
		}
	}
}
