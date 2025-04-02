using Business.Models;
using Data.Entitites;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<bool> ProjectExists(string id);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid form data provided." };

        var projectEntity = formData.MapTo<ProjectEntity>();
        // TODO kolla om client, user och statusid finns

        // TODO fixa statusid från formdata
        //var statusResult = await _statusService.GetStatusByIdAsync(1);
        //var status = statusResult.Result;
        //projectEntity.StatusId = status!.Id;
        var result = await _projectRepository.AddAsync(projectEntity);

        return result.Succeeded ?
            new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync(
            orderByDescending: true, sortByColumn: s => s.Created, filterBy: null, take: 0,
            include => include.User,
            include => include.Client,
            include => include.Status
            );

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync(findBy: x => x.Id == id,
            include => include.User,
            include => include.Client,
            include => include.Status);


        return response.Succeeded ?
            new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
           : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project with id {id} was not found. " };
    }


    public async Task<bool> ProjectExists(string id)
    {
        var response = await _projectRepository.GetAsync(findBy: x => x.Id == id);
        return response.Succeeded;
    }


    // TODO update project
    public async Task<ProjectResult> UpdateProjectAsync(UpdateProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid form data provided." };

        var updatedProjectEntity = formData.MapTo<ProjectEntity>();
        // TODO kolla om client, user och statusid finns


        var result = await _projectRepository.UpdateAsync(updatedProjectEntity);

        return result.Succeeded ?
            new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

// TODO delete project
    public async Task<ProjectResult> DeleteProjectByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid id provided." };

        var result = await _projectRepository.DeleteAsync(x => x.Id == id);

        return result.Succeeded ?
            new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    
}
