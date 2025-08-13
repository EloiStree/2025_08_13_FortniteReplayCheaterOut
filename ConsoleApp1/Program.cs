// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using ConsoleApp1;
using FortniteReplayReader;
using FortniteReplayReader.Models;
using Unreal.Core.Attributes;
using Unreal.Core.Models;




Console.WriteLine("Hello, World!");
var replayFile = "C:\\Users\\EloiStree\\Desktop\\FortniteReplay\\UnsavedReplay-2025.08.10-15.13.20.replay";
var reader = new ReplayReader(parseMode: Unreal.Core.Models.Enums.ParseMode.Full);
FortniteReplay replay = reader.ReadReplay(replayFile);




if (replay != null)
{

    StringBuilder mePath = new StringBuilder();
    StringBuilder allPathPosition = new StringBuilder();

    Dictionary<string, DeathBy> m_playerKilled = new Dictionary<string, DeathBy>();

    foreach (var kill in replay.KillFeed)
    {

        Console.WriteLine($"K:{kill.FinisherOrDownerName}->{kill.PlayerName} ({kill.Distance})" );
        DeathBy deathBy = new DeathBy();
        deathBy.m_killedPosition = kill.DeathLocation;
        deathBy.m_killerEpicIdName = kill.FinisherOrDownerName ?? string.Empty;
        deathBy.m_killedEpicIdName = kill.PlayerName ?? string.Empty;
        deathBy.m_killerDistance = kill.Distance ?? 0f;
        if (!m_playerKilled.ContainsKey(deathBy.m_killedEpicIdName)) { 
            m_playerKilled.Add(deathBy.m_killedEpicIdName, deathBy);
        }

    }
    foreach (var playerData in replay.PlayerData) {

        allPathPosition.AppendLine("___NEW___");
        allPathPosition.AppendLine("PLAYER_ID:" + playerData.EpicId);
        allPathPosition.AppendLine("PLAYER_NAME:" + playerData.PlayerName);
        allPathPosition.AppendLine("PLAYER_KILLS:" + playerData.Kills);
        allPathPosition.AppendLine("PLAYER_TOP:" + playerData.Placement);
        var death = playerData.DeathLocation;
        allPathPosition.AppendLine($"PLAYER_DEATH_POSITION:{death.X} {death.Y} {death.Z}");
        allPathPosition.AppendLine($"PLAYER_DEATH_TIME:{playerData.DeathTimeDouble}");
        if (m_playerKilled.ContainsKey(playerData.EpicId ?? ""))
        {
            DeathBy d = m_playerKilled[playerData.EpicId ?? ""];
            allPathPosition.AppendLine("PLAYER_KILLED_BY:" + d.m_killerEpicId);
            allPathPosition.AppendLine("PLAYER_KILLER_DISTANCE:" + d.m_killerDistance);
        }


        if (playerData.PlayerId?.Trim() == "FA9B8CBF946949E294A1CC322B0F814D") {
            mePath .Append("T");
            string jsonOfCosmetic = JsonSerializer.Serialize(playerData.Cosmetics);
            mePath.AppendLine("Cosmetic:");
            mePath.AppendLine(jsonOfCosmetic);
            mePath.AppendLine("Locations count:");
            mePath.AppendLine(playerData.Locations.Count.ToString());
            mePath.AppendLine("Path:");
            foreach (var playerMovement in playerData.Locations) {
                FVector location = playerMovement.ReplicatedMovement.Value.Location;
                FRotator rotation = playerMovement.ReplicatedMovement.Value.Rotation;
                mePath.AppendLine($"{playerMovement.LastUpdateTime}: Position=({location.X}, {location.Y}, {location.Z}), Rotation=({rotation.Pitch}, {rotation.Yaw}, {rotation.Roll})");
                allPathPosition.AppendLine($"{playerMovement.LastUpdateTime} {location.X} {location.Y} {location.Z} {rotation.Pitch} {rotation.Yaw} {rotation.Roll}");
            }
           

            
        }
        Console.WriteLine($"{playerData.PlayerName} ({playerData.Platform}) _ {playerData.PlayerId} _ {playerData.EpicId} ," +
            $"  {playerData.Cosmetics.Character}  , " +
            $"  K:{playerData.Kills }, " +
            $"  S:{playerData.IsUsingStreamerMode == true}, " +
            $"  A:{playerData.IsUsingAnonymousMode==true}, " +
            $"  B:{playerData.IsBot == true }, " +
            $"  R:{playerData.IsReplayOwner == true }, " +
        $"  Top:{playerData.Placement}, "+
            $"  Death Location:{playerData.DeathLocation}, " +
            $"  Death Time:{playerData.DeathTimeDouble}, "
            );
        
    }
    Console.WriteLine(mePath);


    string directory = Directory.GetCurrentDirectory();
    string file = Path.Combine(directory, Path.GetFileNameWithoutExtension(replayFile));
    Console.Write(file);
    File.WriteAllText(file, allPathPosition.ToString());




}
else
{
    Console.WriteLine("Failed to read replay file.");
}




//while (true) { 

//    Thread.Sleep(1000);
//}