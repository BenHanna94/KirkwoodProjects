using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class ProjectAccessor : IProjectAccessor
    {

        public List<Project> GetProjectsByComplete(bool complete = false)
        {
            List<Project> projects = new List<Project>();

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_projects_by_complete");
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add("@Complete", SqlDbType.Bit);
            cmd.Parameters["@Complete"].Value = complete;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var project = new Project();

                        project.ProjectID = reader.GetInt32(0);
                        project.Name = reader.GetString(1);
                        project.Type = reader.GetString(2);
                        project.Description = reader.GetString(3);
                        project.Complete = complete;

                        projects.Add(project);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return projects;
        }


        public int CompleteProject(int projectID)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_complete_project", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectID", projectID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public int DecompleteProject(int projectID)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_decomplete_project", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectID", projectID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public Project GetProjectByID(int projectID)
        {
            Project project = null;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_project_by_projectid");
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectID", projectID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        project = new Project();

                        project.ProjectID = projectID;
                        project.Name = reader.GetString(0);
                        project.Type = reader.GetString(1);
                        project.Description = reader.GetString(2);
                        project.Complete = reader.GetBoolean(3);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return project;
        }

        public int InsertProject(Project project)
        {
            int projectID = 0;

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_insert_project", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", project.Name);
            cmd.Parameters.AddWithValue("@Type", project.Type);
            cmd.Parameters.AddWithValue("@Description", project.Description);

            try
            {
                conn.Open();
                projectID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return projectID;
        }

        public int UpdateProject(Project oldProject, Project newProject)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_update_project", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectID", oldProject.ProjectID);

            cmd.Parameters.AddWithValue("@NewName", newProject.Name);
            cmd.Parameters.AddWithValue("@NewType", newProject.Type);
            cmd.Parameters.AddWithValue("@NewDescription", newProject.Description);

            cmd.Parameters.AddWithValue("@OldName", oldProject.Name);
            cmd.Parameters.AddWithValue("@OldType", oldProject.Type);
            cmd.Parameters.AddWithValue("@OldDescription", oldProject.Description);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
