using Discord;
using Discord.WebSocket;
using System.Text.Json;

namespace BunnyBot
{
    public class BunnyBotCommandBuilder
    {
        private Config config;
        private DiscordSocketClient client;

        public BunnyBotCommandBuilder(Config config, DiscordSocketClient client)
        {
            this.config = config;
            this.client = client;
        }

        public async Task PrepareCommands()
        {
            var guild = client.GetGuild(config!.GuildId);
            var commands = new List<SlashCommandProperties>();

            var pingCommand = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Says pong if online.")
                .Build();
            commands.Add(pingCommand);

            var rollCommand = new SlashCommandBuilder()
                .WithName("roll")
                .WithDescription("Rolls a bunch of dice for you.")
                .AddOption(
                    "dice",
                    ApplicationCommandOptionType.String,
                    "Example: 4d12+5",
                    isRequired: true)
                .Build();
            commands.Add(rollCommand);

            var pollCommand = new SlashCommandBuilder()
                .WithName("poll")
                .WithDescription("Create a poll for up to 20 successive dates.")
                .AddOption(
                    "start-date",
                    ApplicationCommandOptionType.String,
                    "A date string in ISO format. Example: 2020-06-24 for the 24th of June 2020",
                    isRequired: true)
                .AddOption(
                    "days",
                    ApplicationCommandOptionType.Integer,
                    "The number of successive days to be part of the poll. Max number is 20.",
                    isRequired: true,
                    maxValue: 20)
                .Build();
            commands.Add(pollCommand);

            var specificPollCommand = new SlashCommandBuilder()
                .WithName("specific-poll")
                .WithDescription("Specify up to 20 dates for which to create a poll.")
                .AddOption(
                    "dates",
                    ApplicationCommandOptionType.String,
                    "A comma-separated list of dates in the format 'DDMM' or 'DDMMYY'. Example: 2412,2712,301222,010123",
                    isRequired: true)
                .Build();
            commands.Add(specificPollCommand);

            var noteCommand = new SlashCommandBuilder()
                .WithName("note")
                .WithDescription("Make a note so I can remember it for you!")
                .AddOption(
                    "content",
                    ApplicationCommandOptionType.String,
                    "The content of your note.",
                    isRequired: true)
                .Build();
            commands.Add(noteCommand);

            var rememberCommand = new SlashCommandBuilder()
                .WithName("remember")
                .WithDescription("Let me see if I can remember something like that.")
                .AddOption(
                    "keyword",
                    ApplicationCommandOptionType.String,
                    "A keyword to look for in my notes.",
                    isRequired: true)
                .Build();
            commands.Add(rememberCommand);

            var forgetCommand = new SlashCommandBuilder()
                .WithName("forget")
                .WithDescription("Tell me which note to forget.")
                .AddOption(
                    "id",
                    ApplicationCommandOptionType.String,
                    "The ID of the note I should forget",
                    isRequired: true)
                .Build();
            commands.Add(forgetCommand);

            try
            {
                await guild.BulkOverwriteApplicationCommandAsync(commands.ToArray());
            }
            catch (Exception exception)
            {
                var error = JsonSerializer.Serialize(exception);
                Console.Error.WriteLine(error);
            }
        }
    }
}
