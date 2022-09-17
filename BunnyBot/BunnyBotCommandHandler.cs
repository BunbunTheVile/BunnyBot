using Discord.WebSocket;

namespace BunnyBot
{
    public static class BunnyBotCommandHandler
    {
        public static async Task HandleSlashCommand(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await RespondToPing(command);
                    break;
                case "roll":
                    await RespondToRoll(command);
                    break;
                case "poll":
                    await RespondToPoll(command);
                    break;
                case "specific-poll":
                    await RespondToSpecificPoll(command);
                    break;
                case "note":
                    await RespondToNote(command);
                    break;
                case "remember":
                    await RespondToRemember(command);
                    break;
                case "forget":
                    await RespondToForget(command);
                    break;
            }
        }

        private static async Task RespondToNote(SocketSlashCommand command)
        {
            await command.RespondAsync("Sorry, this command isn't implemented yet. ._.");
        }

        private static async Task RespondToRemember(SocketSlashCommand command)
        {
            await command.RespondAsync("Sorry, this command isn't implemented yet. ._.");
        }

        private static async Task RespondToForget(SocketSlashCommand command)
        {
            await command.RespondAsync("Sorry, this command isn't implemented yet. ._.");
        }

        private static async Task RespondToSpecificPoll(SocketSlashCommand command)
        {
            // Get dates

            var dateString = command.Data.Options.First().Value as string;
            if (dateString == null) 
            {
                await command.RespondAsync($"Your dates seem faulty. :/");
                return;
            }

            var dates = ParseDateString(dateString);
            if (dates == null)
            {
                await command.RespondAsync($"Your dates \"{dateString}\" don't seem right. Parsing them hurt my little CPU...");
                return;
            }
            if (dates.Count > 20)
            {
                await command.RespondAsync($"{dates.Count} DATES?!? THAT'S JUST TOO MANY DATES! AAAAH!!! (max: 20)");
                return;
            }

            // Build response

            var response = "";
            for (int i = 0; i < dates.Count; i++)
            {
                response += Resources.EmojiStrings[i] + " ";
                response += dates[i].ToShortDateString() + "\n";
            }

            // Respond

            await command.RespondAsync(response);
            var message = await command.GetOriginalResponseAsync();
            for (int i = 0; i < dates.Count; i++)
            {
                await message.AddReactionAsync(Resources.Emojis[i]);
            }
        }

        private static List<DateOnly>? ParseDateString(string dateString)
        {
            var singleDateStrings = dateString.Split(",");
            List<DateOnly> dates = new();
            var currentYear = DateTime.Now.Year;

            foreach (var singleDateString in singleDateStrings)
            {
                if (!(singleDateString.Length == 4 || singleDateString.Length == 6)) return null;

                var year = currentYear;

                var parseableDay = int.TryParse(singleDateString.Substring(0, 2), out var day);
                var parseableMonth = int.TryParse(singleDateString.Substring(2, 2), out var month);
                var parseableYear = true;
                if (singleDateString.Length == 6) 
                {
                    parseableYear = int.TryParse(singleDateString.Substring(4, 2), out year);
                    year += 2000;
                }
                
                if (!parseableDay || !parseableMonth || !parseableYear) return null;

                dates.Add(new DateOnly(year, month, day));
            }

            return dates;
        }

        private static async Task RespondToPoll(SocketSlashCommand command)
        {
            // Get options

            var options = command.Data.Options.ToList();
            var dateString = options[0].Value as string;
            if (!DateOnly.TryParse(dateString, out var date))
            {
                await command.RespondAsync("The date has the wrong format. Sorry. It should look like \"YYYY-MM-DD\"");
                return;
            }
            long days = (long)options[1].Value;

            // Build response

            var response = "";
            for (int i = 0; i < days; i++)
            {
                response += Resources.EmojiStrings[i] + " ";
                response += date.ToShortDateString() + "\n";
                date = date.AddDays(1);
            }

            // Respond

            await command.RespondAsync(response);
            var message = await command.GetOriginalResponseAsync();
            for (int i = 0; i < days; i++)
            {
                await message.AddReactionAsync(Resources.Emojis[i]);
            }
        }

        private static async Task RespondToPing(SocketSlashCommand command)
        {
            await command.RespondAsync("Pong! :3");
        }

        private static async Task RespondToRoll(SocketSlashCommand command)
        {
            // Get the dice roll data

            var rollString = (string)command.Data.Options.First();
            var dice = Dice.ParseDiceString(rollString);

            if (dice is null)
            {
                await command.RespondAsync("Oh oh! Those dice don't look right!");
                return;
            }

            // Build response 

            var rolls = dice.Roll();

            var response = "";
            response += $"You rolled {dice.Number}d{dice.Sides}+{dice.Modifier}\n";
            if (dice.Modifier < 0) response.Replace("+", "");
            response += $"Your rolls:";
            for (int i = 1; i < rolls.Count; i++)
            {
                response += $" {rolls[i]}";
            }
            response += $"\n**Total: {rolls[0]}**";

            // Respond

            await command.RespondAsync(response);
        }
    }
}
