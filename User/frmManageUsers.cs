using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DVLD_Business_Layer;

namespace DVLD_Project.User
{
    public partial class frmManageUsers: Form
    {
        private static DataTable _dtAllUsers;
        public frmManageUsers()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void LoadUsersDataToTable()
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;

            if(dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 100;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 100;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 340;

                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[3].Width = 115;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 90;
            }
        }
        private void frmMangeUsers_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            LoadUsersDataToTable();


            lblRecord.Text = dgvUsers.Rows.Count.ToString();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser form = new frmAddUpdateUser();
            form.ShowDialog(); 
            LoadUsersDataToTable();
            frmMangeUsers_Load(null, null);
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "IsActive")
            {
                txbFilter.Visible = false;
                cbAcitve.Visible = true;
                cbAcitve.SelectedIndex = 0;
                return;
            }
            else
            {
                
                txbFilter.Visible = (cbFilter.Text != "None");
                cbAcitve.Visible = false;

                txbFilter.Text = "";
                txbFilter.Focus(); 
                
            }


        }       
        private void txbFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "User Name":
                    FilterColumn = "UserName";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if(txbFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecord.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txbFilter.Text);
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterColumn,txbFilter.Text);

            lblRecord.Text = dgvUsers.Rows.Count.ToString();
        }
        private void cbAcitve_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "Is Active";
            string FilterValue = cbAcitve.Text;
            
            switch(FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;

            }
            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txbFilter.Text);

            lblRecord.Text = dgvUsers.Rows.Count.ToString();
        }
        private void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmUpdate = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frmUpdate.ShowDialog();
            frmMangeUsers_Load(null,null);
                
        }
        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser form = new frmAddUpdateUser();
            form.ShowDialog();
            LoadUsersDataToTable();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            if (UserID != -1)
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (clsUser.DeleteUser(UserID))
                    {
                        LoadUsersDataToTable();
                    }
                    else
                    {
                        MessageBox.Show("User is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmChangePasswrod frm = new frmChangePasswrod((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();                        
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm = new frmShowUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void txbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "User ID" || cbFilter.Text == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true; 
                }
            }
        }
    }
}
