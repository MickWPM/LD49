using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreSetup : MonoBehaviour
{
    [SerializeField] private TextAsset starterNormalSecret;
    [SerializeField] private TextAsset starterHardcoreSecret;
    [SerializeField] private TextAsset starterZenSecret;
    [SerializeField] private TextAsset starterHardcoreZenSecret;

    public static Dictionary<IslandGameKey, string> AppIds;
    public static Dictionary<IslandGameKey, string> AppSecrets;

    //public static string appIDNormalStarter = "a33cdee5-20d4-41cb-83c5-f619f0fa0485";
    //public static string appIDHardcoreStarter = "ffb10de7-e6ac-44d2-ba72-84e46e7bc668";
    //public static string appIDZen = "938136af-7acd-450f-839e-63b1109fb135";
    //public static string appIDHardcoreZen = "544faa42-7abf-4655-926e-57c9c1f2d42e";

    private void Awake()
    {
        PopulateDictionaries();
    }

    private void Start()
    {
        //PLAYER PREFS SETUP
    }

    [System.Serializable]
    public struct IslandGameKey
    {
        public GameOptionsPersistent.GameMode gameMode;
        public ShowHighScores.MapType mapType;
    }

    //public static string appSecretNormal = "";//"200dc6aa-a0e2-4080-a35e-df20a9d04bd5";
    //public static string appSecretHardcore = "";//"b8e9df88-2b21-4280-99cd-d62e1acac36a";
    //public static string appSecretZen = "";//"24a1f794-f6c6-428b-8d60-ee310ae54559";
    //public static string appSecretHardcoreZen = "";//"e3dd978c-940f-4386-bc6d-56c3e88b017e";

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
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.MAUG }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.MAUG }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.MAUG }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.MAUG }, "");

        //Shattered
        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.NORMAL, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "");

        AppIds.Add(
            new IslandGameKey() { gameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN, mapType = ShowHighScores.MapType.SHATTERED }, "");
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
