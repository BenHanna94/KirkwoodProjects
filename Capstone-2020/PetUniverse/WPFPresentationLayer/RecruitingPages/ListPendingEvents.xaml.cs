﻿using DataTransferObjects;
using LogicLayer;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFPresentationLayer.RecruitingPages
{
    /// <summary>
    /// Interaction logic for ListPendingEvents.xaml
    /// 
    /// CREATOR: Steve Coonrod
    /// CREATED: 3\08\2020
    /// APPROVER: Ryan Morganti
    /// 
    /// This is the page for displaying the list of Events pending approval in the DB
    /// This is ADMIN and DC roles only
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// UPDATER: NA
    /// UPDATED: NA
    /// UPDATE: NA
    /// 
    /// </remarks>
    public partial class ListPendingEvents : Page
    {
        private IPUEventManager _eventManager = null;//For using event manager methods
        private EventMgmt _eventMgmt;//For allowing the updating of the EventMgmt page's _selectedEvent

        /// <summary>
        /// 
        /// CREATOR: Steve Coonrod
        /// CREATED: 3\08\2020
        /// APPROVER: Ryan Morganti
        /// 
        /// A no-argumnet constructor. For some reason it is necessary for the programs initialization
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        public ListPendingEvents()
        {
            InitializeComponent();
            _eventManager = new PUEventManager();
        }

        /// <summary>
        /// 
        /// CREATOR: Steve Coonrod
        /// CREATED: 3\08\2020
        /// APPROVER: Ryan Morganti
        /// 
        /// The constructor for this page.
        /// Takes as parameters the current Event Manager and EventMgmt page
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="eventManager"></param>
        /// <param name="eventMgmt"></param>
        public ListPendingEvents(IPUEventManager eventManager, EventMgmt eventMgmt)
        {
            _eventManager = eventManager;
            _eventMgmt = eventMgmt;
            InitializeComponent();

        }

        /// <summary>
        /// 
        /// CREATOR: Steve Coonrod
        /// CREATED: 3\08\2020
        /// APPROVER: Ryan Morganti
        /// 
        /// This is a handler for the datagrid, to hide unwanted fields
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgEventList_Pending_AutoGeneratedColumns(object sender, EventArgs e)
        {
            dgEventList_Pending.Columns.RemoveAt(11);
            dgEventList_Pending.Columns.RemoveAt(10);
            dgEventList_Pending.Columns.RemoveAt(2);
            dgEventList_Pending.Columns.RemoveAt(1);
            dgEventList_Pending.Columns.RemoveAt(0);
            dgEventList_Pending.Columns[1].Header = "Type";
            dgEventList_Pending.Columns[2].Header = "Date and Time";
            dgEventList_Pending.Columns[dgEventList_Pending.Columns.Count - 1].Width =
                new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        /// <summary>
        /// 
        /// CREATOR: Steve Coonrod
        /// CREATED: 3\08\2020
        /// APPROVER: Ryan Morganti
        /// 
        /// This is an event handler for when the datagrid is loaded
        /// Retrieves its item source from the DB through the event manager
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgEventList_Pending_Loaded(object sender, RoutedEventArgs e)
        {
            List<PUEvent> eventList = new List<PUEvent>();
            try
            {
                if (dgEventList_Pending.ItemsSource == null)
                {
                    eventList = _eventManager.GetEventsByStatus("PendingApproval");
                    dgEventList_Pending.ItemsSource = eventList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.InnerException.Message);
            }

        }

        /// <summary>
        /// CREATOR: Steve Coonrod
        /// CREATED: 3\08\2020
        /// APPROVER: Ryan Morganti
        /// 
        /// This is the event handler for the datagrids selection_changed event
        /// This sets the EventMgmt page's _selectedEvent value to the datagrids selected item
        /// Then toggles the buttons that rely on an event being selected
        /// 
        /// Updated By:     
        /// Date Updated: 
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// UPDATER: NA
        /// UPDATED: NA
        /// UPDATE: NA
        /// 
        /// </remarks>
        private void dgEventList_Pending_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _eventMgmt._selectedEvent = null;
            _eventMgmt._selectedEvent = (PUEvent)dgEventList_Pending.SelectedItem;
            _eventMgmt.ToggleEventButtons();
        }
    }
}