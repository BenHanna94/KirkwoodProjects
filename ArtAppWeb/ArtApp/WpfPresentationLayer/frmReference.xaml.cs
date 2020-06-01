using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for frmReference.xaml
    /// </summary>
    public partial class frmReference : Window
    {
        private Client _client = null;
        private Reference _reference = null;
        private IClientManager _clientManager = null;
        private IReferenceManager _referenceManager = null;
        bool _addMode = false;

        // Add mode
        public frmReference(IReferenceManager referenceManager)
        {
            InitializeComponent();

            _referenceManager = referenceManager;

            _addMode = true;

            // Default value for no client
            txtClientID.Text = "1000000";
            lblClient.Content = "Client ID (Default: No Client):";
            
        }

        // Add from client
        public frmReference(IReferenceManager referenceManager, Client client)
        {
            InitializeComponent();

            _referenceManager = referenceManager;

            _client = client;

            _addMode = true;

            txtClientID.Text = _client.ClientID.ToString();

            txtClientID.IsReadOnly = true;

            txtClientID.Background = Brushes.Azure;
        }

        // Edit mode
        public frmReference(IReferenceManager referenceManager, Reference reference)
        {
            InitializeComponent();

            _referenceManager = referenceManager;

            _reference = reference;

            txtClientID.IsReadOnly = true;

            txtClientID.Background = Brushes.Azure;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_addMode == false)
            {
                txtReferenceID.Text = _reference.ReferenceID.ToString();
                txtReferenceName.Text = _reference.ReferenceName;
                txtClientID.Text = _reference.ClientID.ToString();
                txtDescription.Text = _reference.Description;
                txtFileLocation.Text = _reference.FileLocation;
                fetchImage();
            }

            else
            {
                SetEditMode();
            }
        }

        private void fetchImage()
        {
            string imageString = AppDetails.ReferenceImagePath + _reference.FileLocation;
            try
            {
                if (File.Exists(imageString))
                {
                    imgReferenceImage.Source = new BitmapImage(new Uri(imageString));
                }
                else
                {
                    imgReferenceImage.Source = new BitmapImage(new Uri(AppDetails.ImagePath + "fileNotFound.png"));
                }
            }
            catch (FileNotFoundException ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void SetEditMode()
        {
            txtReferenceName.IsEnabled = true;
            if (_addMode)
            {
                txtClientID.IsEnabled = true; 
            }
            txtDescription.IsEnabled = true;
            txtFileLocation.IsEnabled = true;
            btnDelete.IsEnabled = true;

            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SetEditMode();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)

        {

            // Error checking
            if (txtReferenceName.Text == "")
            {
                MessageBox.Show("You must enter a name for your reference.");
                txtReferenceName.Focus();
                return;
            }
            if (txtFileLocation.Text == "")
            {
                MessageBox.Show("You must enter Some form of file location");
                txtFileLocation.Focus();
                return;
            }

            string s = txtClientID.Text;
            if (s.All(Char.IsLetter))
            {
                MessageBox.Show("You may not enter a letter in an ID field");
                txtClientID.Focus();
                return;
            }
            string desc = null;
            if (txtDescription.Text != "")
            {
                desc = txtDescription.Text.ToString();
            }
            int clientID;
            bool success = int.TryParse(txtClientID.Text, out clientID);
            Reference newRreference = new Reference();
            newRreference.ReferenceName = txtReferenceName.Text;
            newRreference.Description = txtDescription.Text;
            newRreference.FileLocation = txtFileLocation.Text;

            if (success)
            {
                newRreference.ClientID = clientID;
            }

            
            if (_addMode)
            {
                try
                {
                    if (_referenceManager.AddReference(newRreference))
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
                    if (_referenceManager.EditReference(_reference, newRreference))
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

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Delete Reference",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                _referenceManager.DeleteReference(_reference);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
            finally
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);

        }


        private static bool IsTextNumeric(string text)
        {
            System.Text.RegularExpressions.Regex regularExpression = new System.Text.RegularExpressions.Regex("[^0-9]");
            return regularExpression.IsMatch(text);

        }
    }
}
