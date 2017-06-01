using Link.CrossCutting;
using Link.Domain.Contracts;
using Moq;
using Should;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Link.Business.UnitTests
{
    public class PackageValidatorTests : TestsFor<PackageValidator>
    {
        [Fact]
        public void IsValidId_PackageIdIsNull_ReturnsFalse()
        {
            // Arrange
            string nullPackageId = null;

            // Act
            var result = Instance.IsValidId(nullPackageId);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValidId_PackageIdIsTooShort_ReturnsFalse()
        {
            // Arrange
            string shortString = new string('1', 9);

            // Act
            var result = Instance.IsValidId(shortString);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValidId_PackageIdIsTooLong_ReturnsFalse()
        {
            // Arrange
            string longString = new string('1', 17);

            // Act
            var result = Instance.IsValidId(longString);

            // Assert
            result.ShouldBeFalse();
        }


        [Fact]
        public void IsValidId_PackageIdIsJustRightLength_ReturnsTrue()
        {
            // Arrange
            string packageId1 = new string('1', 12);
            string packageId2 = new string('1', 16);

            // Act
            var result1 = Instance.IsValidId(packageId1);
            var result2 = Instance.IsValidId(packageId1);

            // Assert
            result1.ShouldBeTrue("12-character ID failed");
            result2.ShouldBeTrue("16-character ID failed");
        }
    }
}
