﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pokedex.Logging.Interfaces;
using PokedexApp.Controllers;
using PokedexApp.Interfaces;
using PokedexApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokedex.Tests.Controllers
{
    [TestClass]
    public class PokedexMVControllerFixture
    {
        private PokedexController _pokedexController;

        private Mock<IPokedexAppLogic> _pokedexAppLogicMock;
        private Mock<ILoggerAdapter<PokedexController>> _loggerMock;

        [TestInitialize]
        public void Initialize()
        {
            _pokedexAppLogicMock = new Mock<IPokedexAppLogic>();
            _pokedexAppLogicMock.Setup(plm => plm.GetMyPokedex()).ReturnsAsync(It.IsAny<List<PokemonListingViewModel>>());
            _pokedexAppLogicMock.Setup(plm => plm.GetMyPokemonById(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<PokemonDetailViewModel>());

            _loggerMock = new Mock<ILoggerAdapter<PokedexController>>();

            _pokedexController = new PokedexController(_pokedexAppLogicMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task IndexActionIsSuccessfulAndCallsLogic()
        {
            await _pokedexController.Index();

            _pokedexAppLogicMock.Verify(plm => plm.GetMyPokedex(), Times.Once);
        }

        [TestMethod]
        public async Task DetailActionIsSuccessfulAndCallsLogic()
        {
            await _pokedexController.Detail(DataGenerator.DefaultGuid);

            _pokedexAppLogicMock.Verify(plm => plm.GetMyPokemonById(DataGenerator.DefaultGuid), Times.Once);
        }

        [TestMethod]
        public async Task DeleteActionIsSuccessfulAndCallsLogic()
        {
            await _pokedexController.Delete(DataGenerator.DefaultGuid);

            _pokedexAppLogicMock.Verify(plm => plm.DeletePokemonById(DataGenerator.DefaultGuid), Times.Once);
        }

        
        [TestMethod]
        public async Task EditIsSuccessfulAndCallsLogic()
        {
            await _pokedexController.Edit(DataGenerator.DefaultGuid);

            _pokedexAppLogicMock.Verify(plm => plm.GetMyPokemonById(DataGenerator.DefaultGuid), Times.Once);

            await _pokedexController.Edit(new PokemonDetailViewModel());

            _pokedexAppLogicMock.Verify(plm => plm.EditPokemon(It.IsAny<PokemonDetailViewModel>()), Times.Once);
        }

        [TestMethod]
        public void ErrorActionLogsError()
        {
            Exception error = new Exception("some error");

            _pokedexController.Error(error);

            _loggerMock.Verify(lm => lm.LogError(error, "some error"), Times.Once);
        }
    }
}