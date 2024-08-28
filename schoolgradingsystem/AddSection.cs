using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace schoolgradingsystem
{
    public partial class AddSection : Form
    {
        public int GradeLevel { get; private set; }
        public string SectionName { get; private set; }
        public AddSection()
        {
            InitializeComponent();
        }
        private bool isAddClicked = false;
        private void roundButton1_Click(object sender, EventArgs e)
        {
            isAddClicked = true; // Flag that Add button was clicked
            GradeLevel = (int)numGradeLevel.Value;
            SectionName = txtSectionName.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!isAddClicked)
            {
                this.DialogResult = DialogResult.Cancel;
            }

        }
    }
}
