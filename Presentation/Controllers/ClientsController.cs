using Business.Models;
using Business.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class ClientsController(IClientService clientService) : Controller
{

    private readonly IClientService _clientService = clientService;

    [Route("admin/clients")]
    public async Task<IActionResult> Index()
    {
        ViewBag.ErrorMessage = "";

        var clientResult = await _clientService.GetClientsAsync();
        var model = new ClientsViewModel
        {
            Clients = clientResult.Result!
        };

        return View(model);
    }

    [HttpPost]
    [Route("admin/clients")]
    public async Task<IActionResult> Add(AddClientViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
        }
        var clientFormData = model.MapTo<AddClientFormData>();

        var result = await _clientService.CreateClientAsync(clientFormData);
        return result.StatusCode switch
        {
            200 => Ok(),
            400 => BadRequest(result.Error),
            409 => Conflict(),
            _ => Problem(),
        };


    }

}
