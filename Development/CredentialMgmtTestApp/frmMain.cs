using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CredentialManagement;

namespace CredentialMgmtTestApp
{
    public partial class frmMain : Form
    {
        CredentialManagement.Credential objCre;

        public frmMain()
        {
            InitializeComponent();
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            objCre = new Credential();

            objCre.Username = txtUserName.Text.Trim();
            objCre.Password = txtPassword.Text.Trim();
            objCre.Target = txtDomainKey.Text.Trim();

            if (!ControlsValidate())
            {
                MessageBox.Show("Enter all fields");
            }
            else
            {
                if (objCre.Exists())
                {
                    MessageBox.Show("Sorry, Already Exists!");
                }
                else
                {
                    objCre.Save();
                    MessageBox.Show("Saved!");
                    ClearFields();
                    LoadCredential(false);
                   
                }
            }
        }

        private bool ControlsValidate()
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()) || string.IsNullOrEmpty(txtDomainKey.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ClearFields()
        {
            txtDomainKey.Text = null;
            txtPassword.Text = null;
            txtUserName.Text = null;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadCredential(false);
        }

        private void btnExistCheck_Click(object sender, EventArgs e)
        {
            objCre = new Credential();

            objCre.Username = txtUserName.Text.Trim();
            objCre.Password = txtPassword.Text.Trim();
            objCre.Target = txtDomainKey.Text.Trim();

            if (!ControlsValidate())
            {
                MessageBox.Show("Enter all fields");
            }
            else
            {
                if(objCre.Exists())
                {
                    MessageBox.Show("Already Exists!");
                }
                else
                {
                    MessageBox.Show("Congrats, Available!");
                }

            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            //Note: Delete only  PersistanceType = Session
            CredentialSet set = new CredentialSet();
            var credentialList = set.Load();
            var FilteredData = credentialList.Where(wh => wh.Target == txtDomainKey.Text.Trim() && wh.PersistanceType == PersistanceType.Session).ToList();

            if (FilteredData.Count > 0)
            {
                foreach (var item in FilteredData)
                {
                    item.Delete();
                }
                MessageBox.Show("Item Deleted!");
                LoadCredential(false);
            }
            else
            {
                MessageBox.Show("Enter Domain");
            }
        }

        

        private void btnLoadAll_Click(object sender, EventArgs e)
        {

            LoadCredential(false);
        }

        private void LoadCredential(bool IsFiltered = true)
        {
            CredentialSet set = new CredentialSet();
            var credentialList = set.Load();

            if(!IsFiltered)
            {
                txtDomainKey.Text = "";
            }

            if (string.IsNullOrEmpty(txtDomainKey.Text.Trim()))
            {
                var FilteredData = credentialList.ToList();

                gvCredentials.DataSource = FilteredData;
            }
            else
            {
                var FilteredData = credentialList.Where(wh => wh.Target == txtDomainKey.Text.Trim()).ToList();

                gvCredentials.DataSource = FilteredData;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();

            MessageBox.Show("Cleared!");
        }
    }

}
