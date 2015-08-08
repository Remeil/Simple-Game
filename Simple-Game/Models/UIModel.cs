using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleGame.Annotations;

namespace SimpleGame.Models
{
    public class UIModel : StatBlock, INotifyPropertyChanged
    {
        private List<string> systemMessages;
        private decimal currentHp;

        public List<string> SystemMessages { get { return systemMessages; } set { OnPropertyChanged(); systemMessages = value; } }

        public decimal CurrentHp { get { return currentHp; } set { currentHp = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
