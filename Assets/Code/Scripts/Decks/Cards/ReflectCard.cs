namespace MHLab.SlayTheOrc.Decks.Cards
{
    public sealed class ReflectCard : Card
    {
        public override void Play()
        {
            Battle.Player.Reflect(Value);
        }
    }
}