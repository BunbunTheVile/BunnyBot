using BunnyBot;
using Discord;
using Discord.WebSocket;
using System.Text.Json;

public class Program
{
    private static Config? config;
    private static DiscordSocketClient client = new DiscordSocketClient();

    private static string[] emojis = new string[]
    {
        "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣", "6️⃣", "7️⃣", "8️⃣" ,"9️⃣",
        "🍏", "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓","🍒", "🥝"
    };

    public static async Task Main(string[] args)
    {
        var configString = File.ReadAllText("config.json");
        config = JsonSerializer.Deserialize<Config>(configString);
        if (config is null) throw new NullReferenceException("Config is null!");

        client.Log += Log;
        client.Ready += PrepareCommands;
        client.SlashCommandExecuted += HandleSlashCommand;

        await client.LoginAsync(TokenType.Bot, config.Token);
        await client.StartAsync();

        await Task.Delay(-1);
    }

    private static async Task HandleSlashCommand(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "ping": await RespondToPing(command);
                break;
            case "roll": await RespondToRoll(command);
                break;
            case "poll": await RespondToPoll(command);
                break;
        }
    }

    private static async Task RespondToPoll(SocketSlashCommand command)
    {
        var options = command.Data.Options.ToList();
        var dateString = options[0].Value as string;
        if (!DateOnly.TryParse(dateString, out var date))
        {
            await command.RespondAsync("The date has the wrong format. Sorry. It should look like \"YYYY-MM-DD\"");
            return;
        }

        long days = (long)options[1].Value;

        var response = "";
        for (int i = 0; i < days; i++)
        {
            response += emojis[i] + " ";
            response += date.ToShortDateString() + "\n";
            date = date.AddDays(1);
        }

        await command.RespondAsync(response);
        var message = await command.GetOriginalResponseAsync();
        for (int i = 0; i < days; i++)
        {
            await message.AddReactionAsync(new Emoji(emojis[i]));
        }
    }

    private static async Task RespondToPing(SocketSlashCommand command)
    {
        await command.RespondAsync("Pong! :3");
    }

    private static async Task RespondToRoll(SocketSlashCommand command)
    {
        var rollString = (string)command.Data.Options.First();
        var dice = Dice.ParseDiceString(rollString);
        
        if (dice is null)
        {
            await command.RespondAsync("Oh oh! Those dice don't look right!");
            return;
        }

        var response = "";
        response += $"You rolled {dice.Number}d{dice.Sides}+{dice.Modifier}\n";
        if (dice.Modifier < 0) response.Replace("+", "");
        response += $"Result: {dice.Roll()}";

        await command.RespondAsync(response);
    }

    private static async Task PrepareCommands()
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

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}