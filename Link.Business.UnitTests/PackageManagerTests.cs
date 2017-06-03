using Link.CrossCutting;
using Link.Domain.Contracts;
using Link.Domain.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Link.Business.UnitTests
{
    public class PackageManagerTests : TestsFor<PackageManager>
    {
        private Address _validAddress;
        private ExceptionHandler _exceptionHandler;
        private string _packageId;


        public override void OverrideMocks()
        {
            var mockedLogger = GetMockFor<ILogger>().Object;
            _exceptionHandler = new ExceptionHandler(mockedLogger);

            // We need a "real" implementation of ExceptionHandler in order to do verification and setups on the repository that it protects
            Inject<IExceptionHandler>(_exceptionHandler);
        }


        [Fact]
        public async Task SetDeliveryAddressAsync_PackageIdIsInvalid_DoesNotInvokeRepository()
        {
            // Act
            var result = await Instance.SetDeliveryAddressAsync(PackageId, ValidAddress);

            // Assert
            GetMockFor<IPackageRepository>().Verify(repo => repo.SetNewDeliveryAddressAsync(It.IsAny<string>(), It.IsAny<Address>()), Times.Never());
        }


        [Fact]
        public async Task SetDeliveryAddressAsync_AddressIsInValid_DoesNotInvokeRepository()
        {
            // Arrange
            GetMockFor<IPackageValidator>().Setup(v => v.IsValidId(PackageId)).Returns(true);
            
            // Act
            await Instance.SetDeliveryAddressAsync(PackageId, ValidAddress);

            // Assert
            GetMockFor<IPackageRepository>().Verify(repo => repo.SetNewDeliveryAddressAsync(It.IsAny<string>(), It.IsAny<Address>()), Times.Never());
        }


        [Fact]
        public async Task SetDeliveryAddressAsync_PackageIdAndAddressIsBothValid_InvokesRepository()
        {
            // Arrange
            GetMockFor<IPackageValidator>().Setup(v => v.IsValidId(PackageId)).Returns(true);
            GetMockFor<IAddressValidator>().Setup(v => v.IsValid(ValidAddress)).Returns(true);

            // Act
            await Instance.SetDeliveryAddressAsync(PackageId, ValidAddress);

            // Assert
            GetMockFor<IPackageRepository>().Verify(repo => repo.SetNewDeliveryAddressAsync(It.IsAny<string>(), It.IsAny<Address>()), Times.Once());
        }


        [Fact]
        public async Task SetDeliveryAddressAsync_RepositoryThrows_LoggerInvokedThroughExceptionHandler()
        {
            // Arrange
            GetMockFor<IPackageValidator>().Setup(v => v.IsValidId(PackageId)).Returns(true);
            GetMockFor<IAddressValidator>().Setup(v => v.IsValid(ValidAddress)).Returns(true);
            var badException = new Exception("I'm bad");
            GetMockFor<IPackageRepository>().Setup(o => o.SetNewDeliveryAddressAsync(PackageId, ValidAddress)).Throws(badException);

            // Act
            await Instance.SetDeliveryAddressAsync(PackageId, ValidAddress);

            // Assert
            GetMockFor<ILogger>().Verify(logger => logger.LogException(badException), Times.Once());
        }


        [Fact]
        public async Task SetDeliveryAddressAsync_AllParametersValid_LoggerDoesNotInvoke()
        {
            // Arrange
            GetMockFor<IPackageValidator>().Setup(v => v.IsValidId(PackageId)).Returns(true);
            GetMockFor<IAddressValidator>().Setup(v => v.IsValid(ValidAddress)).Returns(true);            
            GetMockFor<IPackageRepository>()
                .Setup(r => r.SetNewDeliveryAddressAsync(PackageId, ValidAddress))
                .Returns(Task.FromResult<Package>(null));

            // Act
            await Instance.SetDeliveryAddressAsync(PackageId, ValidAddress);

            // Assert
            GetMockFor<ILogger>().Verify(logger => logger.LogException(It.IsAny<Exception>()), Times.Never());
        }



        private string PackageId
        {
            get
            {
                if (_packageId == null)
                    _packageId = "123 456 789 012";

                return _packageId;
            }
        }


        private Address ValidAddress
        {
            get
            {
                if(_validAddress == null)                
                    _validAddress = new Address {
                        StreetAddress = "2050 Bamako Place",
                        ZipCode = "DC 20521-2050",
                        City = "Washington",
                        Country = "USA"
                    };

                return _validAddress;                
            }
        }
    }
}
