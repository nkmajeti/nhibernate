using NHibernate.Type;

namespace NHibernate.Mapping
{
	/// <summary>
	/// A Set with no nullable element columns will have a primary
	/// key consisting of all table columns (ie - key columns + 
	/// element columns).
	/// </summary>
	public class Set : Collection
	{
		public Set( PersistentClass owner ) : base( owner )
		{
		}

		public override bool IsSet
		{
			get { return true; }
		}

		public override CollectionType DefaultCollectionType
		{
			get
			{
#if NET_2_0
				if( this.IsGeneric )
				{
					if( IsSorted )
					{
						return TypeFactory.GenericSortedSet( Role, ReferencedPropertyName, Comparer, this.GenericArguments[ 0 ] );
					}
					else if( HasOrder )
					{
						throw new MappingException( "Cannot use order-by with generic set, no appropriate collection implementation is available" );
					}
					else
					{
						return TypeFactory.GenericSet( Role, ReferencedPropertyName, this.GenericArguments[ 0 ] );
					}
				}
#endif
				// Non-generic
				if( IsSorted )
				{
					return TypeFactory.SortedSet( Role, ReferencedPropertyName, ( System.Collections.IComparer ) Comparer );
				}
				else if( HasOrder )
				{
					return TypeFactory.OrderedSet( Role, ReferencedPropertyName );
				}
				else
				{
					return TypeFactory.Set( Role, ReferencedPropertyName );
				}
			}
		}

		public override void CreatePrimaryKey()
		{
			if ( !IsOneToMany )
			{
				PrimaryKey pk = new PrimaryKey();
				foreach( Column col in Key.ColumnCollection )
				{
					pk.AddColumn( col );
				}

				bool nullable = false;
				foreach( Column col in Element.ColumnCollection )
				{
					if( col.IsNullable )
					{
						nullable = true;
					}
					pk.AddColumn( col );
				}

				// some databases (Postgres) will tolerate nullable
				// column in a primary key - others (DB2) won't
				if( !nullable )
				{
					CollectionTable.PrimaryKey = pk;
				}
			}
			else
			{
				// Create an index on the key columns?
			}
		}
	}
}