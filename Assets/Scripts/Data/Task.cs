using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "LD49/Task")]
public class Task : ScriptableObject
{
    public int HouseRequirement = 0;
    public int LumberyardRequirement = 0;
    public int FishingHutRequirement = 0;
    public int MineRequirement = 0;

    public int TimeToComplete = 20;

    public int TotalBuildingsToPlace()
    {
        return HouseRequirement + LumberyardRequirement + FishingHutRequirement + MineRequirement;
    }

    public override string ToString()
    {
        return $"Houses: {HouseRequirement}\nLumberyards: {LumberyardRequirement}\nFishing Huts: {FishingHutRequirement}\nMines: {MineRequirement}";
    }

    public static Task RandomTask()
    {
        Task task = new Task();
        int numToPlace = Random.Range(1, 5);        //This is approximate and really represents the minimum as every building has a chance to be added once per below
        task.HouseRequirement = Random.Range(0, 2);
        task.LumberyardRequirement = Random.Range(0, 2);
        task.FishingHutRequirement = Random.Range(0, 2);
        task.MineRequirement = Random.Range(0, 2);

        if(numToPlace > task.TotalBuildingsToPlace())
        {
            for (int i = 0; i < task.TotalBuildingsToPlace() - numToPlace; i++)
            {
                int randomNum = Random.Range(0, 5);
                Building.BuildingType buildingType = (Building.BuildingType)randomNum;
                switch (buildingType)
                {
                    case Building.BuildingType.HOUSE:
                        task.HouseRequirement++;
                        break;
                    case Building.BuildingType.LUMBER_YARD:
                        task.LumberyardRequirement++;
                        break;
                    case Building.BuildingType.FISHING_HUT:
                        task.FishingHutRequirement++;
                        break;
                    case Building.BuildingType.MINE:
                        task.MineRequirement++;
                        break;
                    default:
                        Debug.LogError("Building type not supported: " + buildingType + ". From " + randomNum);
                        break;
                }
            }
        }

        //If we somehow still have no buildings (random luck for first part, something going wrong in switch statement...
        if (task.TotalBuildingsToPlace() < 1)
        {
            Debug.LogWarning("RandomTask was not filled out properlly");
            task.HouseRequirement++;
        }
        
        return task;
    }
}
