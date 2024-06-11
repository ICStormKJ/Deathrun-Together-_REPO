using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData: MonoBehaviour 
{
    //INFO TO STORE
    public string displayName;
    public Rank rank;
    public int elo;
    public Power runnerPower;
    public Power trapPower;

    //SETTER METHODS
    public void UpdateDisplayName(string d) { 
        displayName = d;
        Debug.Log("Saved name as " + displayName);
    }
    public void UpdateElo(int e){
        elo = e;
        Debug.Log("Your new elo is " + elo);
    }
    public void UpdateRank(Rank r)
    {
        rank = r;
        Debug.Log("Your new rank is: " + r);
    }
    public void saveRunner(Power p) {
        runnerPower = p;
        Debug.Log("saved runner power");
    }
    public void saveTrap(Power p) {
        trapPower = p;
        Debug.Log("saved trapmaster power");
    }
    //----------Used for loading back into game build----------
    public void LoadDataBack(PlayerData d) 
    { 
        if (d == null) { return; }
        displayName = d.displayName;
        rank = d.rank;
        runnerPower = d.runnerPower;
        trapPower = d.trapPower;
    }

    //----------Getters for powers used in equip loading----------
    public Power GetRunnerPower() { return runnerPower; }
    public Power GetTrapPower() {  return trapPower; }
    

}
