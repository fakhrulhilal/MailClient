namespace Mail.Library
{
	/// <summary>
	/// Specify interface which can be validated
	/// </summary>
	/// <typeparam name="T">validation result object type</typeparam>
	public interface IValidatable<T> where T : IValidateResult
	{
		/// <summary>
		/// Validate <typeparamref name="T" /> model
		/// </summary>
		/// <returns>Validation result</returns>
		T Validate();

		/// <summary>
		/// Validate <typeparamref name="T" /> with prefix of each error validation message
		/// </summary>
		/// <param name="prefix">Prefix to be add on each error validation message</param>
		/// <returns></returns>
		T Validate(string prefix);
	}

	/// <summary>
	/// Specify interface which can be validated
	/// </summary>
	public interface IValidatable : IValidatable<Validation>
	{
	}

	/// <summary>
	/// Contract for validation result
	/// </summary>
	public interface IValidateResult
	{
		/// <summary>
		/// Define validation status: <code>true</code> for success and vice versa
		/// </summary>
		bool IsValid { get; }
	}
}