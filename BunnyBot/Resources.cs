using Discord;

namespace BunnyBot
{
    public static class Resources
    {
        public static List<string> EmojiStrings = new()
        {
            "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣", "6️⃣", "7️⃣", "8️⃣" ,"9️⃣",
            "🍏", "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓","🍒", "🥝"
        };

        public static List<Emoji> Emojis => EmojiStrings.Select(x => new Emoji(x)).ToList();

        public const string NotesPath = "notes.dat";
    }
}
