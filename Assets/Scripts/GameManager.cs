using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TerrainManager terrainManager;
    public SimManager simManager;
    public FireTruckManager fireTruckManager;

    public enum STATE
    {
        PLAYER_TURN,
        FIRE_SIMULATION,
    }
    private STATE currentSTate;

    private int turnNumber = 0;

    private void Awake()
    {
        GameManager.instance = this;
    }

    private void Start()
    {
        this.terrainManager.Initiaiise();
    }

    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 0.0f, 0.0f, Screen.width, Screen.height / 8.0f ), "Next Turn" ) )
        {
            this.IncrementTurn();
        }
    }

    private void IncrementTurn()
    {
        this.simManager.RunEnvironmentSimulation();
        ++this.turnNumber;
    }
}
