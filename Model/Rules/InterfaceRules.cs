using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model.Rules
{
    interface InterfaceRules
    {
        BasicRules BasicRules { get; set; }

        int GetValueByVariant(Variant.Type variant);

        bool HasSmallStraight();

        bool HasLargeStraight();

    }
}
