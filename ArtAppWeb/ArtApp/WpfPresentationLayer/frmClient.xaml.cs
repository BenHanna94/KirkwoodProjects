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
    /// Interaction logic for frmClient.xaml
    /// </summary>
    public partial class frmClient : Window
    {
        private Client _client = null;
        private IClientManager _clientManager = null;
        private IReferenceManager _referenceManager = new ReferenceManager();
        bool _addMode = false;

        //Add Mode
        public frmClient(IClientManager clientManager)
        {
            InitializeComponent();

            _clientManager = clientManager;
            _addMode = true;
        }

        //Edit Mode
        public frmClient(Client client, IClientManager clientManager)
        {
            InitializeComponent();

            _clientManager = clientManager;
            _client = client;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_addMode == false)
            {
                txtClientID.Text = _client.ClientID.ToString();
                txtName.Text = _client.Name;
                txtContact.Text = _client.Contact;
                txtNotes.Text = _client.Notes;


                //Populate References
                PopulateReferences();

            }
            else
            {
                SetEditMode();
                btnNewReference.IsEnabled = false;
            }
        }

        private void PopulateReferences()
        {
            try
            {
                List<Reference> refList = _referenceManager.GetReferencesByClient(_client.ClientID);

                /*List<string> refNames = new List<string>();
                foreach (var reference in eRefs)
                {
                    refList.Add(reference.ReferenceName);
                }*/
                
                dgReferenceList.ItemsSource = refList;
                dgReferenceList.Columns.RemoveAt(5);
                dgReferenceList.Columns.RemoveAt(2);
                dgReferenceList.Columns.RemoveAt(0);

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

        private void SetEditMode()
        {
            txtName.IsEnabled = true;
            txtContact.IsEnabled = true;
            txtNotes.IsEnabled = true;

            dgReferenceList.IsEnabled = true;
            dgOrdersList.IsEnabled = true;

            btnNewReference.IsEnabled = true;


            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            // Error checking
            if (txtName.Text == "")
            {
                MessageBox.Show("You must enter a name.");
                txtName.Focus();
                return;
            }
            if (txtContact.Text == "")
            {
                MessageBox.Show("You must enter Some form of contact information.");
                txtContact.Focus();
                return;
            }
            string notes = null;
            //if(txtNotes.Text != "")
            //{
            notes = txtNotes.Text.ToString();
            //}

            Client newClient = new Client()
            {
                Name = txtName.Text.ToString(),
                Contact = txtContact.Text.ToString(),
                Notes = notes
            };
            if (_addMode)
            {
                try
                {
                    if (_clientManager.AddClient(newClient))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
            else
            {
                try
                {
                    if(_clientManager.EditClient(_client, newClient))
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnNewReference_Click(object sender, RoutedEventArgs e)
        {
            var referenceWindow = new frmReference(_referenceManager, _client);
            if(referenceWindow.ShowDialog() == true)
            {
                PopulateReferences();
            }
            
        }

        private void DgReferenceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || dgReferenceList.SelectedItems.Count == 0)
            {
                return;
            }

            Reference selectedRef = (Reference)dgReferenceList.SelectedItem;

            var referenceWindow = new frmReference(_referenceManager, selectedRef);

            if (referenceWindow.ShowDialog() == true)
            {
                PopulateReferences();
            }
        }
    }
}
