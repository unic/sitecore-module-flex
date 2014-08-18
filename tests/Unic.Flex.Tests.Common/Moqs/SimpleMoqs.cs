using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Tests.Common.Moqs
{
    using Moq;
    using Unic.Flex.Logging;

    public static class SimpleMoqs
    {
        public static ILogger GetLogger()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(method => method.Info(It.IsAny<string>(), It.IsAny<object>()));
            mock.Setup(method => method.Warn(It.IsAny<string>(), It.IsAny<object>()));
            mock.Setup(method => method.Error(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Exception>()));
            return mock.Object;
        }
    }
}
