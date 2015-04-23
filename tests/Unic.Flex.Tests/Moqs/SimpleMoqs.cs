namespace Unic.Flex.Tests.Moqs
{
    using System;
    using System.Linq.Expressions;
    using Moq;
    using Unic.Configuration;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Model.Configuration;

    /// <summary>
    /// Class to retrieve simple moq classes.
    /// </summary>
    public static class SimpleMoqs
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns>A logger moq</returns>
        public static ILogger GetLogger()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(method => method.Info(It.IsAny<string>(), It.IsAny<object>()));
            mock.Setup(method => method.Warn(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Exception>()));
            mock.Setup(method => method.Error(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Exception>()));
            return mock.Object;
        }

        /// <summary>
        /// Gets the dictionary repository.
        /// </summary>
        /// <returns>A dictionary repository moq</returns>
        public static IDictionaryRepository GetDictionaryRepository()
        {
            var mock = new Mock<IDictionaryRepository>();
            mock.Setup(method => method.GetText(It.IsAny<string>())).Returns((string key) => key);
            return mock.Object;
        }

        /// <summary>
        /// Gets the configuration manager.
        /// </summary>
        /// <returns>A configuration manager moq</returns>
        public static IConfigurationManager GetConfigurationManager()
        {
            var mock = new Mock<IConfigurationManager>();
            mock.Setup(method => method.Get(It.IsAny<Expression<Func<GlobalConfiguration, string>>>())).Returns("(optional)");
            return mock.Object;
        }
    }
}
