using BunnyBot;
using Discord;
using Discord.WebSocket;
using System.Text.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Declare variables
        
        DiscordSocketClient client = new DiscordSocketClient();
        Config? config;
        BunnyBotCommandBuilder commandBuilder;

        // Initialize variables

        var configString = File.ReadAllText("config.json");
        config = JsonSerializer.Deserialize<Config>(configString);
        if (config is null) throw new NullReferenceException("Config is null! Please make sure you have prepared the config.json file!");

        commandBuilder = new(config, client);

        // Prepare client

        client.Log += Log;
        client.Ready += commandBuilder.PrepareCommands;
        client.SlashCommandExecuted += BunnyBotCommandHandler.HandleSlashCommand;

        // Begin

        await client.LoginAsync(TokenType.Bot, config.Token);
        await client.StartAsync();
        await Task.Delay(-1);
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}