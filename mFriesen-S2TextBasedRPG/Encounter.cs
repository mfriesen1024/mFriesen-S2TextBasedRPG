namespace mFriesen_S2TextBasedRPG
{
    internal class Encounter
    {
        // Encounter stores everything for a specific encounter
        Foe[] foes;
        Vector2 startDialogue;
        Vector2 endDialogue;

        public Encounter(Foe[] foes, Vector2 startDialogue, Vector2 endDialogue)
        {
            this.foes = foes;
            this.startDialogue = startDialogue;
            this.endDialogue = endDialogue;
        }
    }
}
