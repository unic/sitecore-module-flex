namespace Unic.Flex.Tests.Common.Moqs
{
    using Moq;
    using System;
    using Unic.Flex.Globalization;
    using Unic.Flex.Logging;

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
            mock.Setup(method => method.Warn(It.IsAny<string>(), It.IsAny<object>()));
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
    }
}
