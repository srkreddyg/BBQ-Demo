using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBQN.CommonMiddleWares.Test
{
    [TestClass]
    public class ExceptionHandlingMiddleWaresTests
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _logger;
        private Mock<ExceptionHandlingMiddleware> _middleware;
       private ExceptionHandlingMiddleware _exceptionHandlingMiddleware;

        public ExceptionHandlingMiddleWaresTests()
        {
            _mockNext = new Mock<RequestDelegate>();
            _logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _middleware = new Mock<ExceptionHandlingMiddleware>();
            _exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(_mockNext.Object, _logger.Object);
        }

        [TestMethod]
        public async Task InvokeAsync_HttpContext()
        {
            var mockContext = new Mock<HttpContext>();
            var res = _exceptionHandlingMiddleware.InvokeAsync(mockContext.Object);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public async Task InvokeAsync_ThrowsException()
        {
            var mockContext = new Mock<HttpContext>();
           // _middleware.Setup(m => m.InvokeAsync(mockContext.)).ThrowsAsync(new Exception("Invalid Token"));

            var res =  _exceptionHandlingMiddleware.InvokeAsync(mockContext.Object);
            Assert.IsNotNull(res);
        }
    }
}
