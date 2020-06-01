using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IProjectManager
    {
        List<Project> GetProjectsByComplete(bool complete = false);

        Project GetProjectByID(int projectID);

        bool AddProject(Project project);

        bool EditProject(Project oldProject, Project newProject);

        bool SetProjectCompleteStatus(bool complete, int projectID);
    }
}
