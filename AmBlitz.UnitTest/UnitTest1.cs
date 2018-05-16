using AmBlitz.Cache;
using AmBlitz.Configuration;
using AmBlitz.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Autofac;

namespace AmBlitz.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public static Bootstrapper Bootstrapper { get; } = Bootstrapper.Create<AmBlitzTestModule>();

        [TestInitialize]
        public void Init()
        {
            var masterDbcon = ConfigurationManager.AppSettings["MongodbHostPrimary"] ?? "";
            //配置手脚架
            AmBlitzGlobalConfiguration.Configuration(settings =>
            {
                settings
                .MasterDataBases("EventBigData", masterDbcon)
                .UseRedisDataBase("ebd.redis.31huiyi.com:6379");
            });
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
            var data = new MyClass { MyProperty = 1, MyProperty1 = new MyClass1 { MyProperty = "xiaom" } };
            cache.Set("666", data);
            var getData = cache.Get<MyClass>("666");
            Assert.AreEqual(getData.MyProperty1.MyProperty, "xiaom");
        }

        
        public class MyClass
        {
            public int MyProperty { get; set; }
            public MyClass1 MyProperty1 { get; set; }
        }
        public class MyClass1
        {
            public string MyProperty { get; set; }
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
