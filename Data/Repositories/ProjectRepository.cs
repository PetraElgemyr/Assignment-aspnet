using Data.Contexts;
using Data.Entitites;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    Task<ProjectResult<Project>> UpdateProjectFromModelAsync(Project projectModel);
}

public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{

    public async Task<ProjectResult<Project>> UpdateProjectFromModelAsync(Project projectModel)
    {
        try
        {
            var projectEntity = await _table.FirstOrDefaultAsync(x => x.Id == projectModel.Id);
            if (projectEntity == null)
                return new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = "Project entity was not found." };

            projectEntity.Image = projectModel.Image;
            projectEntity.ProjectName = projectModel.ProjectName;
            projectEntity.Description = projectModel.Description;
            projectEntity.Budget = projectModel.Budget;
            projectEntity.StartDate = projectModel.StartDate;
            projectEntity.EndDate = projectModel.EndDate;
            projectEntity.ClientId = projectModel.ClientId;
            projectEntity.StatusId = projectModel.StatusId;
            projectEntity.UserId = projectModel.UserId;

            _table.Update(projectEntity);
            await _context.SaveChangesAsync();
            var updatedModel = projectEntity.MapTo<Project>();

            return new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = updatedModel };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

}
