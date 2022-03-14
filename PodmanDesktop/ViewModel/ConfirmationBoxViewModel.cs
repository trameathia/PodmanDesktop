using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PodmanDesktop.ViewModel
{
    public class ConfirmationBoxViewModel
    {
        private string title;
        private string message;
        public string? Title {
            get => title;
            set => title = value;
        }
        public string? Message { 
            get => message;
            set => message = value;
        }
    }
}
