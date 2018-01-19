using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleGame.Annotations;

namespace SimpleGame.Models
{
    public class UIModel : StatBlock, INotifyPropertyChanged
    {
        public UIModel() : base()
        {
            CurrentHp = base.CurrentHp;
            MaxHp = base.MaxHp;
            CurrentMp = base.CurrentMp;
            MaxMp = base.MaxMp;
        }

        public UIModel(StatBlock stats)
        {
            CurrentHp = stats.CurrentHp;
            MaxHp = stats.MaxHp;
            CurrentMp = stats.CurrentMp;
            MaxMp = stats.MaxMp;
        }

        private List<string> systemMessages;
        private decimal currentHp;
        private decimal maxHp;
        private decimal currentMp;
        private decimal maxMp;

        public List<string> SystemMessages { get { return systemMessages; } set { systemMessages = value; OnPropertyChanged(); } }

        public new decimal CurrentHp { get { return currentHp; } set { currentHp = value; OnPropertyChanged(); } }
        public new decimal MaxHp { get { return maxHp; } set { maxHp = value; OnPropertyChanged(); } }
        public new decimal CurrentMp { get { return currentMp; } set { currentMp = value; OnPropertyChanged(); } }
        public new decimal MaxMp { get { return maxMp; } set { maxMp = value; OnPropertyChanged(); } }

        public new event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
