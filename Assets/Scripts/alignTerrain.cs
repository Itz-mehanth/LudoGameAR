using UnityEngine;

public class alignTerrain : MonoBehaviour
{

    // Reference to your Ludo board and terrain
    public GameObject ludoBoard;
    public Terrain terrain;

    void AlignTerrainWithLudoBoard()
    {
        // Get the position of the Ludo board
        Vector3 ludoBoardPosition = ludoBoard.transform.position;
        Quaternion ludoBoardRotation = ludoBoard.transform.rotation;

        
        // Get the terrain size to compute the offset (since terrain origin might be at one corner)
        Vector3 terrainSize = terrain.terrainData.size;
        
        // Calculate offset to center the terrain with the Ludo board
        Vector3 terrainOffset = new Vector3(terrainSize.x / 2, 0, terrainSize.z / 2);
        
        // If the terrain is still slightly above or below, add an offset to the Y-axis
        // For example, if terrain is too high, you can subtract from the Y position:
        terrain.transform.position = new Vector3(
            ludoBoardPosition.x - terrainOffset.x,
            ludoBoardPosition.y - 0.5f,  // Adjust here if needed
            ludoBoardPosition.z - terrainOffset.z
        );

        terrain.transform.rotation = ludoBoardRotation;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlignTerrainWithLudoBoard();
    }

    // Update is called once per frame
    void Update()
    {
        AlignTerrainWithLudoBoard();
    }
}
