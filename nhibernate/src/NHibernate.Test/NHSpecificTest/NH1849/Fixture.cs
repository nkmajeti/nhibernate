using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Engine.Query;
using NHibernate.Util;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1849
{
	public class CustomDialect : MsSql2005Dialect
	{
		public CustomDialect()
		{
			RegisterFunction("contains", new StandardSQLFunction("contains", NHibernateUtil.Boolean));
		}
	}

	[TestFixture]
	public class Fixture : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);

			configuration.SetProperty("dialect", "NHibernate.Test.NHSpecificTest.NH1849.CustomDialect, NHibernate.Test");
		}

		/// <summary>
      /// We don't actually execute the query, since it will throw an ado exception due to the absence of a full text index,
      /// however the query should compile
      /// </summary>
		[Test]
		public void ExecutesCustomSqlFunctionContains()
		{
         string hql = @"from Customer c where contains(c.Name, :smth)";

         HQLQueryPlan plan = new HQLQueryPlan(hql, false, new CollectionHelper.EmptyMapClass<string, IFilter>(), sessions);

         Assert.AreEqual(1, plan.ParameterMetadata.NamedParameterNames.Count);
         Assert.AreEqual(1, plan.QuerySpaces.Count);
         Assert.AreEqual(1, plan.SqlStrings.Length);
      }
	}
}