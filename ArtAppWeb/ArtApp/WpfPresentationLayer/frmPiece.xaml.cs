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
    /// Interaction logic for frmPiece.xaml
    /// </summary>
    public partial class frmPiece : Window
    {
        private Piece _piece;
        private IPieceManager _pieceManager;
        private IReferenceManager _referenceManager;
        private bool _addMode = false;


        // add mode
        public frmPiece(IPieceManager pieceManager, int projectID)
        {
            InitializeComponent();

            _addMode = true;
            _pieceManager = pieceManager;
            txtProjectID.Text = projectID.ToString();
            _referenceManager = new ReferenceManager();
        }

        // edit mode
        public frmPiece(Piece piece, IPieceManager pieceManager)
        {
            InitializeComponent();

            _piece = piece;

            _pieceManager = pieceManager;

            _referenceManager = new ReferenceManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_addMode == false)
            {
                txtPieceID.Text = _piece.PieceID.ToString();
                txtProjectID.Text = _piece.ProjectID.ToString();
                txtAssignedUser.Text = _piece.UserID.ToString();
                txtDescription.Text = _piece.Description;
                txtStage.Text = _piece.Stage;
                txtCompensation.Text = _piece.Compensation.ToString("0.00");
                cmbCompensatedStatus.Text = _piece.CompensatedStatus;

                chkComplete.IsChecked = _piece.Complete;


                populateReferences();
                
            }
            else
            {
                SetEditMode();
                chkComplete.IsChecked = false;
                chkComplete.IsEnabled = false;
                lstAssignedRefs.IsEnabled = false;
                lstUnassignedRefs.IsEnabled = false;
                txtCompensation.Text = 0.00.ToString("0.00");
            }
            populateCompensationStatuses();
        }

        private void populateCompensationStatuses()
        {
            try
            {
                List<string> statuses = _pieceManager.GetAllCompensatedStatuses();

                statuses.Sort();

                foreach (string status in statuses)
                {
                    cmbCompensatedStatus.Items.Add(status);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void SetEditMode()
        {
            txtAssignedUser.IsEnabled = true;
            txtDescription.IsEnabled = true;
            txtStage.IsEnabled = true;
            chkComplete.IsEnabled = true;

            txtCompensation.IsEnabled = true;
            cmbCompensatedStatus.IsEnabled = true;

            lstAssignedRefs.IsEnabled = true;
            lstUnassignedRefs.IsEnabled = true;

            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void populateReferences()
        {

            try
            {
                var eRefs = _referenceManager.GetReferenceNamesByPiece(_piece.PieceID);
                lstAssignedRefs.ItemsSource = eRefs;

                var refs = _referenceManager.GetAllReferenceNames();

                foreach (string r in eRefs)
                {
                    refs.Remove(r);
                }

                lstUnassignedRefs.ItemsSource = refs;
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Error checks
            if (txtAssignedUser.Text == "")
            {
                MessageBox.Show("You must enter an assigned user.");
                txtAssignedUser.Focus();
                return;
            }
            if (txtStage.Text == "")
            {
                txtStage.Text = "Not Started";
            }
            /*
            string s = txtAssignedUser.Text;
            if (s.All(Char.IsLetter))
            {
                MessageBox.Show("You may not enter a non-numeric character in an ID field");
                txtAssignedUser.Focus();
                return;
            }

            // In case the regular expression fails?
            string s2 = txtCompensation.Text;
            if (s2.All(Char.IsLetter))
            {
                MessageBox.Show("You may not enter a non-numeric character as a money value");
                txtAssignedUser.Focus();
                return;
            } */


            Piece newPiece = new Piece();

            int i1;
            decimal d;

            newPiece.Description = txtDescription.Text;
            newPiece.Stage = txtStage.Text;
            newPiece.ProjectID = int.Parse(txtProjectID.Text);

            newPiece.CompensatedStatus = cmbCompensatedStatus.Text;
            bool result1 = int.TryParse(txtAssignedUser.Text, out i1);
            bool result2 = decimal.TryParse(txtCompensation.Text, out d);

            if (result1)
            {
                newPiece.UserID = i1;
            }
            if (result2)
            {
                newPiece.Compensation = d;
            }

            if (_addMode)
            {
                try
                {
                    if (_pieceManager.AddPiece(newPiece))
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
                    if (_pieceManager.EditPiece(_piece, newPiece))
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

        private void LstAssignedRefs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstAssignedRefs.SelectedItems.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?", "Change Reference Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                string refName = (string)lstAssignedRefs.SelectedItem;

                Reference reference = _referenceManager.GetReferenceByName(refName);

                if (_referenceManager.DeletePieceReference(reference.ReferenceID, _piece.PieceID))
                {
                    populateReferences();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LstUnassignedRefs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_addMode || lstUnassignedRefs.SelectedItems.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("Are you sure?", "Change Reference Assignment",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                string refName = (string)lstUnassignedRefs.SelectedItem;

                Reference reference = _referenceManager.GetReferenceByName(refName);

                if (_referenceManager.AddPieceReference(reference.ReferenceID, _piece.PieceID))
                {
                    populateReferences();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsNumberOnly(e.Text);

        }


        private static bool IsNumberOnly(string text)
        {
            System.Text.RegularExpressions.Regex regularExpression = new System.Text.RegularExpressions.Regex("[^0-9]");
            return regularExpression.IsMatch(text);

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

                _pieceManager.SetPieceCompleteStatus((bool)chkComplete.IsChecked, _piece.PieceID);
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

