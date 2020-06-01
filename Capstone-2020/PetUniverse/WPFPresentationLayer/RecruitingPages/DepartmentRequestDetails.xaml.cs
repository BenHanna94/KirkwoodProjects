using DataTransferObjects;
using LogicLayer;
using LogicLayerInterfaces;
using PresentationUtilityCode;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFPresentationLayer.RecruitingPages
{
    /// <summary>
    /// Interaction logic for DepartmentRequestDetails.xaml
    /// </summary>
    public partial class DepartmentRequestDetails : Page
    {
        private IRequestManager _requestManager;
        private IDepartmentManager _deptManager = new DepartmentManager();
        private string[] _status = new string[] { "new", "acknowledged", "complete" };
        private PetUniverseUser _user;
        private DepartmentRequestVM _request;
        private List<string> _departmentIDs = new List<string>();
        private List<string> _requestTypes;
        private List<string[]> _employeeNames;
        private string _requestStatus;
        private bool _edit = false;


        public DepartmentRequestDetails(IRequestManager requestManager, PetUniverseUser user,
            DepartmentRequestVM deptRequest, List<string> deptIDs, List<string> requestTypes, List<string[]> empNames)
        {
            _requestManager = requestManager;
            _request = deptRequest;
            _user = user;
            _departmentIDs = deptIDs;
            _requestTypes = requestTypes;
            _employeeNames = empNames;
            InitializeComponent();
            cbbRequestedGroup.ItemsSource = _departmentIDs;
            cbbRequestType.ItemsSource = _requestTypes;
            PopulateFields();

        }

        /// <summary>
        /// Creator: Ryan Morganti
        /// Created: 2020/03/10
        /// Approver: Derek Taylor
        ///
        /// Load Method to fill in all the appropriate field for the DepartmentRequest's detailed view.
        /// This method will load differently based on the date values assigned to various status point for the request.
        /// </summary>
        /// <remarks>
        /// Updater:   
        /// Updated: 
        /// Update: 
        ///
        /// </remarks>
        private void PopulateFields()
        {
            if (_request.DateAcknowledged.Year < 2000) //New Requests
            {
                if (_user.PUUserID == _request.RequestorID)
                {
                    btnEditDepartmentRequest.Visibility = Visibility.Visible;
                    btnUpdateRequestStatus.IsEnabled = false;
                    btnUpdateRequestStatus.ToolTip = "Author not allowed to update status.";
                }

                _requestStatus = _status[0];
                cbbRequestedGroup.SelectedItem = _request.RequesteeGroupID;
                cbbRequestType.SelectedItem = _request.RequestTypeID;
                lblRequestUser.Content = "Requested By: " + _request.RequestorGroupID;
                lblRequestUser.ToolTip = _request.RequestorFirstName + " " + _request.RequestorLastName;
                lblRequestStatus.Content = "Status: New";
                txtDeptRequestTopic.Text = _request.Topic;
                txtDeptRequestBody.Text = _request.Body;

            }
            else if (_request.DateAcknowledged.Year > 2000 && _request.DateCompleted.Year < 2000) //Acknowledged Requests
            {
                _requestStatus = _status[1];
                btnRetractRequest.Visibility = Visibility.Hidden;
                btnRespondToRequest.Visibility = Visibility.Visible;
                btnUpdateRequestStatus.Content = "Request Completed";
                cbbRequestedGroup.SelectedItem = _request.RequesteeGroupID;
                cbbRequestType.SelectedItem = _request.RequestTypeID;
                lblRequestUser.Content = "Requested By: " + _request.RequestorGroupID;
                lblRequestUser.ToolTip = string.Join(" ", new[] { _request.RequestorFirstName, _request.RequestorLastName });
                lblRequestStatus.Content = "Status: Acknowledged " + _request.DateAcknowledged.ToShortDateString();
                lblRequestStatus.ToolTip = _request.AcknowledgeFirstName + " " + _request.AcknowledgeLastName;
                txtDeptRequestTopic.Text = _request.Topic;
                txtDeptRequestBody.Text = _request.Body;

                try
                {
                    txtDeptRequestResponses.Text =
                        _requestManager.RetrieveAllResponsesByRequestID(_request.RequestID).ResponseListBuilder(_employeeNames);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.InnerException.Message);
                }
            }
            else if (_request.DateCompleted.Year > 2000) //Completed Requests
            {
                _requestStatus = _status[2];
                btnRetractRequest.Visibility = Visibility.Hidden;
                btnRespondToRequest.Visibility = Visibility.Hidden;
                btnUpdateRequestStatus.Visibility = Visibility.Hidden;
                cbbRequestedGroup.SelectedItem = _request.RequesteeGroupID;
                cbbRequestType.SelectedItem = _request.RequestTypeID;
                lblRequestUser.Content = "Requested By: " + _request.RequestorGroupID;
                lblRequestUser.ToolTip = string.Join(" ", new[] { _request.RequestorFirstName, _request.RequestorLastName });
                lblRequestStatus.Content = "Status: Completed " + _request.DateCompleted.ToShortDateString();
                lblRequestStatus.ToolTip = _request.CompleteFirstName + " " + _request.CompleteLastName;
                txtDeptRequestTopic.Text = _request.Topic;
                txtDeptRequestBody.Text = _request.Body;

                try
                {
                    txtDeptRequestResponses.Text =
                        _requestManager.RetrieveAllResponsesByRequestID(_request.RequestID).ResponseListBuilder(_employeeNames);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, ex.InnerException.Message);
                }
            }



        }

        /// <summary>
        /// Creator: Ryan Morganti
        /// Created: 2020/03/10
        /// Approver: Derek Taylor
        ///
        /// Click Event Method to close the request and return to the previous frame if able.
        /// </summary>
        /// <remarks>
        /// Updater:   
        /// Updated: 
        /// Update: 
        ///
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseRequestDetails_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Unable to Navigate");
            }
        }

        /// <summary>
        /// Creator: Ryan Morganti
        /// Created: 2020/03/16
        /// Approver: Derek Taylor
        ///
        /// Click Event Method to update the status of a DepartmentRequest based on its current status.
        /// Will display a success message and return to the list view if successful.
        /// Will display an error and remain on the detailed view if unsuccessful.
        /// </summary>
        /// <remarks>
        /// Updater:   
        /// Updated: 
        /// Update: 
        ///
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            if (_requestStatus == _status[0])
            {
                try
                {
                    int result = _requestManager.SetDeptRequestStatusToAcknowledged(_user.PUUserID, _request.RequestID);
                    if (result == 1)
                    {
                        MessageBox.Show("Status UPDATED!");
                        if (this.NavigationService.CanGoBack)
                        {
                            this.NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Unable to Navigate");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Status update failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.InnerException.Message);
                }
            }
            else if (_requestStatus == _status[1])
            {
                try
                {
                    int result = _requestManager.SetDeptRequestStatusToCompleted(_user.PUUserID, _request.RequestID);
                    if (result == 1)
                    {
                        MessageBox.Show("Status UPDATED!");
                    }
                    else
                    {
                        MessageBox.Show("Status update failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.InnerException.Message);
                }
            }


        }

        /// <summary>
        /// Creator: Ryan Morganti
        /// Created: 2020/03/18
        /// Approver: Derek Taylor
        ///
        /// Click Event Method to update the details of a DepartmentRequest.
        /// </summary>
        /// <remarks>
        /// Updater:   
        /// Updated: 
        /// Update: 
        ///
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditDepartmentRequest_Click(object sender, RoutedEventArgs e)
        {
            if (_edit)
            {
                if (cbbRequestedGroup.SelectedItem.ToString().IsValidDepartment(_departmentIDs))
                {
                    string newRequestedGroupID = cbbRequestedGroup.SelectedItem.ToString();
                    string newRequestTopic = txtDeptRequestTopic.Text;
                    string newRequestBody = txtDeptRequestBody.Text;

                    try
                    {
                        int result;
                        result = _requestManager.EditDepartmentRequestDetails(_request.RequestorID, _request.RequestID, _request.RequesteeGroupID, _request.Topic, _request.Body,
                                                                                newRequestedGroupID, newRequestTopic, newRequestBody);
                        if (result == 1)
                        {
                            MessageBox.Show("Request UPDATED!");

                            _edit = false;
                            cbbRequestedGroup.IsEnabled = false;
                            txtDeptRequestTopic.IsReadOnly = true;
                            txtDeptRequestBody.IsReadOnly = true;
                            btnEditDepartmentRequest.Content = "Edit";

                        }
                        else
                        {
                            MessageBox.Show("Request update failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.InnerException.Message);
                    }
                }
            }
            else
            {
                _edit = true;
                cbbRequestedGroup.IsEnabled = true;
                txtDeptRequestTopic.IsReadOnly = false;
                txtDeptRequestBody.IsReadOnly = false;

                btnEditDepartmentRequest.Content = "Save";
            }
        }
    }
}
