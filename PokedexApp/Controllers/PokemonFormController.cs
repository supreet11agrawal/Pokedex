﻿using Microsoft.AspNetCore.Mvc;
using Pokedex.Common;
using Pokedex.Logging.Interfaces;
using PokedexApp.Interfaces;
using PokedexApp.Models;
using System;
using System.Threading.Tasks;

namespace PokedexApp.Controllers
{
    /// <summary>
    /// The Pokémon Form Controller.
    /// </summary>
    public class PokemonFormController : Controller
    {
        private readonly ILoggerAdapter<PokemonFormController> _logger;
        private  readonly IPokedexAppLogic _pokedexAppLogic;
        public PokemonFormController(ILoggerAdapter<PokemonFormController> logger, IPokedexAppLogic pokedexAppLogic)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pokedexAppLogic = pokedexAppLogic ?? throw new ArgumentNullException(nameof(pokedexAppLogic));
        }

        /// <summary>
        /// The form for adding a Pokémon to personal Pokédex.
        /// </summary>
        /// <returns>The form.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var pokemonFormViewModel = await _pokedexAppLogic.GetNewPokemonForm();

                return View(pokemonFormViewModel);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// The add action for given form data.
        /// </summary>
        /// <param name="pokemonFormViewModel">The form view model.</param>
        /// <returns>The success result.</returns>
        [HttpPost]
        public async Task<IActionResult> Index(PokemonFormViewModel pokemonFormViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _pokedexAppLogic.AddPokemon(pokemonFormViewModel);

                    return View(Constants.Success, new SuccessViewModel { ActionName = "addition" });
                }

                _logger.LogInformation(Constants.InvalidRequest);

                return await Index();
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Capture the Pokémon from the given id.
        /// </summary>
        /// <param name="id">The NationalDex Id.</param>
        /// <returns>The pre-filled form.</returns>
        public async Task<IActionResult> Capture(int id)
        {
            try
            {
                var pokemonFormViewModel = await _pokedexAppLogic.GetNewPokemonForm();

                pokemonFormViewModel.SelectedNationalDexPokemonId = id;

                return View("Index", pokemonFormViewModel);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// The generic error page.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The error result.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return View(Constants.Error, new ErrorViewModel { Message = ex.Message });
        }
    }
}