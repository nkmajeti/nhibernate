using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Util;

namespace NHibernate.Cfg.XmlHbmBinding
{
	public class NamedQueryBinder : Binder
	{
		public NamedQueryBinder(Mappings mappings)
			: base(mappings)
		{
		}

		public NamedQueryBinder(Binder parent)
			: base(parent)
		{
		}

		public void AddQuery(HbmQuery querySchema)
		{
			string queryName = querySchema.name;
			string queryText = querySchema.GetText();

			log.DebugFormat("Named query: {0} -> {1}", queryName, queryText);

			bool cacheable = querySchema.cacheable;
			string region = querySchema.cacheregion;
			int timeout = string.IsNullOrEmpty(querySchema.timeout) ? RowSelection.NoValue : int.Parse(querySchema.timeout);
			int fetchSize = querySchema.fetchsizeSpecified ? querySchema.fetchsize : -1;
			bool readOnly = querySchema.readonlySpecified ? querySchema.@readonly : false;
			string comment = querySchema.comment;

			FlushMode flushMode = FlushModeConverter.GetFlushMode(querySchema);
			CacheMode? cacheMode = (querySchema.cachemodeSpecified)
										? CacheModeConverter.GetCacheMode(querySchema.cachemode)
										: null;



			IDictionary<string,string> parameterTypes = new LinkedHashMap<string,string>();

			NamedQueryDefinition namedQuery = new NamedQueryDefinition(queryText, cacheable, region, timeout,
				fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes);

			mappings.AddQuery(queryName, namedQuery);
		}
	}
}