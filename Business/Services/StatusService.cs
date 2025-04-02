using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;

namespace Business.Services;

public interface IStatusService
{
    Task<StatusResult<Status>> GetStatusByIdAsync(int id);
    Task<StatusResult<Status>> GetStatusByNamedAsync(string statusName);
    Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync()
    {
        var repositoryResult = await _statusRepository.GetAllAsync
            (
                orderByDescending: false,
                sortByColumn: x => x.Id
            );
        var statuses = repositoryResult.Result;


        return repositoryResult.Succeeded ?
            new StatusResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Result = statuses }
        : new StatusResult<IEnumerable<Status>> { Succeeded = false, StatusCode = repositoryResult.StatusCode, Error = repositoryResult.Error };
    }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
    {
        var repositoryResult = await _statusRepository.GetAsync(x => x.Id == id);
        var status = repositoryResult.Result;

        return repositoryResult.Succeeded ?
            new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = status }
        : new StatusResult<Status> { Succeeded = false, StatusCode = repositoryResult.StatusCode, Error = repositoryResult.Error };
    }

    public async Task<StatusResult<Status>> GetStatusByNamedAsync(string statusName)
    {
        var repositoryResult = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        var status = repositoryResult.Result;

        return repositoryResult.Succeeded ?
               new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = status }
           : new StatusResult<Status> { Succeeded = false, StatusCode = repositoryResult.StatusCode, Error = repositoryResult.Error };
    }
}
