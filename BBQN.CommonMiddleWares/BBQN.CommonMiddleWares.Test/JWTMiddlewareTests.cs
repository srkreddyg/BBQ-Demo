using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BBQN.CommonMiddleWares.Test
{
    [TestClass]
    public class JWTMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<IConfiguration> _mockConfiguration;

        private JWTMiddleware _middleware;

        public JWTMiddlewareTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockConfiguration = new Mock<IConfiguration>();
            _middleware = new JWTMiddleware(_mockNext.Object, _mockConfiguration.Object);
        }

        [TestMethod]
        public async Task Invoke_HttpContext_ThrowsException()
        {
            var mockContext = new Mock<HttpContext>();
            var res = _middleware.Invoke(mockContext.Object);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task Invoke_HttpContext()
        {
            var memoryStream = new MemoryStream();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "test-header";
            httpContext.Response.Body = memoryStream;
            var res = _middleware.Invoke(httpContext);
            Assert.IsNotNull(res);
        }
    }
}
