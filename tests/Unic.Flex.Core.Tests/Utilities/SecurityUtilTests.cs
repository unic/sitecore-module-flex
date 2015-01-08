namespace Unic.Flex.Core.Tests.Utilities
{
    using System.Security.Cryptography;
    using NUnit.Framework;
    using Unic.Flex.Core.Utilities;

    /// <summary>
    /// Tests for the security util.
    /// </summary>
    public class SecurityUtilTests
    {
        /// <summary>
        /// Tests the method to get an md5 hash
        /// </summary>
        [TestFixture]
        public class TheGetMd5HashMethod
        {
            /// <summary>
            /// Tests if the method would correctly generate md5 hashes.
            /// </summary>
            [Test]
            public void WillGenerateCorrectMd5Hash()
            {
                // prepare
                const string Input = "simpletest";

                // act
                var hash = SecurityUtil.GetMd5Hash(MD5.Create(), Input);

                // Assert
                Assert.AreEqual("be60d38818fcb0444ff5c757acecc319", hash);
            }
        }

        /// <summary>
        /// Tests the method to verify md5 hash
        /// </summary>
        [TestFixture]
        public class TheVerifyMd5HashMethod
        {
            /// <summary>
            /// Tests if the hash will be correctly verified
            /// </summary>
            [Test]
            public void WillCorrectlyVerifyHash()
            {
                // prepare
                const string Input = "simpletest";
                const string Hash = "be60d38818fcb0444ff5c757acecc319";

                // act
                var result = SecurityUtil.VerifyMd5Hash(MD5.Create(), Input, Hash);

                // assert
                Assert.IsTrue(result);
            }
        }
    }
}
