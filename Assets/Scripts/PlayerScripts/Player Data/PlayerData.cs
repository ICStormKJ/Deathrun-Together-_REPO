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
        Debug.Log("Saved name as " + displayName); 
        displayName = d; }
    public void UpdateElo(int e){
        Debug.Log("Your new elo is " + elo);
        elo = e; }
    public void UpdateRank(Rank r)
    {
        Debug.Log("Your new rank is: " + r);
        rank = r;
    }
    public void saveRunner(Power p) {
        Debug.Log("saved runner power");
        runnerPower = p; }
    public void saveTrap(Power p) {
        Debug.Log("saved trapmaster power");
        trapPower = p; }
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
