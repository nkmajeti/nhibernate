using System;
using System.Data;


namespace NHibernate.Connection {
	/// <summary>
	/// An implementation of the <c>IConnectionProvider</c> that simply throws an exception when
	/// a connection is requested.
	/// </summary>
	/// <remarks>
	/// This implementation indicates that the user is expected to supply an ADO.NET connection
	/// </remarks>
	public class UserSuppliedConnectionProvider : IConnectionProvider {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UserSuppliedConnectionProvider));
		
		public void CloseConnection(IDbConnection conn) {
			throw new InvalidOperationException("The user must supply an ADO.NET connection");
		}

		public IDbConnection GetConnection() {
			throw new InvalidOperationException("The user must supply an ADO.NET connection");
		}

		public void Configure(ConnectionProviderSettings settings) {
			log.Warn("No connection properties specified - the user must supply an ADO.NET connection");
		}

		public bool IsStatementCache {
			get { return false; }
		}
	}
}
