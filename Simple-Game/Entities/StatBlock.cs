namespace SimpleGame.Entities
{
    public class StatBlock
    {
        public decimal MaxHp{ get { return BaseHp + HpMod; } }
        public decimal BaseHp { get; set; }
        public int HpMod { get; set; }
        public decimal CurrentHp { get; set; }

        public int AttackPower { get { return BaseAttackPower + AttackPowerMod; } }
        public int BaseAttackPower { get; set; }
        public int AttackPowerMod { get; set; }

        public int DefensePower { get { return BaseDefensePower + DefensePowerMod; } }
        public int BaseDefensePower { get; set; }
        public int DefensePowerMod { get; set; }
    }
}
