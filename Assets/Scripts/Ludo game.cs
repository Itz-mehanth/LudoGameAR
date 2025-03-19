using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LudoGame : MonoBehaviour
{

    public GameObject dice1; // Assign this in the Inspector or dynamically in the script
    public GameObject dice2; // Assign this in the Inspector or dynamically in the script

    int currentPlayer = 0; // 0 = red, 1 = green, 2 = blue, 3 = yellow

    public TextMeshProUGUI currentPlayerStatus;

    public Button coin1Button;  // Button for Coin 1
    public Button coin2Button;  // Button for Coin 2
    public Button coin3Button;  // Button for Coin 3
    public Button coin4Button;  // Button for Coin 4

    private int die1Res;
    private int die2Res;

    private int selectedCoinIndex = -1; // To store the selected coin index

    private List<bool> redInitialized = Enumerable.Repeat(false, 4).ToList();
    private List<bool> yellowInitialized = Enumerable.Repeat(false, 4).ToList();
    private List<bool> greenInitialized = Enumerable.Repeat(false, 4).ToList();
    private List<bool> blueInitialized = Enumerable.Repeat(false, 4).ToList();

    private List<bool> redFinished = Enumerable.Repeat(false, 4).ToList();
    private List<bool> yellowFinished = Enumerable.Repeat(false, 4).ToList();
    private List<bool> greenFinished = Enumerable.Repeat(false, 4).ToList();
    private List<bool> blueFinished = Enumerable.Repeat(false, 4).ToList();

    private SamuraiAnimationController coin1Controller;
    public GameObject redcoin1;
    public GameObject redcoin2;
    public GameObject redcoin3;
    public GameObject redcoin4;
    
    public GameObject greencoin1;
    public GameObject greencoin2;
    public GameObject greencoin3;
    public GameObject greencoin4;
    
    public GameObject yellowcoin1;
    public GameObject yellowcoin2;
    public GameObject yellowcoin3;
    public GameObject yellowcoin4;
    
    public GameObject bluecoin1;
    public GameObject bluecoin2;
    public GameObject bluecoin3;
    public GameObject bluecoin4;

    private List<GameObject> redCoins = new List<GameObject>(4);
    private List<GameObject> yellowCoins = new List<GameObject>(4);
    private List<GameObject> blueCoins = new List<GameObject>(4);
    private List<GameObject> greenCoins = new List<GameObject>(4);

    private List<int> redCoinPositions = Enumerable.Repeat(0, 4).ToList();
    private List<int> yellowCoinPositions = Enumerable.Repeat(0, 4).ToList();
    private List<int> greenCoinPositions = Enumerable.Repeat(0, 4).ToList();
    private List<int> blueCoinPositions = Enumerable.Repeat(0, 4).ToList();


    public Button rollButton;  // Reference to the button

    public TextMeshProUGUI res1;    // Reference to the text UI to display the result
    public TextMeshProUGUI res2;    // Reference to the text UI to display the result
   
    // Dictionary to hold tile data
    private Dictionary<string, GameObject> tileDictionary;

    private int randomNumber = -1;

    private List<string> redPath;
    private List<string> yellowPath;
    private List<string> bluePath;
    private List<string> greenPath;

    void disableButton(){
        coin1Button.gameObject.SetActive(true);
        coin2Button.gameObject.SetActive(true);
        coin3Button.gameObject.SetActive(true);
        coin4Button.gameObject.SetActive(true);
        rollButton.gameObject.SetActive(false);
    }

    void enableButton(){
        coin1Button.gameObject.SetActive(false);
        coin2Button.gameObject.SetActive(false);
        coin3Button.gameObject.SetActive(false);
        coin4Button.gameObject.SetActive(false);
        rollButton.gameObject.SetActive(true);
    }

    bool isPlayerInitialized(){
        List<bool> coinsInitialized;
        switch (currentPlayer)
        {
            case 0:
                coinsInitialized = redInitialized;
                break;
            case 1:
                coinsInitialized = greenInitialized;
                break;

            case 2:
                coinsInitialized = blueInitialized;
                break;

            case 3:
                coinsInitialized = yellowInitialized;
                break;

            default:
                coinsInitialized = Enumerable.Repeat(false, 4).ToList();
                break;
        }
        return coinsInitialized[0] || coinsInitialized[1] || coinsInitialized[2] || coinsInitialized[3];
    }
    // Method to calculate the dice face based on its rotation

    async void OnRollButtonClick()
    {

        DiceRoller dice1Roller = dice1.GetComponent<DiceRoller>();
        DiceRoller dice2Roller = dice2.GetComponent<DiceRoller>();
        if (dice1Roller == null)
        {
            dice1Roller = dice1.AddComponent<DiceRoller>();
        }

        if (dice2Roller == null)
        {
            dice2Roller = dice1.AddComponent<DiceRoller>();
        }

        // Now you can safely call RollDice
        dice1Roller.RollDice();
        dice2Roller.RollDice();

        await Task.Delay((int)(7f * 1000));

        if (res1.text == "Invalid" || res2.text == "Invalid")
        {
            return;
        }

        char lastChar1 = res1.text[res1.text.Length - 1]; // Get the last character
        int die1Res = int.Parse(lastChar1.ToString());

        char lastChar2 = res2.text[res2.text.Length - 1]; // Get the last character
        int die2Res = int.Parse(lastChar2.ToString());

        randomNumber = (die1Res != -1 && die2Res != -1) ? die1Res + die2Res : -1;

        Debug.Log($"The random number is: {randomNumber}");

        if (randomNumber != -1 && isPlayerInitialized() || randomNumber == 1)
        {
            disableButton();
        }

        if (!isPlayerInitialized() && randomNumber != 1)
        {
            Debug.Log("Random number is not 1 and player has no active coins");
            currentPlayer = (currentPlayer + 1) % 4; // Loop between 0 and 3
            if(currentPlayer == 0){
                currentPlayerStatus.text = $"Player: Red";
            }else if(currentPlayer == 1){ 
                currentPlayerStatus.text = $"Player: Green";
            }
            else if(currentPlayer == 2){
                currentPlayerStatus.text = $"Player: Blue";
            }
            else if(currentPlayer == 3){
                currentPlayerStatus.text = $"Player: Yellow";
            }
        }
        
    }

    bool IsSafeZone(string position)
    {
        // Define the positions that are considered safe zones
        List<string> safeZones = new List<string> {"43","1","15","29","8","22","36","50"}; // Example positions
        return safeZones.Contains(position);
    }


    void SendCoinHome(ref List<int> coinPositions, List<GameObject> coins, int coinIndex,ref List<bool> initialized)
    {
        // Reset coin position
        coinPositions[coinIndex] = 0;
        initialized[coinIndex] = false;

        // Trigger any animation for the coin being sent home
        // SamuraiAnimationController coinController = coins[coinIndex].GetComponent<SamuraiAnimationController>();
        // coinController.isSwinging = true; // Assuming this animation is the "going home" one
    }
    
    void CheckCollision(int playerIndex, int newCoinPosition)
    {
        // Loop through each player's coin positions to see if anyone is standing on the same square
        for (int i = 0; i < 4; i++) // Assume 4 players
        {
            if (i == playerIndex)
                continue; // Skip checking against the current player's coins

            // Now check each coin of the other players
            switch (i)
            {
                case 0: // Red Player
                    for (int j = 0; j < redCoinPositions.Count; j++)
                    {
                        if (redCoinPositions[j] == newCoinPosition && !IsSafeZone(redPath[newCoinPosition])) 
                        {
                            SendCoinHome(ref redCoinPositions, redCoins, j,ref redInitialized);
                        }
                    }
                    break;
                case 1: // Green Player
                    for (int j = 0; j < greenCoinPositions.Count; j++)
                    {
                        if (greenCoinPositions[j] == newCoinPosition && !IsSafeZone(greenPath[newCoinPosition])) 
                        {
                            SendCoinHome(ref greenCoinPositions, greenCoins, j,ref greenInitialized);
                        }
                    }
                    break;
                case 2: // Blue Player
                    for (int j = 0; j < blueCoinPositions.Count; j++)
                    {
                        if (blueCoinPositions[j] == newCoinPosition && !IsSafeZone(bluePath[newCoinPosition])) 
                        {
                            SendCoinHome(ref blueCoinPositions, blueCoins, j,ref blueInitialized);
                        }
                    }
                    break;
                case 3: // Yellow Player
                    for (int j = 0; j < yellowCoinPositions.Count; j++)
                    {
                        if (yellowCoinPositions[j] == newCoinPosition && !IsSafeZone(yellowPath[newCoinPosition])) 
                        {
                            SendCoinHome(ref yellowCoinPositions, yellowCoins, j,ref yellowInitialized);
                        }
                    }
                    break;
            }
        }
    }


    async void nextTurn(int selectedCoinIndex, int randomNumber, int index) {
        // Trigger the next player's dice roll and movement
        switch (currentPlayer) {
            case 0:
                // Red player's turn
                if (redCoinPositions[index] == 0)
                {
                    redInitialized[index] = true;
                    coin1Controller = redCoins[index].GetComponent<SamuraiAnimationController>();
                    coin1Controller.isSwinging = true; //Initial swing jump
                    await Task.Delay((int)(3 * 1000));
                }
                MoveCoin(redCoins[selectedCoinIndex], redPath, ref redCoinPositions, randomNumber, selectedCoinIndex);
                CheckCollision(0, redCoinPositions[selectedCoinIndex]);

                break;
            case 1:
                if (greenCoinPositions[index] == 0)
                {
                    greenInitialized[index] = true;
                    coin1Controller = greenCoins[index].GetComponent<SamuraiAnimationController>();
                    coin1Controller.isSwinging = true; //Initial swing jump
                    await Task.Delay((int)(3 * 1000));
                }
                // Green player's turn
                MoveCoin(greenCoins[selectedCoinIndex], greenPath, ref greenCoinPositions, randomNumber, selectedCoinIndex);
                CheckCollision(1, greenCoinPositions[selectedCoinIndex]);

                break;
            case 2:
                if (yellowCoinPositions[index] == 0)
                {
                    yellowInitialized[index] = true;
                    coin1Controller = yellowCoins[index].GetComponent<SamuraiAnimationController>();
                    coin1Controller.isSwinging = true; //Initial swing jump
                    await Task.Delay((int)(3 * 1000));
                }
                // Yellow player's turn
                MoveCoin(yellowCoins[selectedCoinIndex], yellowPath, ref yellowCoinPositions, randomNumber, selectedCoinIndex);
                CheckCollision(2, yellowCoinPositions[selectedCoinIndex]);

                break;
            case 3:
                if (blueCoinPositions[index] == 0)
                {
                    blueInitialized[index] = true;
                    coin1Controller = blueCoins[index].GetComponent<SamuraiAnimationController>();
                    coin1Controller.isSwinging = true; //Initial swing jump
                    await Task.Delay((int)(3 * 1000));
                }
                // Blue player's turn
                MoveCoin(blueCoins[selectedCoinIndex], bluePath, ref blueCoinPositions, randomNumber, selectedCoinIndex);
                CheckCollision(3, blueCoinPositions[selectedCoinIndex]);

                break;
        }

        randomNumber = -1;
    }

    async void SelectCoin(int index)
    {
        selectedCoinIndex = index;
        Debug.Log("Selected Coin: " + (index + 1));

        char lastChar1 = res1.text[res1.text.Length - 1]; // Get the last character
        int die1Res = int.Parse(lastChar1.ToString());
        
        char lastChar2 = res2.text[res2.text.Length - 1]; // Get the last character
        int die2Res = int.Parse(lastChar2.ToString());

        randomNumber = (die1Res != -1 && die2Res != -1) ? die1Res + die2Res : -1;

        if(randomNumber == -1){
            return;
        }

        // Check if the selected coin is initialized (active)
        bool isCoinInitialized = false;
        switch (currentPlayer)
        {
            case 0:
                Debug.Log("current player is red");
                if (randomNumber == 1)
                {
                    if (redFinished[index])
                    {
                        Debug.Log("The Coin has already reached the Home");
                        return;
                    }
                    if(!redInitialized[index]){
                        redInitialized[index] = true;
                        Debug.Log("New red coin initialized");
                    }
                }
                isCoinInitialized = redInitialized[index];
                break;
            case 1:
                Debug.Log("current player is green");
                if (randomNumber == 1)
                {
                    if (greenFinished[index])
                    {
                        Debug.Log("The Coin has already reached the Home");
                        return;
                    }
                    if(!greenInitialized[index]){
                        greenInitialized[index] = true;
                        Debug.Log("New green coin initialized");
                    }
                }
                isCoinInitialized = greenInitialized[index];
                break;
            case 2:
                Debug.Log("current player is yellow");
                if (randomNumber == 1)
                {
                    if (yellowFinished[index])
                    {
                        Debug.Log("The Coin has already reached the Home");
                        return;
                    }
                    if(!yellowInitialized[index]){
                        yellowInitialized[index] = true;
                        Debug.Log("New yellow coin initialized");
                    }
                }
                isCoinInitialized = yellowInitialized[index];
                break;
            case 3:
                Debug.Log("current player is blue");
                if (randomNumber == 1)
                {
                    if (blueFinished[index])
                    {
                        Debug.Log("The Coin has already reached the Home");
                        return;
                    }
                    if(!blueInitialized[index]){
                        blueInitialized[index] = true;
                        Debug.Log("New blue coin initialized");
                    }
                }
                isCoinInitialized = blueInitialized[index];
                break;
        }

        // If the coin is not initialized (inactive) and the random number is not 1, show a message
        if (!isCoinInitialized && randomNumber != 1)
        {
            Debug.Log("The selected coin is inactive. Please select a different coin.");
            // You can show a message to the player in the UI here
            return;
        }

        if (die1Res == 0 && die2Res == 0)
        {
            randomNumber = 12;
        }

        Debug.Log("Dice result:" + randomNumber);

        nextTurn(selectedCoinIndex, randomNumber, index);
    }

    // Moves the coin based on the dice roll
    void MoveCoin(GameObject coin, List<string> path, ref List<int> currentPosition, int steps, int coinNum)
    {
        int newPosition = currentPosition[coinNum] + steps;
        if (newPosition > path.Count - 1)
        {
            return;
        }else if(newPosition == path.Count - 1)
        {
            switch (currentPlayer)
            {
                case 0:
                    redFinished[coinNum] = true;
                    break;
                case 1:
                    greenFinished[coinNum] = true;
                    break;
                case 2:
                    yellowFinished[coinNum] = true;
                    break;
                case 3:
                    blueFinished[coinNum] = true;
                    break;
                default:
                    break;
            }
        }
        Debug.Log("Moving Coin " + (coinNum + 1) + " from " + currentPosition[coinNum] + " to " + newPosition + " (" + path[newPosition] + ")");
        currentPosition[coinNum] = newPosition;

        // Check if the new position is within the bounds of the path
        if (newPosition < path.Count)
        {
            // Start a coroutine to move the coin step by step between currentPosition and newPosition
            StartCoroutine(MoveCoinAlongPath(coin, path, currentPosition[coinNum] - steps, newPosition));  // Fixed: Start from the correct current position
        }

        if (randomNumber != 1 && randomNumber != 12 && randomNumber != 6 && !(!isPlayerInitialized() && randomNumber != 1))
        {
            Debug.Log("Random number is not 1 and player has no active coins");
            currentPlayer = (currentPlayer + 1) % 4; // Loop between 0 and 3
            currentPlayerStatus.text = $"Player: {currentPlayer + 1}";
        }

        enableButton();
    }

    IEnumerator MoveCoinAlongPath(GameObject coin, List<string> path, int currentPosIndex, int newPosIndex)
    {
        Animator animator = coin.GetComponent<Animator>();
        SamuraiAnimationController coinController = coin.GetComponent<SamuraiAnimationController>();

        // Play the walking animation
        if (animator != null)
        {
            coinController.isSwinging = false; // Initial swing jump
            coinController.isWalking = true;   // Start walking animation
        }

        float speed = 1.0f; // Adjust speed as needed for movement
        float rotationSpeed = 1.0f; // Speed multiplier for rotation (adjust as needed)
        float timePerStep = 2.0f / speed; // Time to move between two adjacent positions

        // Loop through each position between the current position and new position
        for (int i = currentPosIndex; i < newPosIndex; i++)  // Iterate through every step in the path
        {
            string currentTileName = path[i];
            string nextTileName = path[i + 1];  // Get the next tile in the path

            if (tileDictionary.ContainsKey(currentTileName) && tileDictionary.ContainsKey(nextTileName))
            {
                Vector3 startingPosition = tileDictionary[currentTileName].transform.position;
                Vector3 targetPosition = tileDictionary[nextTileName].transform.position;

                // Calculate the direction the coin needs to face
                Vector3 directionToFace = targetPosition - startingPosition;
                Quaternion targetRotation = Quaternion.LookRotation(directionToFace); // Get the rotation to face the target

                float elapsedTime = 0f;
                Vector3 originalPosition = startingPosition;

                // Move the coin over time between the positions
                while (elapsedTime < timePerStep)
                {
                    // Move the coin's position
                    coin.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / timePerStep);

                    // Smoothly rotate the coin towards the target rotation
                    coin.transform.rotation = Quaternion.Slerp(coin.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                    elapsedTime += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Ensure the final position is exactly the target position for the current step
                coin.transform.position = targetPosition;
            }
        }

        // Stop the walking animation once the destination is reached
        if (coinController != null)
        {
            coinController.isWalking = false; // Stop walking animation
            coinController.isStanding = true;
        }
    }


    // Initializes the path for each player
    void InitializePaths()
    {
        redPath = new List<string>
        {
            // Start in the home area
            "r","r1", "r2", "r3", "r4", "r5",

            // Move through the outer tiles (1 to 1)
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", 
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "1",

            // Return to the home stretch (r5 to r)
            "r5", "r4", "r3", "r2", "r1", "r"
        };
        bluePath = new List<string>
        {
            // Start in the home area
            "b","b1", "b2", "b3", "b4", "b5",

            // Move thbough the outeb tiles (1 to 1)
            "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56","1", "2", "3", "4", "5", "6", "7", "8", "9", "10", 
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", 

            // Betubn to the home stbetch (b5 to b)
            "b5", "b4", "b3", "b2", "b1", "b"
        };
        yellowPath = new List<string>
        {
            // Start in the home area
            "y","y1", "y2", "y3", "y4", "y5",

            // Move thyough the outey tiles (1 to 1)
            "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", 
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29",

            // Yetuyn to the home styetch (y5 to y)
            "y5", "y4", "y3", "y2", "y1", "y"
        };
        greenPath = new List<string>
        {
            // Start in the home area
            "g","g1", "g2", "g3", "g4", "g5",

            // Move thgough the outeg tiles (1 to 1)
            "15","16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "1","2", "3", "4", "5", "6", "7", "8", "9", "10", 
            "11", "12", "13", "14", "15", 

            // Getugn to the home stgetch (g5 to g)
            "g5", "g4", "g3", "g2", "g1", "g"
        };
    }

    void InitializeTileDictionary()
    {
        // Initialize the dictionary
        tileDictionary = new Dictionary<string, GameObject>();

        // For outer tiles (1 to 56)
        for (int i = 1; i <= 56; i++)
        {
            string tileName = i.ToString(); // tile names: "1", "2", ..., "56"
            GameObject tileObject = GameObject.Find(tileName);

            if (tileObject != null)
            {
                // Add tile data to the dictionary: { tileName: [GameObject, position, rotation] }
                tileDictionary[tileName] = tileObject;
            }else{
                Debug.Log(tileName +  "not found");
            }
        }

        // For colored tiles (r1 to r5, y1 to y5, b1 to b5, g1 to g5)
        string[] colors = { "r", "y", "b", "g" };
        foreach (string color in colors)
        {
            for (int i = 1; i <= 5; i++)
            {
                string tileName = color + i; // tile names: "r1", "r2", ..., "g5"
                GameObject tileObject = GameObject.Find(tileName);

                if (tileObject != null)
                {
                   // Add tile data to the dictionary: { tileName: [GameObject, position, rotation] }
                tileDictionary[tileName] = tileObject;
                }
            }

            GameObject tileObjectFinal = GameObject.Find(color);
            if (tileObjectFinal != null)
            {
                // Add tile data to the dictionary: { tileName: [GameObject, position, rotation] }
                tileDictionary[color] = tileObjectFinal;
            }else{
                Debug.Log(color +  "not found");
            }
        }
    }

    void LogPositions()
    {
        foreach (string str in redPath)
        {
            if (tileDictionary.ContainsKey(str) && tileDictionary[str] != null)
            {
                Debug.Log(str + " Position: " + tileDictionary[str].transform.position);
            }
            else
            {
                Debug.LogError("Tile " + str + " not found or is null in tileDictionary.");
            }
        }
    }


    void Start()
    {


        // Initialize paths
        redCoins = new List<GameObject> {redcoin1, redcoin2, redcoin3, redcoin4};
        blueCoins = new List<GameObject> {bluecoin1, bluecoin2, bluecoin3, bluecoin4};
        yellowCoins = new List<GameObject> {yellowcoin1, yellowcoin2, yellowcoin3, yellowcoin4};
        greenCoins = new List<GameObject> {greencoin1, greencoin2, greencoin3, greencoin4};
        InitializePaths();
        InitializeTileDictionary();
        LogPositions();


        // Make sure the button is assigned, and attach the event listener
        if (rollButton != null)
        {
            rollButton.onClick.AddListener(OnRollButtonClick);
        }

        // Add listeners to each button and pass the coin index as argument
        coin1Button.onClick.AddListener(() => SelectCoin(0));  // Coin 1
        coin2Button.onClick.AddListener(() => SelectCoin(1));  // Coin 2
        coin3Button.onClick.AddListener(() => SelectCoin(2));  // Coin 3
        coin4Button.onClick.AddListener(() => SelectCoin(3));  // Coin 4

        // Hide the coin selection buttons initially
        enableButton();

    }

    void Update()
    {
        
        // Disable tile updates for testing
        // coinRed.transform.SetPositionAndRotation((Vector3)tileDictionary[redPath[currentRedPosition]][1], (Quaternion)tileDictionary[redPath[currentRedPosition]][2]);
    }
}
