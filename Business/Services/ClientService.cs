using Business.Models;
using Data.Entitites;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using System.Diagnostics;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult> CreateClientAsync(AddClientFormData formData);
    Task<ClientResult> DeleteClientByIdAsync(string id);
    Task<ClientResult<Client>> GetClientByIdAsync(string id);
    Task<ClientResult<IEnumerable<Client>>> GetClientsAsync();
    Task<ClientResult> UpdateClientAsync(UpdateClientFormData formData);
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ClientResult<IEnumerable<Client>>> GetClientsAsync()
    {
        var repositoryResult = await _clientRepository.GetAllAsync
            (
                orderByDescending: false,
                sortByColumn: x => x.ClientName
            );

        var clients = repositoryResult.Result;

        return new ClientResult<IEnumerable<Client>> { Succeeded = true, StatusCode = 200, Result = clients };
    }

    public async Task<ClientResult<Client>> GetClientByIdAsync(string id)
    {
        var repositoryResult = await _clientRepository.GetAsync(x => x.Id == id);

        var client = repositoryResult.Result;
        if (client == null)
            return new ClientResult<Client> { Succeeded = false, StatusCode = 404, Error = $"Client with id '{id}' was not found." };

        return new ClientResult<Client> { Succeeded = true, StatusCode = 200, Result = client };
    }

    public async Task<ClientResult> CreateClientAsync(AddClientFormData formData)
    {
        if(formData == null)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Client data is required." };

        try
        {
            var clientEntity = formData.MapTo<ClientEntity>();
            var result = await _clientRepository.AddAsync(clientEntity);

            return result.Succeeded ? new ClientResult { Succeeded = true, StatusCode = 200 } 
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error =result.Error};
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ClientResult> UpdateClientAsync(UpdateClientFormData formData)
    {
        if (formData == null)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Client data is required." };
        try
        {
            var updatedClientEntity = formData.MapTo<ClientEntity>();
            var result = await _clientRepository.UpdateAsync(updatedClientEntity);
            return result.Succeeded ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ClientResult> DeleteClientByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Client id is required." };
        try
        {
            var result = await _clientRepository.DeleteAsync(x => x.Id == id);
      
            return result.Succeeded ? new ClientResult { Succeeded = true, StatusCode = 200 }
        : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
       
    }
}
