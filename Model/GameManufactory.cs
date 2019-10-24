using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model
{
    class GameManufactory
    {
        private GameType gameType;
        public GameManufactory(GameType gameType)
        {
            this.gameType = gameType;
        }

        public InterfaceRules GetRules(DiceCollection diceCollection)
        {
            if (gameType == GameType.Yahtzee)
            {
            }
            else if (gameType == GameType.Yatzy)
            {
            }
            return null;
        }

        public Variant GetVariant()
        {
            if (gameType == GameType.Yahtzee)
            {
                return new VariantYahtzee();
            }
            else if (gameType == GameType.Yatzy)
            {
                return new VariantYatzy();
            }
            return null;
        }
    }
}
