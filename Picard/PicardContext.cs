using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard
{
    class PicardContext : ApplicationContext
    {
        public PicardContext()
        {
            InaraApi api = new InaraApi();
            using (WelcomeLoginForm loginFrm = new WelcomeLoginForm(api))
            {
                loginFrm.ShowDialog();

                if (!api.isAuthenticated)
                {
                    Application.Exit();
                }

                MainForm = new MatInitialVerifyForm(api);
                MainForm.Show();
            }
        }
    }
}
