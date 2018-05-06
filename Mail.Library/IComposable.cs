namespace Mail.Library
{
	/// <summary>
	/// Determine interface which produce an object
	/// </summary>
	/// <typeparam name="TObject">Object result type</typeparam>
	public interface IComposable<TObject> where TObject : class
	{
		TObject Compose();
	}
}