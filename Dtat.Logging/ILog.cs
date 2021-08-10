namespace Dtat.Logging
{
	public interface ILog
	{
		LogLevel Level { get; set; }



		string LocalIP { get; set; }

		string LocalPort { get; set; }



		string RemoteIP { get; set; }

		string RemotePort { get; set; }



		string Username { get; set; }

		string RequestPath { get; set; }

		string HttpReferrer { get; set; }



		string Namespace { get; set; }

		string ClassName { get; set; }

		string MethodName { get; set; }



		string Message { get; set; }

		string Exceptions { get; set; }

		string Parameters { get; set; }
	}
}
