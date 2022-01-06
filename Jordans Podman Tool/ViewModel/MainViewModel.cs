using System;
using System.Windows.Input;

namespace Jordans_Podman_Tool.ViewModel
{
    public class MainViewModel
    {
        private bool useSudo;
        private ICommand useSudoCommand;
        public ICommand UseSudoCommand
        {
            get => useSudoCommand;
            set => useSudoCommand = value;
        }

        public bool UseSudo
        {
            get => useSudo;
            set => useSudo = value; 
        }

        public MainViewModel()
        {
            useSudo = false;
            UseSudoCommand = new RelayCommand(new Action<object>(UpdateUseSudo));
        }

        private void UpdateUseSudo(object obj)
        {
            ;
        }
    }
}
