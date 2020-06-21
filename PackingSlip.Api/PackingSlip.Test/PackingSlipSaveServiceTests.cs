using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PackingSlip.Domain.Entities;
using PackingSlip.Repository;
using PackingSlip.Service;
using PackingSlip.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Test
{
    [TestClass]
    public class PackingSlipSaveServiceTests
    {
        private IPackingSlipSaveService _packingSlipSaveService = null;
        private Mock<IPackingSlipRepository> _mockPackingSlipRepository = null;
        private Mock<IPackingSlipValidator> _mockPackingSlipValidator = null;
        private Mock<IFreeItemCheckerService> _mockFreeItemCheckerService = null;
        private Mock<ICustomerMembershipRepository> _mockCustomerMembershipRepository = null;
        private Mock<IAgentCommissionRepository> _mockAgentCommissionRepository = null;
        private Mock<IEmailService> _mockEmailService = null;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockPackingSlipRepository = new Mock<IPackingSlipRepository>();
            _mockPackingSlipValidator = new Mock<IPackingSlipValidator>();
            _mockFreeItemCheckerService = new Mock<IFreeItemCheckerService>();
            _mockCustomerMembershipRepository = new Mock<ICustomerMembershipRepository>();
            _mockAgentCommissionRepository = new Mock<IAgentCommissionRepository>();
            _mockEmailService = new Mock<IEmailService>();
            var configOptions = Options.Create<FileConfig>(new FileConfig { DownloadPath = @"C:\ProgramData", Host = "smtp.gmail.com", Port = "587", UserCredential = "mailtestdebasis@gmail.com", Password = "Deb@2090" });
            
            _packingSlipSaveService = new PackingSlipSaveService(_mockPackingSlipRepository.Object,
                _mockPackingSlipValidator.Object,
                _mockFreeItemCheckerService.Object,
                _mockCustomerMembershipRepository.Object,
                _mockAgentCommissionRepository.Object,
                _mockEmailService.Object, configOptions);
        }

        [TestMethod]
        public void When_I_Call_SavePackingSlip_With_Invalid_Option_I_Should_Get_Failure_Response()
        {
            // Arrange
            var validatorResponse = new ResponseMessage { IsSuccess = false };
            var packingSlip = new PackingSlipHeader
            {
                CustomerEmail = string.Empty
            };

            // Act
            _mockFreeItemCheckerService.Setup(x => x.CheckForFreeItems(packingSlip)).ReturnsAsync(packingSlip);
            _mockPackingSlipValidator.Setup(x => x.IsPackingSlipRequestValid(packingSlip)).Returns(validatorResponse);
            var result = _packingSlipSaveService.SavePackingSlip(packingSlip);

            // Assert
            Assert.IsFalse(result.Result.IsSuccess);
        }

        [TestMethod]
        public void When_I_Call_SavePackingSlip_With_PhysicalProducts_AgentCommission_Should_Get_Called()
        {
            // Arrange
            var validatorResponse = new ResponseMessage { IsSuccess = true };
            var creationResponse = new ResponseMessage { IsSuccess = true, Detail = "PS-004" };
            var packingSlip = new PackingSlipHeader
            {
                CustomerEmail = string.Empty,
                HasPhysicalProduct = true
            };

            // Act
            _mockFreeItemCheckerService.Setup(x => x.CheckForFreeItems(packingSlip)).ReturnsAsync(packingSlip);
            _mockPackingSlipValidator.Setup(x => x.IsPackingSlipRequestValid(packingSlip)).Returns(validatorResponse);
            _mockPackingSlipRepository.Setup(x => x.SavePackingSlipAsync(packingSlip)).ReturnsAsync(creationResponse);
            var result = _packingSlipSaveService.SavePackingSlip(packingSlip);

            // Assert
            _mockAgentCommissionRepository.Verify(x => x.AddAgentCommission(packingSlip, 100), Times.Once);
        }

        [TestMethod]
        public void When_I_Call_SavePackingSlip_With_MembershipActivation_Flag_ActivateMembership_Should_Get_Called()
        {
            // Arrange
            string customerMail = "test@test.com";
            var validatorResponse = new ResponseMessage { IsSuccess = true };
            var creationResponse = new ResponseMessage { IsSuccess = true, Detail = "PS-004" };
            var packingSlip = new PackingSlipHeader
            {
                CustomerEmail = customerMail,
                HasPhysicalProduct = true,
                ActivateMembership=true
            };

            // Act
            _mockFreeItemCheckerService.Setup(x => x.CheckForFreeItems(packingSlip)).ReturnsAsync(packingSlip);
            _mockPackingSlipValidator.Setup(x => x.IsPackingSlipRequestValid(packingSlip)).Returns(validatorResponse);
            _mockPackingSlipRepository.Setup(x => x.SavePackingSlipAsync(packingSlip)).ReturnsAsync(creationResponse);
            var result = _packingSlipSaveService.SavePackingSlip(packingSlip);

            // Assert
            _mockCustomerMembershipRepository.Verify(x => x.ActivateMembershipAsync(customerMail), Times.Once);
        }

        [TestMethod]
        public void When_I_Call_SavePackingSlip_With_MembershipUpgrade_Flag_UpgradeMembership_Should_Get_Called()
        {
            // Arrange
            string customerMail = "test@test.com";
            var validatorResponse = new ResponseMessage { IsSuccess = true };
            var creationResponse = new ResponseMessage { IsSuccess = true, Detail = "PS-004" };
            var packingSlip = new PackingSlipHeader
            {
                CustomerEmail = customerMail,
                HasPhysicalProduct = true,
                UpgradeMembership = true
            };

            // Act
            _mockFreeItemCheckerService.Setup(x => x.CheckForFreeItems(packingSlip)).ReturnsAsync(packingSlip);
            _mockPackingSlipValidator.Setup(x => x.IsPackingSlipRequestValid(packingSlip)).Returns(validatorResponse);
            _mockPackingSlipRepository.Setup(x => x.SavePackingSlipAsync(packingSlip)).ReturnsAsync(creationResponse);
            var result = _packingSlipSaveService.SavePackingSlip(packingSlip);

            // Assert
            _mockCustomerMembershipRepository.Verify(x => x.UpgradeMembershipAsync(customerMail), Times.Once);
        }
    }
}
