using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PackingSlip.Domain.Entities;
using PackingSlip.Service;
using System.Collections.Generic;

namespace PackingSlip.Test
{
    [TestClass]
    public class PackingSlipValidatorTests
    {
        private IPackingSlipValidator _packingSlipValidator = null;

        [TestInitialize]
        public void InitializeTests()
        {
            _packingSlipValidator = new PackingSlipValidator();
        }

        [TestMethod]
        public void When_I_Call_IsPackingSlipRequestValid_With_Valid_PackingSlip_I_Should_Get_True()
        {
            // Arrange
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
                CustomerEmail = "test@test.com",
                CustomerName = "Test Customer",
                PackingSlipNumber = "PS/001"
            };

            // Act
            var valid = _packingSlipValidator.IsPackingSlipRequestValid(packingSlip);

            // Assert
            Assert.IsTrue(valid.IsSuccess);
        }
    }
}
