namespace MHLab.SlayTheOrc.Decks.Cards
{
    public sealed class AttackCard : Card
    {
        public override void Play()
        {
            Battle.Player.Attack(Battle.Monster, Value);
        }
    }
}