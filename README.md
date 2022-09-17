# BunnyBot

## Usage
Every now and again I'm going to place a compiled version (".exe" file) in the releases for this repo. It will be compiled for x86 processors on windows. Running the bot is as simple as double-clicking the ".exe" file. Before starting the bot, don't forget to add a "config.json" file to the same directory as the ".exe" file. It should contain your bot's token and the guild ID of the server it is supposed to connect to.

Its content should look like this:
```
{
  "Token": "MyBotToken",
  "GuildId": 123456789
}
```

Keep in mind that the Token is a string, while the GuildId is a number (specifically a long).

## Commands
```
/ping
```
Simply responds Pong if the bot is active.

---

```
/roll
```
Rolls some dice for you.

---

```
/poll
```
Creates a poll with emoji reactions for up to 20 consecutive days.

---

```
/specific-poll
```
Creates a poll with emoji reactions for up to 20 specific dates.
