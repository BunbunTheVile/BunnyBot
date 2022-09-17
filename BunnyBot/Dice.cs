namespace BunnyBot
{
    public class Dice
    {
        public int Number { get; set; }
        public int Sides { get; set; }
        public int Modifier { get; set; }

        public List<int> Roll()
        {
            ///<summary>
            ///Returns a list of results. The first element (index 0) is the sum of all rolls. The remaining elements (beginning with 
            ///index 1) are all the single rolls on their own.
            ///</summary>
            var random = new Random();
            List<int> results = new() { Modifier };

            for (int i = 1; i <= Number; i++)
            {
                var result = random.Next(1, Sides + 1);
                results[0] += result;
                results.Add(result);
            }

            return results;
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
