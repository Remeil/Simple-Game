namespace SimpleGame.Entities
{
    public class StatBlock
    {
        public decimal MaxHp
        {
            get { return BaseHp + HpMod; }
        }
        public decimal BaseHp { get; set; }
        public int HpMod { get; set; }
        public decimal CurrentHp { get; set; }
    }
}
