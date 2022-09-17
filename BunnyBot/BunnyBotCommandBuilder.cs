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
