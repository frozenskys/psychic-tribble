namespace TribbleClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class TribbleClientTests
    {
        [Test]
		public void TestPact() 
		{
            var client = new TribbleClient.Client();
        }
    }
}
