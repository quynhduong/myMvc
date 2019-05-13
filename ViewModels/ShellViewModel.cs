using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopWPFUI.ViewModels;

namespace TRMDesktopWPFUserInterface.ViewModels
{
    public class ShelViewModel: Conductor<object>
    {
        //Create holder for it
        private LoginViewModel _loginVM;

        public ShelViewModel(LoginViewModel loginVM)
        {
            //Create a constructor
            _loginVM = loginVM;
            ActivateItem(_loginVM);
        }
    }
}