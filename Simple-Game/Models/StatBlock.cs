using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleGame.Annotations;

namespace SimpleGame.Models
{
    public class StatBlock : INotifyPropertyChanged
    {
        private const decimal HpMult = 3.2m;

        public StatBlock()
        {
            CurrentHp = MaxHp;
            CurrentMp = MaxMp;
            Level = 1;
        }

        public StatBlock(decimal baseHp, decimal baseMp, int baseAttackPower, int baseDefensePower, int baseDodge, int baseAccuracy, int baseSpeed, int baseMagic)
        {
            BaseHp = baseHp;
            CurrentHp = MaxHp;
            BaseMp = baseMp;
            CurrentMp = MaxMp;
            BaseAttackPower = baseAttackPower;
            BaseDefensePower = baseDefensePower;
            BaseDodge = baseDodge;
            BaseAccuracy = baseAccuracy;
            BaseSpeed = baseSpeed;
            BaseMagic = baseMagic;
            Level = 1;
        }

        public decimal MaxHp { get { return (BaseHp + HpMod) * HpMult + 25; } }
        public decimal BaseHp
        {
            get
            {
                return _baseHp;
            }
            set
            {
                var oldBaseHp = _baseHp;
                _baseHp = value;
                _currentHp += (_baseHp - oldBaseHp) * HpMult;
                OnPropertyChanged(nameof(MaxHp));
                OnPropertyChanged(nameof(CurrentHp));
            }
        }

        public int HpMod { get; set; }
        public decimal CurrentHp { get { return _currentHp; } set { _currentHp = value; OnPropertyChanged(); } }
        private decimal _currentHp;
        private decimal _baseHp;

        public decimal MaxMp { get { return (BaseMp + MpMod) * (decimal)2.4 + 10; } }
        public decimal BaseMp { get; set; }
        public int MpMod { get; set; }
        public decimal CurrentMp { get; set; }

        public int AttackPower { get { return BaseAttackPower + AttackPowerMod; } }
        public int BaseAttackPower { get; set; }
        public int AttackPowerMod { get; set; }

        public int DefensePower { get { return BaseDefensePower + DefensePowerMod; } }
        public int BaseDefensePower { get; set; }
        public int DefensePowerMod { get; set; }

        public int Dodge { get { return BaseDodge + DodgeMod; } }
        public int BaseDodge { get; set; }
        public int DodgeMod { get; set; }
        public decimal DodgeChance { get { return (1 - ((decimal)30 / (Dodge + 30))); } }

        public int Accuracy { get { return BaseAccuracy + AccuracyMod; }}
        public int BaseAccuracy { get; set; }
        public int AccuracyMod { get; set; }
        public decimal AccuracyChance { get { return (1 - ((decimal)30.0 / (Accuracy + 30))); } }

        public int Speed { get { return BaseSpeed + SpeedMod; } }
        public int BaseSpeed { get; set; }
        public int SpeedMod { get; set; }

        public int Magic { get { return BaseMagic + MagicMod; } }
        public int BaseMagic { get; set; }
        public int MagicMod { get; set; }

        public long Experience { get; set; }
        public int Level { get; set; }

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
