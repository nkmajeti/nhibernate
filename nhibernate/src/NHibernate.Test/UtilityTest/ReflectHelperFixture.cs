using System;
using System.Reflection;
using NHibernate.DomainModel;
using NHibernate.Util;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.Test.UtilityTest
{
	/// <summary>
	/// Summary description for ReflectHelperFixture.
	/// </summary>
	[TestFixture]
	public class ReflectHelperFixture
	{
		[Test]
		public void GetConstantValueEnum()
		{
			object result = ReflectHelper.GetConstantValue(typeof(FooStatus), "ON");
			Assert.AreEqual(1, (int) result, "Should have found value of 1");
		}

		[Test]
		public void OverridesEquals()
		{
			Assert.IsFalse(ReflectHelper.OverridesEquals(this.GetType()), "ReflectHelperFixture does not override equals");
			Assert.IsTrue(ReflectHelper.OverridesEquals(typeof(string)), "String does override equals");
			Assert.IsFalse(ReflectHelper.OverridesEquals(typeof(IDisposable)), "IDisposable does not override equals");
			Assert.IsTrue(ReflectHelper.OverridesEquals(typeof(BRhf)), "Base class overrides equals");
		}

		[Test]
		public void NoTypeFoundReturnsNull()
		{
			System.Type noType = ReflectHelper.TypeFromAssembly("noclass", "noassembly", false);
			Assert.IsNull(noType);
		}

		[Test]
		public void TypeFoundInNotLoadedAssembly()
		{
			System.Type httpRequest = ReflectHelper.TypeFromAssembly("System.Web.HttpRequest", "System.Web", false);
			Assert.IsNotNull(httpRequest);

			System.Type sameType = ReflectHelper.TypeFromAssembly("System.Web.HttpRequest", "System.Web", false);
			Assert.AreEqual(httpRequest, sameType, "should be the exact same Type");
		}

		[Test]
		public void SystemTypes()
		{
			System.Type int32 = ReflectHelper.ClassForName("System.Int32");
			Assert.AreEqual(typeof(Int32), int32);
		}

		[Test]
		public void TryGetMethod()
		{
			const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			MethodInfo mig = typeof (MyBaseImplementation).GetMethod("get_Id", bf);
			MethodInfo mis = typeof(MyBaseImplementation).GetMethod("set_Id", bf);
			MethodInfo mng = typeof(MyBaseImplementation).GetMethod("get_Name", bf);
			MethodInfo mns = typeof(MyBaseImplementation).GetMethod("set_Name", bf);
			Assert.That(ReflectHelper.TryGetMethod(typeof(IMyBaseInterface), mig), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof(IMyBaseInterface), mis), Is.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof(IMyBaseInterface), mng), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof(IMyBaseInterface), mns), Is.Not.Null);

			mig = typeof(MyImplementation).GetMethod("get_Id", bf);
			mis = typeof(MyImplementation).GetMethod("set_Id", bf);
			mng = typeof(MyImplementation).GetMethod("get_Name", bf);
			mns = typeof(MyImplementation).GetMethod("set_Name", bf);
			MethodInfo mdg = typeof(MyImplementation).GetMethod("get_Description", bf);
			MethodInfo mds = typeof(MyImplementation).GetMethod("set_Description", bf);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mig), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mis), Is.Null);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mng), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mns), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mdg), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(new[] { typeof(IMyBaseInterface), typeof(IMyInterface) }, mds), Is.Null);
		}
	}

	public class ARhf
	{
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class BRhf : ARhf
	{
	}

	public interface IMyBaseInterface
	{
		int Id { get; }
		string Name { get; set; }
	}

	public interface IMyInterface: IMyBaseInterface
	{
		string Description { get; }
	}

	public class MyBaseImplementation : IMyBaseInterface
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class MyImplementation: IMyInterface
	{
		public int Id{ get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}

}