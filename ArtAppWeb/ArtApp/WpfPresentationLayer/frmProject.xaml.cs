using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for frmProject.xaml
    /// </summary>
    public partial class frmProject : Window
    {
        private Project _project;
        private IProjectManager _projectManager;
        private bool _addMode = false;

        // add mode
        public frmProject(IProjectManager projectManager)
        {
            InitializeComponent();

            _addMode = true;
            _projectManager = projectManager;
        }

        // edit mode
        public frmProject(Project project, IProjectManager projectManager)
        {
            InitializeComponent();

            _project = project;

            _projectManager = projectManager;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_addMode == false)
            {
                txtProjectID.Text     = _project.ProjectID.ToString();
                txtName.Text          = _project.Name;
                txtType.Text          = _project.Type;
                txtDescription.Text   = _project.Description;
                chkComplete.IsChecked = _project.Complete;
            }
            else
            {
                SetEditMode();
                chkComplete.IsChecked = false;
                chkComplete.IsEnabled = false;
                lstPieces.IsEnabled = false;
            }
        }

        private void SetEditMode()
        {
            txtName.IsEnabled = true;
            txtType.IsEnabled = true;
            txtDescription.IsEnabled = true;
            chkComplete.IsEnabled = true;

            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void BtnNewPiece_Click(object sender, RoutedEventArgs e)
        {
            // Ignore this for now
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("You must enter a name.");
                txtName.Focus();
                return;
            }
            if (txtType.Text == "")
            {
                MessageBox.Show("You must enter a type.");
                txtName.Focus();
                return;
            }

            Project newProject = new Project()
            {
                Name = txtName.Text.ToString(),
                Type = txtType.Text.ToString(),
                Description = txtDescription.Text.ToString(),
                Complete = false

            };

            if (_addMode)
            {
                
                try
                {
                    if (_projectManager.AddProject(newProject))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n"
                        + ex.InnerException.Message);
                }
            }
            else
            {
                try
                {
                    if (_projectManager.EditProject(_project, newProject))
                    {
                        // success
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        throw new ApplicationException("Data not Saved.",
                            new ApplicationException("Project may no longer exist.")); ;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n"
                        + ex.InnerException.Message);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ChkComplete_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                string caption = (bool)chkComplete.IsChecked ? "Complete Project" :
                    "Decomplete Project";
                if (MessageBox.Show("Are you sure?", caption,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
                {
                    chkComplete.IsChecked = !(bool)chkComplete.IsChecked;
                    return;
                }

                _projectManager.SetProjectCompleteStatus((bool)chkComplete.IsChecked, _project.ProjectID);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n"
                    + ex.InnerException.Message);
            }
        }
    }
}
