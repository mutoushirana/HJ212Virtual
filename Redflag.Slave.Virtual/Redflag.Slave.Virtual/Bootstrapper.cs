using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Redflag.Slave.Virtual.ViewModels;

namespace Redflag.Slave.Virtual
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object obj, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
