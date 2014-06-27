using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Tests.Common.Moqs
{
    using Moq;
    using Unic.Flex.Caching;
    using Unic.Flex.Logging;

    public static class SimpleMoqs
    {
        public static ILogger GetLogger()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(method => method.Info(It.IsAny<string>(), It.IsAny<object>()));
            mock.Setup(method => method.Error(It.IsAny<string>(), It.IsAny<object>()));
            return mock.Object;
        }

        public static ICacheRepository GetCacheRepository<T>() where T : class
        {
            var mock = new Mock<ICacheRepository>();
            mock.Setup(method => method.Get<T>(It.IsAny<string>())).Returns(() => null);
            mock.Setup(method => method.Add(It.IsAny<string>(), It.IsAny<object>()));
            return mock.Object;
        }
    }
}
