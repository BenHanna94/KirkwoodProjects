using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class ProjectManager : IProjectManager
    {
        private IProjectAccessor _projectAccessor;

        public ProjectManager()
        {
            _projectAccessor = new ProjectAccessor();
        }

        public ProjectManager(ProjectAccessor projectAccessor)
        {
            _projectAccessor = projectAccessor;
        }

        

        public List<Project> GetProjectsByComplete(bool complete = false)
        {
            List<Project> projects;

            try
            {
                projects = _projectAccessor.GetProjectsByComplete(complete);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found. ", ex);
            }

            return projects;
        }


        public bool AddProject(Project project)
        {
            bool result = false;

            try
            {
                result = _projectAccessor.InsertProject(project) > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Project not added", ex);
            }
            return result;
        }

        public bool EditProject(Project oldProject, Project newProject)
        {
            bool result = false;

            try
            {
                result = _projectAccessor.UpdateProject(oldProject, newProject) == 1;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Project not edited", ex);
            }
            return result;
        }

        public Project GetProjectByID(int projectID)
        {
            Project project = null;

            try
            {
                project = _projectAccessor.GetProjectByID(projectID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Project not found", ex);
            }
            return project;
        }

        public bool SetProjectCompleteStatus(bool complete, int projectID)
        {
            bool result = false;
            try
            {
                if (complete)
                {
                    result = 1 == _projectAccessor.CompleteProject(projectID);
                }
                else
                {
                    result = 1 == _projectAccessor.DecompleteProject(projectID);
                }
                if (result == false)
                {
                    throw new ApplicationException("Project Status not Updated.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed!", ex);
            }
            return result;
        }
    }
}
