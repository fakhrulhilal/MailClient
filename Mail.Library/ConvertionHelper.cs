using System;
using System.ComponentModel;
using System.Globalization;

namespace Mail.Library
{
	public static class ConvertionHelper
	{
		public static bool? ToBoolean(string value, bool? defaultValue = null)
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			if (value == "1") return true;
			if (value == "0") return false;
			return bool.TryParse(value, out bool output) ? output : defaultValue;
		}

		public static int? ToInteger(string value, int? defaultValue = null)
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			var numberFormat = CultureInfo.CurrentCulture.NumberFormat;
			value = value.Replace(numberFormat.NumberGroupSeparator, string.Empty);
			if (int.TryParse(value, out int output)) return output;
			return value.Contains(numberFormat.NumberDecimalSeparator)
				? Convert.ToInt32(decimal.Parse(value))
				: defaultValue;
		}

		public static decimal? ToDecimal(string value, decimal? defaultValue = null)
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			return decimal.TryParse(value, out decimal output) ? output : defaultValue;
		}

		public static double? ToDouble(string value, double? defaultValue = null)
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			return double.TryParse(value, out double output) ? output : defaultValue;
		}

		public static dynamic To(Type type, string value, object defaultValue = null)
		{
			value = (value ?? string.Empty).Trim();
			if (type == typeof(string)) return value;
			if (type == typeof(bool?) || type == typeof(bool))
			{
				var output = ToBoolean(value, (bool?)defaultValue);
				if (type == typeof(bool?)) return output;
				return output ?? Convert.ToBoolean(defaultValue);
			}

			if (type == typeof(decimal?) || type == typeof(decimal))
			{
				var output = ToDecimal(value, defaultValue as decimal?);
				if (type == typeof(decimal?)) return output;
				return output ?? Convert.ToDecimal(defaultValue);
			}

			if (type == typeof(double?) || type == typeof(double))
			{
				var output = ToDouble(value, defaultValue as double?);
				if (type == typeof(double?)) return output;
				return output ?? Convert.ToDouble(defaultValue);
			}

			if (type == typeof(int?) || type == typeof(int))
			{
				var output = ToInteger(value, defaultValue as int?);
				if (type == typeof(int?)) return output;
				return output ?? Convert.ToInt32(defaultValue);
			}

			if (type.IsEnum || Nullable.GetUnderlyingType(type) != null && Nullable.GetUnderlyingType(type).IsEnum)
			{
				var realType = Nullable.GetUnderlyingType(type) ?? type;
				bool isNullable = realType != type;
				try
				{
					var output = Enum.Parse(realType, value, true);
					return !isNullable
						? output
						: TypeDescriptor.GetConverter(type).ConvertFrom(output);
				}
				catch (Exception exception) when (exception is OverflowException || exception is ArgumentException)
				{
					if (isNullable && defaultValue == null) return null;
					if (defaultValue == null) return Activator.CreateInstance(type);
					return Enum.IsDefined(realType, defaultValue)
						? defaultValue
						: Activator.CreateInstance(type);
				}
			}

			throw new NotSupportedException($"Not supported data type: {type.Name}");
		}

		public static TOutput To<TOutput>(string value, TOutput defaultValue = default(TOutput))
			=> (TOutput)To(typeof(TOutput), value, defaultValue);
	}
}