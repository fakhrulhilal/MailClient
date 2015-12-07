namespace Mail.Library
{
	/// <summary>
	/// Collection of <see cref="System.Exception"/> helper
	/// </summary>
	public static class ExceptionExtension
	{
		/// <summary>
		/// Get leaf inner exception message
		/// </summary>
		/// <param name="exception"></param>
		/// <returns><see cref="System.Exception.Message"/> of leaf inner <paramref name="exception"/></returns>
		public static string GetLeafException(this System.Exception exception)
		{
			if (exception == null) throw new System.ArgumentNullException(nameof(exception));
			return exception.InnerException != null ? exception.GetLeafException() : exception.Message;
		}
	}
}
