using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services
{
         public static class TestExtensions
        {
            public static T ShouldNotNull<T>(this T obj)
            {
                Assert.IsNull(obj);
                return obj;
            }

            public static T ShouldNotNull<T>(this T obj, string message)
            {
                Assert.IsNull(obj, message);
                return obj;
            }

            public static T ShouldNotBeNull<T>(this T obj)
            {
                Assert.IsNotNull(obj);
                return obj;
            }

            public static T ShouldNotBeNull<T>(this T obj, string message)
            {
                Assert.IsNotNull(obj, message);
                return obj;
            }

            public static T ShouldEqual<T>(this T actual, object expected)
            {
                Assert.AreEqual(expected, actual);
                return actual;
            }

            ///<summary>
            /// Asserts that two objects are equal.
            ///</summary>
            ///<param name="actual"></param>
            ///<param name="expected"></param>
            ///<param name="message"></param>
            ///<exception cref="AssertionException"></exception>
            public static void ShouldEqual(this object actual, object expected)
            {
                Assert.AreEqual(expected, actual);
            }

            public static void ShouldNotEqual(this object actual, object expected)
            {
                Assert.AreNotEqual(expected, actual);
            }

            public static T ShouldContains<T>(this T actual, object expected)
            {
                Assert.True(actual.ToString().Contains(expected.ToString()));
                return actual;
            }

            public static Exception ShouldBeThrownBy(this Type exceptionType, TestDelegate testDelegate)
            {
                return Assert.Throws(exceptionType, testDelegate);
            }

            public static void ShouldBe<T>(this object actual)
            {
                Assert.IsInstanceOf<T>(actual);
            }

            public static void ShouldBeNull(this object actual)
            {
                Assert.IsNull(actual);
            }

            public static void ShouldBeTheSameAs(this object actual, object expected)
            {
                Assert.AreSame(expected, actual);
            }

            public static void ShouldBeNotBeTheSameAs(this object actual, object expected)
            {
                Assert.AreNotSame(expected, actual);
            }

            public static T CastTo<T>(this object source)
            {
                return (T)source;
            }

            public static void ShouldBeTrue(this bool source)
            {
                Assert.IsTrue(source);
            }

            public static void ShouldBeFalse(this bool source)
            {
                Assert.IsFalse(source);
            }

             public static void ShouldBeEmpty<T>(this IEnumerable<T> list)
             {
                 Assert.IsEmpty(list);
             }

             public static void ShouldNotBeEmpty<T>(this IEnumerable<T> list)
             {
                 Assert.IsNotEmpty(list);
             }

            /// <summary>
            /// Compares the two strings (case-insensitive).
            /// </summary>
            /// <param name="actual"></param>
            /// <param name="expected"></param>
            public static void AssertSameStringAs(this string actual, string expected)
            {
                if (!string.Equals(actual, expected, StringComparison.InvariantCultureIgnoreCase))
                {
                    var message = string.Format("Expected {0} but was {1}", expected, actual);
                    throw new AssertionException(message);
                }
            }
        }

    
    
}
