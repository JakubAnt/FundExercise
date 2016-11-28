using NUnit.Framework;
using System;

namespace Exercise.Test
{
    [TestFixture]
    public abstract class TestWithSubject<T> where T : class
    {
        public T Subject;

        [SetUp]
        public virtual void InitializeSubject()
        {
            Subject = CreateSubject();
        }

        public virtual T CreateSubject()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
