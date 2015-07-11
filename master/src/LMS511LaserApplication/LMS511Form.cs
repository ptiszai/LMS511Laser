using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS511LaserApplication
{
    public partial class LMS511Form : Form
    {
        #region Variables private
        private Laser laser;
        private bool doTerminate;
        private int testCounter;
        #endregion

        #region Construction and Initiaize all Comps and Laser. 
        public LMS511Form()
        {
            InitializeComponent();
            try
            {
                testCounter = 0;
                laser = new Laser();
                laser.Initialize();
                laser.StatusComponentEvent += new Action<string>(StatusComponentHandler);
                laser.ScanResultComponentEvent += new Action<string>(ScanResultComponentHandler); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                doTerminate = true;
            }
        }
        #endregion
        #region EventHandler part
        private void LMS511Form_Load(object sender, EventArgs e)
        {
            if (doTerminate)
            {
                Application.Exit();
            }
        }
        private void startDriverBtn_Click(object sender, EventArgs e)
        {
            if (laser.Start())
            {
                startDriverBtn.Enabled = false;
                stopDriverBtn.Enabled  = true;
                ReBootBtn.Enabled = true;
            }           
        }      
        private void stopDriverBtn_Click(object sender, EventArgs e)
        {
            if (laser.Stop())
            {
                startDriverBtn.Enabled = true;
                stopDriverBtn.Enabled = false;
                ReBootBtn.Enabled = false;
            } 
        }        
        private void ReBootBtn_Click(object sender, EventArgs e)
        {
            laser.ReBoot();
           // throw new NotImplementedException();
        }       
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }           
        private void clearStateBtn_Click(object sender, EventArgs e)
        {
            StatusText.Clear();
        }
        private void clearResultBtn_Click(object sender, EventArgs e)
        {
            ResultText.Clear();
        }
        private void StatusComponentHandler(string message)
        {
            this.StatusText.Invoke(new Action(() => 
            {
                if (this.StatusText.Text.Length > 1000)
                    this.StatusText.Clear();
                this.StatusText.AppendText(message + "\n"); 
                this.StatusText.SelectionStart = StatusText.Text.Length;
                this.StatusText.ScrollToCaret();
            }));
        }
        private void ScanResultComponentHandler(string message)
        {
            this.ResultText.Invoke(new Action(() =>
            {
                this.ResultText.Text = message;            
            }));
        }      
        #endregion
    }
}
