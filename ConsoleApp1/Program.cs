// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using FortniteReplayReader;
using FortniteReplayReader.Models;

Console.WriteLine("Hello, World!");
var replayFile = "C:\\Users\\EloiStree\\Desktop\\FortniteReplay\\UnsavedReplay-2025.08.10-15.13.20.replay";
var reader = new ReplayReader(parseMode: Unreal.Core.Models.Enums.ParseMode.Full);
FortniteReplay replay = reader.ReadReplay(replayFile);

if (replay != null)
{

    foreach (var playerData in replay.PlayerData) { 
    
        Console.WriteLine($"{playerData.PlayerName}:{playerData.PlayerId}");
    }
}
else
{
    Console.WriteLine("Failed to read replay file.");
}


while (true) { 

    Thread.Sleep(1000);
}