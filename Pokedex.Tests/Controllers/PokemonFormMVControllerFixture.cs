﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pokedex.Logging.Interfaces;
using PokedexApp.Controllers;
using PokedexApp.Interfaces;
using PokedexApp.Models;
using System;
using System.Threading.Tasks;

namespace Pokedex.Tests.Controllers
{
    [TestClass]
    public class PokemonFormMVControllerFixture
    {
        private PokemonFormController _pokemonFormController;

        private Mock<IPokedexAppLogic> _pokedexAppLogicMock;
        private Mock<ILoggerAdapter<PokemonFormController>> _loggerMock;

        [TestInitialize]
        public void Initialize()
        {
            _pokedexAppLogicMock = new Mock<IPokedexAppLogic>();
            _pokedexAppLogicMock.Setup(plm => plm.GetNewPokemonForm()).ReturnsAsync(It.IsAny<PokemonFormViewModel>());

            _loggerMock = new Mock<ILoggerAdapter<PokemonFormController>>();

            _pokemonFormController = new PokemonFormController(_pokedexAppLogicMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task IndexActionIsSuccessfulAndCallsLogic()
        {
            await _pokemonFormController.Index();

            _pokedexAppLogicMock.Verify(plm => plm.GetNewPokemonForm(), Times.Once);
        }

        [TestMethod]
        public async Task IndexWithVMActionIsSuccessfulAndCallsLogic()
        {
            await _pokemonFormController.Index(new PokemonFormViewModel());

            _pokedexAppLogicMock.Verify(plm => plm.AddPokemon(It.IsAny<PokemonFormViewModel>()), Times.Once);
        }

        [TestMethod]
        public void ErrorActionLogsError()
        {
            Exception error = new Exception("some error");

            _pokemonFormController.Error(error);

            _loggerMock.Verify(lm => lm.LogError(error, "some error"), Times.Once);
        }
    }
}