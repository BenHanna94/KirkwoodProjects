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
    /// Interaction logic for frmReferencesQuery.xaml
    /// </summary>
    public partial class frmReferencesQuery : Window
    {
        private IReferenceManager _referenceManager;
        private Client _client = null;
        private IClientManager _clientManager = null;

        public frmReferencesQuery(Client client, IClientManager clientManager)
        {
            InitializeComponent();

            _client = client;
            _clientManager = clientManager;
            _referenceManager = new ReferenceManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgReferencesList.ItemsSource = _referenceManager.GetReferencesByClient(_client.ClientID);
                dgReferencesList.Visibility = Visibility.Visible;

                dgReferencesList.Columns[1].Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + "Reference data not found.");
            }
        }
    }
}
