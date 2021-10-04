using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public string appSecretNormal = "";//"20
    public string appSecretHardcore = "";//"
    public string appSecretZen = "";//"24a1f
    public string appSecretHardcoreZen = "";

    // Start is called before the first frame update
    void Start()
    {
        appSecretNormal = HighScoreSetup.appSecretNormal;
        appSecretHardcore = HighScoreSetup.appSecretHardcore;
        appSecretZen = HighScoreSetup.appSecretZen;
        appSecretHardcoreZen = HighScoreSetup.appSecretHardcoreZen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
