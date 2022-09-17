namespace BunnyBot
{
    public class Dice
    {
        public int Number { get; set; }
        public int Sides { get; set; }
        public int Modifier { get; set; }

        public int Roll()
        {
            var random = new Random();
            int result = Modifier;
            for (int i = 0; i < Number; i++)
            {
                result += random.Next(1, Sides + 1);
            }
            return result;
        }

        public static Dice? ParseDiceString(string diceString)
        {
            // Initializations
            diceString = diceString.Trim().ToLower();

            int number = 1;
            int sides;
            int modifier = 0;

            var dIndex = diceString.IndexOf("d");
            var plusIndex = diceString.IndexOf("+");
            var minusIndex = diceString.IndexOf("-");

            // Early returns
            if (plusIndex > 0 && minusIndex > 0) return null;
            if (dIndex < 0) return null;

            // Number parsing
            if (dIndex != 0)
            {
                var numberSubstring = diceString.Substring(0, dIndex);
                var parseable = int.TryParse(numberSubstring, out number);
                if (!parseable) return null;
            }

            // Sides parsing
            int signIndex = -1;
            if (plusIndex > 0) signIndex = plusIndex;
            if (minusIndex > 0) signIndex = minusIndex;

            var sidesString = signIndex > 0
                ? diceString.Substring(dIndex + 1, signIndex - dIndex - 1)
                : diceString.Substring(dIndex + 1);

            if (!int.TryParse(sidesString, out sides)) return null;

            // Modifier parsing
            if (signIndex > 0)
            {
                var modifierSubString = diceString.Substring(signIndex);
                var parseable = int.TryParse(modifierSubString, out modifier);
                if (!parseable) return null;
            }

            return new Dice
            {
                Number = number,
                Sides = sides,
                Modifier = modifier
            };
        }
    }
}
