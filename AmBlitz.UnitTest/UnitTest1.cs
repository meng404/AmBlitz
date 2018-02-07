using AmBlitz.Cache;
using AmBlitz.Domain;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmBlitz.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public static Bootstrapper Bootstrapper { get; } = Bootstrapper.Create<AmBlitzTestModule>();

        [TestInitialize]
        public void Init()
        {
            Bootstrapper.Initialize();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var cache = Bootstrapper.IocManager.Container.Resolve<ICache>();
            cache.Set("111", "魏安蒙");
            var xx =  cache.Get<string>("111");
            Assert.AreEqual("魏安蒙", xx);

            cache.Set("222", 100);
            var ss = cache.Get<int>("222");
            Assert.AreEqual(100, ss);

            var x = cache.Increment("j", 1);
            Assert.AreEqual(x, 1);
            x = cache.Decrement("j", 1);
            Assert.AreEqual(x, 0);

        }


        [TestMethod]
        public void TestMethod2()
        {

            var studentRepository = Bootstrapper.IocManager.Container.Resolve<IRepository<Student>>();
            //studentRepository.Insert(new Student { Name = "小明" });
            var student = studentRepository.FirstOrDefault(m => m.Name == "小明");
            Assert.AreNotEqual(student, null);
        }
    }
}
