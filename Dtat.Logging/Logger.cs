namespace Dtat.Logging
{
	public abstract class Logger<T> : object, ILogger<T> where T : class
	{
		#region Constructor
		protected Logger
			(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor = null) : base()
		{
			// **************************************************
			HttpContextAccessor = httpContextAccessor;
			// **************************************************

			// **************************************************
			//if (httpContextAccessor == null)
			//{
			//	throw new System.ArgumentNullException(nameof(httpContextAccessor));
			//}

			//HttpContextAccessor = httpContextAccessor;
			// **************************************************

			// **************************************************
			//HttpContextAccessor =
			//	httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
			// **************************************************
		}
		#endregion /Constructor

		protected Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; }

		#region GetExceptions
		protected virtual string GetExceptions(System.Exception exception)
		{
			if (exception == null)
			{
				return null;
			}

			var stringBuilder =
				new System.Text.StringBuilder();

			int index = 0;
			System.Exception currentException = exception;

			while (currentException != null)
			{
				if (index == 0)
				{
					stringBuilder.Append($"<{ nameof(System.Exception) }>");
				}
				else
				{
					stringBuilder.Append($"<{ nameof(System.Exception.InnerException) }>");
				}

				stringBuilder.Append(currentException.Message);

				if (index == 0)
				{
					stringBuilder.Append($"</{ nameof(System.Exception) }>");
				}
				else
				{
					stringBuilder.Append($"</{ nameof(System.Exception.InnerException) }>");
				}

				index++;

				currentException =
					currentException.InnerException;
			}

			string result =
				stringBuilder.ToString();

			return result;
		}
		#endregion /GetExceptions

		#region GetParameters
		protected virtual string GetParameters(System.Collections.Hashtable parameters)
		{
			if ((parameters == null) || (parameters.Count == 0))
			{
				return null;
			}

			var stringBuilder =
				new System.Text.StringBuilder();

			foreach (System.Collections.DictionaryEntry item in parameters)
			{
				if (item.Key != null)
				{
					stringBuilder.Append($"<{ nameof(item.Key) }>{ item.Key }</{ nameof(item.Key) }>");

					if (item.Value == null)
					{
						stringBuilder.Append($"<{ nameof(item.Value) }>NULL</{ nameof(item.Value) }>");
					}
					else
					{
						stringBuilder.Append($"<{ nameof(item.Value) }>{ item.Value }</{ nameof(item.Value) }>");
					}
				}
			}

			string result =
				stringBuilder.ToString();

			return result;
		}
		#endregion /GetParameters

		#region Log
		protected bool Log
			(LogLevel level,
			System.Reflection.MethodBase methodBase,
			string message,
			System.Exception exception = null,
			System.Collections.Hashtable parameters = null)
		{
			try
			{
				// **************************************************
				string currentCultureName =
					System.Threading.Thread.CurrentThread.CurrentCulture.Name;

				var newCultureInfo =
					new System.Globalization.CultureInfo(name: "en-US");

				var currentCultureInfo =
					new System.Globalization.CultureInfo(currentCultureName);

				System.Threading.Thread.CurrentThread.CurrentCulture = newCultureInfo;
				// **************************************************

				var log = new Log
				{
					Level = level,
				};



				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Connection != null) &&
					(HttpContextAccessor.HttpContext.Connection.LocalIpAddress != null))
				{
					log.LocalIP =
						HttpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
				}

				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Connection != null))
				{
					log.LocalPort =
						HttpContextAccessor.HttpContext.Connection.LocalPort.ToString();
				}



				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Connection != null) &&
					(HttpContextAccessor.HttpContext.Connection.RemoteIpAddress != null))
				{
					log.RemoteIP =
						HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
				}

				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Connection != null))
				{
					log.RemotePort =
						HttpContextAccessor.HttpContext.Connection.RemotePort.ToString();
				}



				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.User != null) &&
					(HttpContextAccessor.HttpContext.User.Identity != null))
				{
					log.Username =
						HttpContextAccessor.HttpContext.User.Identity.Name;
				}

				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Request != null))
				{
					log.RequestPath =
						HttpContextAccessor.HttpContext.Request.Path;
				}

				if ((HttpContextAccessor != null) &&
					(HttpContextAccessor.HttpContext != null) &&
					(HttpContextAccessor.HttpContext.Request != null))
				{
					log.HttpReferrer =
						HttpContextAccessor.HttpContext.Request.Headers["Referer"];
				}



				log.ClassName = typeof(T).Name;
				log.MethodName = methodBase.Name;
				log.Namespace = typeof(T).Namespace;



				log.Message = message;

				log.Exceptions =
					GetExceptions(exception: exception);

				log.Parameters =
					GetParameters(parameters: parameters);



				LogByFavoriteLibrary(log: log, exception: exception);

				// **************************************************
				System.Threading.Thread.CurrentThread.CurrentCulture = currentCultureInfo;
				// **************************************************

				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion /Log

		protected abstract void LogByFavoriteLibrary(Log log, System.Exception exception);

		#region LogTrace
		public virtual bool LogTrace
			(string message, System.Collections.Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Trace,
				message: message,
				exception: null,
				parameters: parameters);

			return result;
		}
		#endregion /LogTrace

		#region LogDebug
		public virtual bool LogDebug
			(string message, System.Collections.Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Debug,
				message: message,
				exception: null,
				parameters: parameters);

			return result;
		}
		#endregion /LogDebug

		#region LogInformation
		public virtual bool LogInformation
			(string message, System.Collections.Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Information,
				message: message,
				exception: null,
				parameters: parameters);

			return result;
		}
		#endregion /LogInformation

		#region LogWarning
		public virtual bool LogWarning
			(string message, System.Collections.Hashtable parameters = null)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Warning,
				message: message,
				exception: null,
				parameters: parameters);

			return result;
		}
		#endregion /LogWarning

		#region LogError
		public virtual bool LogError
			(System.Exception exception,
			string message = null, System.Collections.Hashtable parameters = null)
		{
			if (exception == null)
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Error,
				message: message,
				exception: exception,
				parameters: parameters);

			return result;
		}
		#endregion /LogError

		#region LogCritical
		public virtual bool LogCritical
			(System.Exception exception,
			string message = null, System.Collections.Hashtable parameters = null)
		{
			if (exception == null)
			{
				return false;
			}

			// **************************************************
			var stackTrace =
				new System.Diagnostics.StackTrace();

			System.Reflection.MethodBase methodBase = null;

			if (stackTrace.GetFrame(index: 1) != null)
			{
				methodBase =
					stackTrace.GetFrame(index: 1).GetMethod();
			}
			// **************************************************

			bool result =
				Log(methodBase: methodBase,
				level: LogLevel.Critical,
				message: message,
				exception: exception,
				parameters: parameters);

			return result;
		}
		#endregion /LogCritical
	}
}
