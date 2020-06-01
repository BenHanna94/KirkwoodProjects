using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IProjectAccessor
    {
        List<Project> GetProjectsByComplete(bool complete = false);

        Project GetProjectByID(int projectID);

        int InsertProject(Project project);

        int UpdateProject(Project oldProject, Project newProject);

        int CompleteProject(int projectID);

        int DecompleteProject(int projectID);
    }
}
