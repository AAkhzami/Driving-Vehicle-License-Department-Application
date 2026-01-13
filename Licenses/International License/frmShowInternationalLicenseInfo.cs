using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_License
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        int _InternationalID = -1;
        public frmShowInternationalLicenseInfo(int InternationalID)
        {
            InitializeComponent();
            _InternationalID = InternationalID;
            ctrlDriverInternationalLicenseInfo1.LoadData(_InternationalID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
