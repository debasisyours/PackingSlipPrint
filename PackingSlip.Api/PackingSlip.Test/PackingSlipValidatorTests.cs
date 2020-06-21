using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PackingSlip.Domain.Entities;
using PackingSlip.Repository;
using PackingSlip.Service;
using PackingSlip.Utility;
using System.Collections.Generic;

namespace PackingSlip.Test
{
    [TestClass]
    public class PackingSlipValidatorTests
    {
        private IPackingSlipValidator _packingSlipValidator = null;
        private Mock<ICustomerMembershipRepository> _mockCustomerMembershipRepository = null;

        [TestInitialize]
        public void InitializeTests()
        {
            _mockCustomerMembershipRepository = new Mock<ICustomerMembershipRepository>();
            _packingSlipValidator = new PackingSlipValidator(_mockCustomerMembershipRepository.Object);
        }

        [TestMethod]
        public void When_I_Call_IsPackingSlipRequestValid_With_Valid_PackingSlip_I_Should_Get_True()
        {
            // Arrange
            string customerEmail = "test@test.com";
            var createMembershipCheckResponse = new ResponseMessage { IsSuccess = false };
            var membership = new CustomerMembership { Email = customerEmail, IsActivated = false, IsUpgraded = false };
            var packingSlip = new PackingSlipHeader
            {
                PackingSlipItems = new List<PackingSlipItem>
                {
                    new PackingSlipItem
                    {
                        Name="Test Item",
                        Quantity=2
                    }
                },
                AgentName = It.IsAny<string>(),
                CustomerEmail = customerEmail,
                CustomerName = "Test Customer",
                PackingSlipNumber = "PS/001"
            };

            // Act
            _mockCustomerMembershipRepository.Setup(x => x.IsMembershipValidForCreate(customerEmail)).Returns(createMembershipCheckResponse);
            var valid = _packingSlipValidator.IsPackingSlipRequestValid(packingSlip);

            // Assert
            Assert.IsTrue(valid.IsSuccess);
        }

        [TestMethod]
        public void When_I_Call_IsPackingSlipRequestValid_With_Empty_CustomerEmail_I_Should_Get_False()
        {
            // Arrange
            string customerEmail = string.Empty;
            var packingSlip = new PackingSlipHeader
            {
                PackingSlipItems = new List<PackingSlipItem>
                {
                    new PackingSlipItem
                    {
                        Name="Test Item",
                        Quantity=2
                    }
                },
                AgentName = It.IsAny<string>(),
                CustomerEmail = customerEmail,
                CustomerName = "Test Customer",
                PackingSlipNumber = "PS/001"
            };

            // Act
            var valid = _packingSlipValidator.IsPackingSlipRequestValid(packingSlip);

            // Assert
            Assert.AreEqual(valid.Detail, PackingSlipConstants.CustomerEmailCannotBeBlankMessage);
        }

        [TestMethod]
        public void When_I_Call_IsPackingSlipRequestValid_With_Empty_Agent_With_PhysicalProduct_I_Should_Get_False()
        {
            // Arrange
            string customerEmail = "test@test.com";
            var membershipResponse = new ResponseMessage { IsSuccess = false };
            var packingSlip = new PackingSlipHeader
            {
                PackingSlipItems = new List<PackingSlipItem>
                {
                    new PackingSlipItem
                    {
                        Name="Test Item",
                        Quantity=2
                    }
                },
                AgentName = string.Empty,
                HasPhysicalProduct=true,
                CustomerEmail = customerEmail,
                CustomerName = "Test Customer",
                PackingSlipNumber = "PS/001"
            };

            // Act
            _mockCustomerMembershipRepository.Setup(x => x.IsMembershipValidForCreate(customerEmail)).Returns(membershipResponse);
            var valid = _packingSlipValidator.IsPackingSlipRequestValid(packingSlip);

            // Assert
            Assert.AreEqual(valid.Detail, PackingSlipConstants.AgentCannotBeBlankMessage);
        }
    }
}
