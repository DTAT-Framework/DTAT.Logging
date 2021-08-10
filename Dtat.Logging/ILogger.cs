namespace Dtat.Logging
{
	public interface ILogger<T> where T : class
	{
		bool LogTrace
			(string message, System.Collections.Hashtable parameters = null);

		bool LogDebug
			(string message, System.Collections.Hashtable parameters = null);

		bool LogInformation
			(string message, System.Collections.Hashtable parameters = null);

		bool LogWarning
			(string message, System.Collections.Hashtable parameters = null);

		bool LogError
			(System.Exception exception,
			string message = null, System.Collections.Hashtable parameters = null);

		bool LogCritical
			(System.Exception exception,
			string message = null, System.Collections.Hashtable parameters = null);
	}
}
