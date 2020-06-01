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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _currentUser = null;

        private Client _client = null;

        private Project _project = null;

        private IUserManager _userManager;

        private IClientManager _clientManager = null;

        private IProjectManager _projectManager;

        private IPieceManager _pieceManager;

        private IReferenceManager _referenceManager;

        public MainWindow()
        {
            InitializeComponent();
            _userManager = new UserManager();
            _clientManager = new ClientManager();
            _projectManager = new ProjectManager();
            _pieceManager = new PieceManager();
            _referenceManager = new ReferenceManager();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = pwdPassword.Password;

            //Logging out
            if (btnLogin.Content.ToString() == "Logout")
            {
                LogoutUser();
                return;
            }

            if (email.Length < 7 || password.Length < 7)
            {
                MessageBox.Show("Bad username or Password.", "Login Failed.", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.Focus();
                return;
            }
            // Try to log in

            try
            {
                _currentUser = _userManager.AuthenticateUser(email, password);

                string roles = "";

                for (int i = 0; i < _currentUser.Roles.Count; i++)
                {
                    roles += _currentUser.Roles[i];
                    if (i < _currentUser.Roles.Count - 1)
                    {
                        roles += ", ";
                    }

                }

                string message = "Login Succeeded. Welcome, " + _currentUser.Name + ". You are logged in as: " + roles;
                lblStatusMessage.Content = message;

                //force new users to reset their password
                if (pwdPassword.Password == "newuser")
                {
                    var updatePassword = new frmUpdatePassword(_currentUser, _userManager);

                    // ShowDialog stops the thread until the dialogue window is resolved. Show does not, and can open as many as needed

                    if (updatePassword.ShowDialog() == false)
                    {
                        // Code to log the user back out and display and error message
                        LogoutUser();
                        MessageBox.Show("You must change your password to continue.");
                        return;
                    }
                }

                // Reset the login
                btnLogin.Content = "Logout";
                txtEmail.Text = "";
                pwdPassword.Password = "";
                txtEmail.IsEnabled = false;
                pwdPassword.IsEnabled = false;
                txtEmail.Visibility = Visibility.Hidden;
                pwdPassword.Visibility = Visibility.Hidden;
                lblEmail.Visibility = Visibility.Hidden;
                lblPassword.Visibility = Visibility.Hidden;
                ShowUserTabs();
                btnLogin.IsDefault = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LogoutUser()
        {
            _currentUser = null;
            lblStatusMessage.Content = "You are not logged in. Please log in to continue.";
            // Reset the login
            btnLogin.Content = "Login";
            txtEmail.Text = "";
            pwdPassword.Password = "";
            txtEmail.IsEnabled = true;
            pwdPassword.IsEnabled = true;
            txtEmail.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;

            HideAllUserTabs();
            btnLogin.IsDefault = true;
            return;
        }

        private void HideAllUserTabs()
        {
            foreach (TabItem item in tabsetMain.Items)
            {
                item.Visibility = Visibility.Collapsed;
            }

            tabsetMain.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HideAllUserTabs();
            dgPieceList.Visibility = Visibility.Hidden;
            DataObjects.AppDetails.AppPath = AppContext.BaseDirectory;
        }

        private void ShowUserTabs()
        {
            tabsetMain.Visibility = Visibility.Visible;

            foreach (var r in _currentUser.Roles)
            {
                switch (r)
                {
                    case "Contributor":
                        tabProjects.Visibility = Visibility.Visible;
                        tabProjects.IsSelected = true;
                        tabReferences.Visibility = Visibility.Visible;
                        break;
                    case "Manager":
                        tabProjects.Visibility = Visibility.Visible;
                        tabManagement.Visibility = Visibility.Visible;
                        dgUserList.Visibility = Visibility.Visible;
                        tabManagement.IsSelected = true;
                        break;
                    case "Customer Service":
                        tabClients.Visibility = Visibility.Visible;
                        dgClientList.Visibility = Visibility.Visible;
                        break;
                    case "Admin":
                        tabProjects.Visibility = Visibility.Visible;
                        tabReferences.Visibility = Visibility.Visible;
                        tabClients.Visibility = Visibility.Visible;
                        tabManagement.Visibility = Visibility.Visible;

                        dgClientList.Visibility = Visibility.Visible;
                        dgUserList.Visibility = Visibility.Visible;
                        tabProjects.IsSelected = true;
                        break;
                    
                }


            }
        }

        private void DgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User selectedUser = (User)dgUserList.SelectedItem;

            var userWindow = new frmUser(selectedUser, _userManager, _currentUser);
            if (userWindow.ShowDialog() == true)
            {
                refreshUserList();
            }
        }



        private void DgClientList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Client selectedClient = (Client)dgClientList.SelectedItem;

            var clientWindow = new frmClient(selectedClient, _clientManager);

            if (clientWindow.ShowDialog() == true)
            {
                refreshClientList();
            }
        }

        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            var clientWindow = new frmClient(_clientManager);


            if (clientWindow.ShowDialog() == true)
            {
                refreshClientList();
            }
            
        }


        private void TabManagement_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgUserList.ItemsSource == null)
                {
                    refreshUserList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void TabClients_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dgClientList.ItemsSource == null)
                {
                    refreshClientList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        private void TabProjects_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProjectList.ItemsSource == null)
                {
                    refreshProjects();

                    dgPieceList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void refreshProjects()
        {
            try
            {
                dgProjectList.ItemsSource = _projectManager.GetProjectsByComplete((bool)chkCompleteProjects.IsChecked);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n");
            }
        }

        private void DgProjectList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Project selectedProject = (Project)dgProjectList.SelectedItem;

            // Store this project temporarily
            _project = selectedProject;


            var projectWindow = new frmProject(selectedProject, _projectManager);
            projectWindow.ShowDialog();
            refreshProjects();
        }

        private void DgProjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project selectedProject = (Project)dgProjectList.SelectedItem;
            if(selectedProject == null)
            {
                // If null, assign the the value of the stored project to selectedProject
                selectedProject = _project;
                // Then, reset the project back to null. 
                _project = null;
            }
            RefreshPieces(selectedProject);
        }

        private void RefreshPieces(Project selectedProject)
        {
            try
            {
                dgPieceList.ItemsSource = _pieceManager.GetPiecesByProject(selectedProject.ProjectID, (bool)chkCompletePiece.IsChecked);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n");
            }
        }

        private void DgPieceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Piece selectedPiece = (Piece)dgPieceList.SelectedItem;
            Project project = (Project)dgProjectList.SelectedItem;

            if (project != null)
            {
                var pieceWindow = new frmPiece(selectedPiece, _pieceManager);
                if (pieceWindow.ShowDialog() == true)
                {
                    RefreshPieces(project);
                }
            }
            else
            {
                MessageBox.Show("You must select a project first", "Piece Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            


        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new frmUser(_userManager, _currentUser);
            if (userWindow.ShowDialog() == true)
            {
                refreshUserList();
            }
        }

        private void refreshUserList(bool active = true)
        {
            dgUserList.ItemsSource = _userManager.RetrieveUserListByActive((bool)chkActive.IsChecked);

        }

        private void refreshClientList()
        {
            dgClientList.ItemsSource = _clientManager.RetrieveAllClients();
            
        }

        private void ChkActive_Click(object sender, RoutedEventArgs e)
        {
            refreshUserList();
        }

        private void ChkCompleteProjects_Click(object sender, RoutedEventArgs e)
        {
            refreshProjects();
        }

        private void ChkCompletePiece_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = (Project)dgProjectList.SelectedItem;

            RefreshPieces(selectedProject);
        }



        private void DgProjectList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgProjectList.Columns.RemoveAt(5);
            dgProjectList.Columns.RemoveAt(4);
        }

        private void DgPieceList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgPieceList.Columns[2].Header = "Assigned User";
            dgPieceList.Columns[4].Header = "Current Stage";
            dgPieceList.Columns[7].Header = "Compensated";

            dgPieceList.Columns.RemoveAt(8);
            dgPieceList.Columns.RemoveAt(6);
            dgPieceList.Columns.RemoveAt(5);
        }

        private void DgClientList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgClientList.Columns.RemoveAt(4);
        }

        private void DgUserList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgUserList.Columns.RemoveAt(6);
            dgUserList.Columns.RemoveAt(5);
            dgUserList.Columns.RemoveAt(4);
        }

        private void BtnAddProject_Click(object sender, RoutedEventArgs e)
        {
            var projectWindow = new frmProject(_projectManager);
            if(projectWindow.ShowDialog() == true)
            {
                refreshProjects();
            }
        }

        private void BtnAddPiece_Click(object sender, RoutedEventArgs e)
        {
            Project project = (Project)dgProjectList.SelectedItem;

            if (project != null)
            {
                var pieceWindow = new frmPiece(_pieceManager, project.ProjectID);
                if (pieceWindow.ShowDialog() == true)
                {
                    RefreshPieces(project);
                }
            }
            else
            {
                MessageBox.Show("You must select a project first", "Piece Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void TabReferences_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgReferenceList.ItemsSource == null)
                {
                    refreshReferences();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void refreshReferences()
        {
            try
            {
                dgReferenceList.ItemsSource = _referenceManager.GetAllReferences();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n");
            }
        }

        private void DgReferenceList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgReferenceList.Columns.RemoveAt(5);
        }

        private void DgReferenceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Reference selectedRef = (Reference)dgReferenceList.SelectedItem;

            var referenceWindow = new frmReference(_referenceManager, selectedRef);

            if (referenceWindow.ShowDialog() == true)
            {
                refreshReferences();
            }
        }

        private void BtnAddReference_Click(object sender, RoutedEventArgs e)
        {
            var referenceWindow = new frmReference(_referenceManager);
            if (referenceWindow.ShowDialog() == true)
            {
                refreshReferences();
            }
        }
    }
}
