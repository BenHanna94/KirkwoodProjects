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
    /// Interaction logic for frmUser.xaml
    /// </summary>

    public partial class frmUser : Window
    {
        private User _currentUser;
        private User _user = null;
        private IUserManager _userManager = null;
        private bool _addMode = false;


        // Add mode
        public frmUser(IUserManager userManager, User currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser;
            _userManager = userManager;
            _addMode = true;
        }

        // Edit mode
        public frmUser(User user, IUserManager userManager, User currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser;
            _user = user;
            _userManager = userManager;
        }

        private void SetEditMode()
        {
            txtName.IsEnabled = true;
            txtEmail.IsEnabled = true;
            txtPhoneNumber.IsEnabled = true;
            chkActive.IsEnabled = true;

            lstAssignedRoles.IsEnabled = true;
            lstUnassignedRoles.IsEnabled = true;

            lstAssignedSkills.IsEnabled = true;
            lstUnassignedSkills.IsEnabled = true;


            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;

            txtName.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_addMode == false)
            {
                txtUserID.Text = _user.UserID.ToString();
                txtName.Text = _user.Name;
                txtEmail.Text = _user.Email;
                txtPhoneNumber.Text = _user.PhoneNumber;
                chkActive.IsChecked = _user.Active;

                populateRoles();
                populateSkills();
            }
            else
            {
                SetEditMode();
                chkActive.IsChecked = true;
                chkActive.IsEnabled = false;
            }
        }

        private void populateRoles()
        {
            try
            {
                var eRoles = _userManager.RetrieveUserRoles(_user.UserID);
                lstAssignedRoles.ItemsSource = eRoles;

                var roles = _userManager.RetrieveUserRoles();

                for (int i = 0; i < roles.Count; i++)
                {
                    foreach (string r in eRoles)
                    {
                        roles.Remove(r);
                    }

                    lstUnassignedRoles.ItemsSource = roles;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void populateSkills()
        {
            try
            {
                var eSkills = _userManager.RetrieveUserSkills(_user.UserID);
                lstAssignedSkills.ItemsSource = eSkills;

                var skills = _userManager.RetrieveUserSkills();

                for (int i = 0; i < skills.Count; i++)
                {
                    foreach (string s in eSkills)
                    {
                        skills.Remove(s);
                    }

                    lstUnassignedSkills.ItemsSource = skills;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            {
                // Some error checks
                if (txtName.Text == "")
                {
                    MessageBox.Show("You must enter aname.");
                    txtName.Focus();
                    return;
                }
                if (!(txtEmail.Text.ToString().Length > 6
                    && txtEmail.Text.ToString().Contains("@")
                    && txtEmail.Text.ToString().Contains(".")))
                {
                    MessageBox.Show("You must enter a valid Email Address.");
                    txtEmail.Focus();
                    return;
                }
                if (txtPhoneNumber.Text.ToString().Length < 10)
                {
                    MessageBox.Show("You must enter a valid phone number.");
                    txtPhoneNumber.Focus();
                    return;
                }

                User newUser = new User()
                {
                    Name = txtName.Text.ToString(),
                    PhoneNumber = txtPhoneNumber.Text.ToString(),
                    Email = txtEmail.Text.ToString(),
                    Active = (bool)chkActive.IsChecked
                };

                if (_addMode)
                {
                    try
                    {
                        if (_userManager.AddUser(newUser))
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
                        if (_userManager.EditUser(_user, newUser))
                        {
                            // success
                            this.DialogResult = true;
                            this.Close();
                        }
                        else
                        {
                            throw new ApplicationException("Data not saved.", new ApplicationException("User may no longer exist."));
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                    }
                }


            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ChkActive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string caption = (bool)chkActive.IsChecked ? "Reactivate User?" :
                    "Deactivate User?";
                if (MessageBox.Show("Are you sure?", caption,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
                {
                    chkActive.IsChecked = !(bool)chkActive.IsChecked;
                    return;
                }
                _userManager.SetUserActiveState((bool) chkActive.IsChecked, _user.UserID);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstUnassignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstUnassignedRoles.SelectedItems.Count == 0)
            {
                return;
            }

            int roleCheckCount = 0;

            foreach (var r in _currentUser.Roles)
            {
                if (string.Equals(r, "Admin") || string.Equals(r, "Manager"))
                {
                    roleCheckCount += 1;
                }
            }

            if (roleCheckCount < 1)
            {
                MessageBox.Show("You do not have the permissions to perform this action.");
                return;
            }

            if (MessageBox.Show("Are you sure?", "Change Role Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                chkActive.IsChecked = !(bool)chkActive.IsChecked;
                return;
            }

            try
            {
                if (_userManager.AddUserRole(_user.UserID, (string)lstUnassignedRoles.SelectedItem))
                {
                    populateRoles();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstAssignedRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstAssignedRoles.SelectedItems.Count == 0)
            {
                return;
            }


            if (string.Equals((string)lstAssignedRoles.SelectedItem, "Admin"))
            {
                List<int> users = _userManager.RetrieveUserIDsByRole("Admin");

                if (users.Count <= 1)
                {
                    MessageBox.Show("There must be at least one user with the Admin role");
                    return;
                }
            }

            int roleCheckCount = 0;

            foreach (var r in _currentUser.Roles)
            {
                if (string.Equals(r, "Admin") || string.Equals(r, "Manager"))
                {
                    roleCheckCount += 1;
                }
            }

            if (roleCheckCount < 1)
            {
                MessageBox.Show("You do not have the permissions to perform this action.");
                return;
            }


            if (MessageBox.Show("Are you sure?", "Change Role Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                chkActive.IsChecked = !(bool)chkActive.IsChecked;
                return;
            }

            try
            {
                if (_userManager.DeleteUserRole(_user.UserID, (string)lstAssignedRoles.SelectedItem))
                {
                    populateRoles();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstUnassignedSkills_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstUnassignedSkills.SelectedItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Are you sure?", "Change Skill Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                chkActive.IsChecked = !(bool)chkActive.IsChecked;
                return;
            }

            try
            {
                if (_userManager.AddUserSkill(_user.UserID, (string)lstUnassignedSkills.SelectedItem))
                {
                    populateSkills();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstAssignedSkills_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (_addMode || lstAssignedSkills.SelectedItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Are you sure?", "Change Skill Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                chkActive.IsChecked = !(bool)chkActive.IsChecked;
                return;
            }

            try
            {
                if (_userManager.DeleteUserSkill(_user.UserID, (string)lstAssignedSkills.SelectedItem))
                {
                    populateSkills();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        
    }
}
