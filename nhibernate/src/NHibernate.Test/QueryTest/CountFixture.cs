using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using NHibernate.DomainModel;
using NUnit.Framework;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	public class CountFixture
	{
		[Test]
		public void Default()
		{
			Configuration cfg = new Configuration();
			cfg.AddResource("NHibernate.DomainModel.Simple.hbm.xml", typeof (Simple).Assembly);
			cfg.SetProperty(Cfg.Environment.Hbm2ddlAuto, "create-drop");
			ISessionFactory sf = cfg.BuildSessionFactory();

			using (ISession s = sf.OpenSession())
			{
				object count = s.CreateQuery("select count(*) from Simple").UniqueResult();
				Assert.IsTrue(count is Int64);
			}
			sf.Close();
		}

		[Test]
		public void Overridden()
		{
			Configuration cfg = new Configuration();
			cfg.SetProperty(Cfg.Environment.Hbm2ddlAuto, "create-drop");
			cfg.AddResource("NHibernate.DomainModel.Simple.hbm.xml", typeof (Simple).Assembly);
			cfg.AddSqlFunction("count", new ClassicCountFunction());

			ISessionFactory sf = cfg.BuildSessionFactory();

			using (ISession s = sf.OpenSession())
			{
				object count = s.CreateQuery("select count(*) from Simple").UniqueResult();
				Assert.IsTrue(count is Int32);
			}
			sf.Close();
		}
	}
}