using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace AjBase.Tests
{
    [TestClass]
    public class EngineTests
    {
        [TestMethod]
        public void CurrentEngine()
        {
            Engine.Current = new Engine();

            Assert.IsNotNull(Engine.Current);

            ManualResetEvent wait = new ManualResetEvent(false);

            Engine result = null;

            ThreadStart start = new ThreadStart(() => { result = Engine.Current; wait.Set(); });
            Thread thread = new Thread(start);
            thread.Start();

            wait.WaitOne();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUnknownDatabase()
        {
            Engine engine = new Engine();

            Assert.IsNull(engine.GetDatabase("MyEnterprise"));
        }
    }
}
