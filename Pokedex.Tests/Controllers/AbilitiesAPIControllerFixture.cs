﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pokedex.Data.Models;
using Pokedex.Logging.Interfaces;
using Pokedex.Repository.Interfaces;
using PokedexAPI.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokedex.Tests.Controllers
{
    [TestClass]
    public class AbilitiesAPIControllerFixture
    {
        private Mock<IPokedexRepository> _pokedexRepositoryMock;
        private Mock<ILoggerAdapter<AbilitiesController>> _loggerMock;

        private AbilitiesController _abilitiesController;

        [TestInitialize]
        public void Intitialize()
        {
            _pokedexRepositoryMock = new Mock<IPokedexRepository>();
            _pokedexRepositoryMock.Setup(prm => prm.GetAllAbilities()).ReturnsAsync(It.IsAny<List<tlkpAbility>>());
            _pokedexRepositoryMock.Setup(prm => prm.GetAbilityById(1)).ReturnsAsync(DataGenerator.GenerateAbilities(1)[0]);

            _loggerMock = new Mock<ILoggerAdapter<AbilitiesController>>();

            _abilitiesController = new AbilitiesController(_pokedexRepositoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAbilitiesIsSuccessfulAndCallsRepository()
        {
            await _abilitiesController.GetAbilities();

            _pokedexRepositoryMock.Verify(prm => prm.GetAllAbilities(), Times.Once);
        }

        [TestMethod]
        public async Task GetAbilityByIdIsSuccessfulAndCallsRepository()
        {
            await _abilitiesController.GetAbilityById(1);

            _pokedexRepositoryMock.Verify(prm => prm.GetAbilityById(1), Times.Once);

            _loggerMock.VerifyNoOtherCalls();
        }


        [TestMethod]
        public async Task GetAbilityByInvalidIdCallsRepositoryAndLogsInformation()
        {
            await _abilitiesController.GetAbilityById(2);

            _pokedexRepositoryMock.Verify(prm => prm.GetAbilityById(2), Times.Once);

            _loggerMock.Verify(lm => lm.LogInformation("No ability with id: 2"), Times.Once);
        }
    }
}