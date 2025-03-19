using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    public GameObject dice; // Drag the dice GameObject in the Inspector
    private Rigidbody diceRb;

    public TextMeshProUGUI res1;
    public TextMeshProUGUI res2;

    private int res;

    private string objectName;

    public Button rollButton;
    public GameObject ludoBoard; // Ensure this is assigned in the Inspector
    private Vector3 initialPosition; // To store the starting position of the dice
    private Quaternion initialRotation; // To store the starting rotation of the dice
    public float stopTime = 6f; // Time after which the dice stops

    // Tolerance value for comparing rotations (5 degrees tolerance)
    private const float rotationTolerance = 5.0f;

    void Start()
    {
        diceRb = dice.GetComponent<Rigidbody>();

        objectName = gameObject.name;

        // Disable gravity initially and reset velocity
        diceRb.useGravity = false;
        diceRb.linearVelocity = Vector3.zero; // Use velocity instead of linearVelocity
        diceRb.angularVelocity = Vector3.zero;
    }

    int findDiceSide(){
        // Loop through all child objects of this GameObject
        foreach (Transform child in transform)
        {
            // Access the GameObject of the child;
            GameObject childObject = child.gameObject;

            if (childObject.transform.rotation.eulerAngles.x % 360 > 260 && childObject.transform.rotation.eulerAngles.x % 360 < 280 )
            {
                Debug.Log(gameObject.name + " Child GameObject: " + childObject.name);
                char lastChar = childObject.name[childObject.name.Length - 1]; // Get the last character

                int result = int.Parse(lastChar.ToString());

                if (childObject.name[4] == '1')
                {
                    res1.text = "Die 1: " + result;
                }
                else if (childObject.name[4] == '2')
                {
                    res2.text = "Die 2: " + result;
                }
                // Convert the last character to an integer
                return result;
            }
        }

        res1.text = "Invalid";
        res2.text = "Invalid";
        Debug.Log("Retry");
        return -1;
    }


    public async void RollDice()
    {
        // Disable the roll button immediately

        // Store the initial position and rotation of the dice, 2 units above the board
        initialPosition = ludoBoard.transform.position + new Vector3(0.3f, 2, 0);
        initialRotation = Quaternion.identity; // Reset to no rotation

        // Reset the dice to its original position and rotation
        dice.transform.position = initialPosition;
        dice.transform.rotation = initialRotation;

        // Reset the physics properties of the dice
        diceRb.linearVelocity = Vector3.zero;
        diceRb.angularVelocity = Vector3.zero;

        // Enable gravity to let the dice fall
        diceRb.useGravity = true;

        // Apply a random torque (rotational force) to simulate dice rolling
        ApplyRandomForceAndTorque();

        await StopDiceCoroutine(stopTime);

        Debug.Log("determining face");
    }

    private void ApplyRandomForceAndTorque()
    {
        // Add a slight upward force to make the dice move
        diceRb.AddForce(Vector3.up * 2, ForceMode.Impulse);

        // Apply random torque to make it spin randomly
        Vector3 randomTorque = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        diceRb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    private async Task StopDiceCoroutine(float time)
    {
        // Await the time delay (non-blocking)
    await Task.Delay((int)(time * 1000));  // Convert seconds to milliseconds

        // Disable gravity and stop the dice movement
        diceRb.useGravity = false;
        diceRb.linearVelocity = Vector3.zero;
        diceRb.angularVelocity = Vector3.zero;

        // Enable the roll button after stopping

        Debug.Log("Finding side");
        res = findDiceSide();

        // determineFace();
    }
}
