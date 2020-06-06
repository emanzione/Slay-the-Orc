namespace MHLab.SlayTheOrc.Decks.Cards
{
    public sealed class ShieldCard : Card
    {
        public override void Play()
        {
            Battle.Player.Shield(Value);
        }
    }
}