using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;

namespace Business.Services;

public interface IStatusService
{
    Task<StatusResult<Status>> GetStatusByIdAsync(int id);
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

        return new StatusResult<IEnumerable<Status>> { Succeeded = true, StatusCode = 200, Result = statuses };
    }

    public async Task<StatusResult<Status>> GetStatusByIdAsync(int id)
    {
        var repositoryResult = await _statusRepository.GetAsync(x => x.Id == id);

        var status = repositoryResult.Result;
        if (status == null)
            return new StatusResult<Status> { Succeeded = false, StatusCode = 404, Error = $"Status with id '{id}' was not found." };

        return new StatusResult<Status> { Succeeded = true, StatusCode = 200, Result = status };
    }
}
