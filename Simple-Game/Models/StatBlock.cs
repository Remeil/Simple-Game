namespace SimpleGame.Models
{
    public class StatBlock
    {
        public decimal MaxHp{ get { return (BaseHp + HpMod) * (decimal)3.2 + 25; } }
        public decimal BaseHp { get; set; }
        public int HpMod { get; set; }
        public decimal CurrentHp { get; set; }

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
    }
}
