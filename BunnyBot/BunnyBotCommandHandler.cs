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
            }
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

            var response = "";
            response += $"You rolled {dice.Number}d{dice.Sides}+{dice.Modifier}\n";
            if (dice.Modifier < 0) response.Replace("+", "");
            response += $"Result: {dice.Roll()}";

            // Respond

            await command.RespondAsync(response);
        }
    }
}
