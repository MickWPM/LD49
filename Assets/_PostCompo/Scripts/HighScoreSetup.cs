using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreSetup : MonoBehaviour
{
    public static readonly string API_URL = "https://www.highscores.ovh/api/highscore";

    [SerializeField] private TextAsset starterNormalSecret;
    [SerializeField] private TextAsset starterHardcoreSecret;
    [SerializeField] private TextAsset starterZenSecret;
    [SerializeField] private TextAsset starterHardcoreZenSecret;

    [SerializeField] private TextAsset MaugNormalSecret;
    [SerializeField] private TextAsset MaugHardcoreSecret;
    [SerializeField] private TextAsset MaugZenSecret;
    [SerializeField] private TextAsset MaugHardcoreZenSecret;

    [SerializeField] private TextAsset shatteredNormalSecret;
    [SerializeField] private TextAsset shatteredHardcoreSecret;
    [SerializeField] private TextAsset shatteredZenSecret;
    [SerializeField] private TextAsset shatteredHardcoreZenSecret;

    public static Dictionary<IslandGameKey, string> AppIds;
    public static Dictionary<IslandGameKey, string> AppSecrets;
    
    private void Awake()
    {
        if(PlayerPrefs.HasKey("Guid") == false)
        {
            PlayerPrefs.SetString("Guid", System.Guid.NewGuid().ToString());
            PlayerPrefs.Save();
        }
        
        PopulateDictionaries();
    }

    [System.Serializable]
    public struct IslandGameKey
    {
        public GameOptionsPersistent.GameMode gameMode;
        public ShowHighScores.MapType mapType;
    }

    public static string GetGameModeIslandIDForHighScoreNAMEPlayerPrefs(IslandGameKey islandGameKey)
    {
        return GetGameModeIslandIDForHighScorePlayerPrefs(islandGameKey.mapType, islandGameKey.gameMode) + "-NAME";
    }

    public static string GetGameModeIslandIDForHighScoreNAMEPlayerPrefs(ShowHighScores.MapType mapType, GameOptionsPersistent.GameMode gameMode)
    {
        return GetGameModeIslandIDForHighScorePlayerPrefs(mapType, gameMode) + "-NAME";
    }

    public static string GetGameModeIslandIDForHighScorePlayerPrefs(IslandGameKey islandGameKey)
    {
        return GetGameModeIslandIDForHighScorePlayerPrefs(islandGameKey.mapType, islandGameKey.gameMode);
    }

    public static string GetGameModeIslandIDForHighScorePlayerPrefs(ShowHighScores.MapType mapType, GameOptionsPersistent.GameMode gameMode)
    {
        int index = 0;

        switch (gameMode)
        {
            case GameOptionsPersistent.GameMode.NORMAL:
                index += 1;
                break;
            case GameOptionsPersistent.GameMode.HARDCORE:
                index += 2;
                break;
            case GameOptionsPersistent.GameMode.ZEN:
                index += 3;
                break;
            case GameOptionsPersistent.GameMode.HARDCORE_ZEN:
                index += 4;
                break;
            default:
                break;
        }

        switch (mapType)
        {
            case ShowHighScores.MapType.STARTER:
                index += 100;
                break;
            case ShowHighScores.MapType.MAUG:
                index += 200;
                break;
            case ShowHighScores.MapType.SHATTERED:
                index += 300;
                break;
            default:
                break;
        }

        return index.ToString();
    }

    void PopulateDictionaries()
    {
        #region AppIds
        AppIds = AppIds = new System.Collections.Generic.Dictionary<IslandGameKey, string>();
        //Starter
        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.STARTER }, "a33cdee5-20d4-41cb-83c5-f619f0fa0485");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.STARTER }, "ffb10de7-e6ac-44d2-ba72-84e46e7bc668");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.STARTER }, "938136af-7acd-450f-839e-63b1109fb135");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.STARTER }, "544faa42-7abf-4655-926e-57c9c1f2d42e");

        //Maug
        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.MAUG }, "69aee78e-1129-49a5-b9a6-ae8b73976a20");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.MAUG }, "9efe8384-e201-4c83-bc8d-683d157274bd");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.MAUG }, "16c10494-1707-413d-994f-a6a6db0f39fa");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.MAUG }, "f5a6a200-75e1-4938-af87-584891297a6e");

        //Shattered
        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.SHATTERED }, "32359b90-d1dd-4b24-84e3-7347eb9d299b");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.SHATTERED }, "1cd093a2-4c64-420b-9567-ee7914563a88");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "731ebdaa-2073-4c96-9085-85cc58da13c0");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "b8d33770-7a2e-49f6-8bd2-faa81bcef54f");
        #endregion

        #region secrets

        AppSecrets = new System.Collections.Generic.Dictionary<IslandGameKey, string>();
        //Starter
        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.STARTER }, starterNormalSecret.text);

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.STARTER }, starterHardcoreSecret.text);

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.STARTER }, starterZenSecret.text);

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.STARTER }, starterHardcoreZenSecret.text);

        //Maug
        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.MAUG }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.MAUG }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.MAUG }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.MAUG }, "");

        //Shattered
        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppSecrets.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "");
        #endregion

    }
}
