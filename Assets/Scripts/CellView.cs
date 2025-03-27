using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellView : MonoBehaviour
{
    public bool isMoving { get; set; }

    [SerializeField]
    public TextMeshProUGUI valueText;

    [SerializeField]
    Color startingColour;
    [SerializeField]
    Color endingColour;

    InputManager inputManager;
    SpriteRenderer spriteRenderer;

    Cell cell;

    const int maxValue = 11;

    private void Awake()
    {
        inputManager = FindFirstObjectByType<InputManager>();

        //if (inputManager == null )
        //{
        //    Debug.LogError("Input Manager not found.");
        //}

        spriteRenderer = GetComponent<SpriteRenderer>();
        //if (!TryGetComponent<SpriteRenderer>(out spriteRenderer))
        //{
        //    Debug.LogError("Sprite rendered not found.");
        //}
    }

    public void Init(Cell cell)
    {
        this.cell = cell;
        Color currentColour = Color.Lerp(startingColour, endingColour, Mathf.Log(cell.Value, 2) / maxValue);
        spriteRenderer.color = new Color(currentColour.r, currentColour.g, currentColour.b, 1);
        valueText.text = cell.Value.ToString();
        cell.OnValueChanged += UpdateValue;
        cell.OnPositionChanged += UpdatePosition;
    }

    void UpdateValue()
    {
        Color currentColour = Color.Lerp(startingColour, endingColour, Mathf.Log(cell.Value, 2) / maxValue);
        spriteRenderer.color = new Color(currentColour.r, currentColour.g, currentColour.b, 1);
        valueText.text = cell.Value.ToString();
    }

    void UpdatePosition()
    {
        StartCoroutine(UpdatePositionCoroutine(transform.position, cell.Position));
    }

    IEnumerator UpdatePositionCoroutine(Vector3 from, Vector3 to)
    {
        isMoving = true;
        InputManager.movingCellAmount += 1;
        Vector3 unit = (to - from) * 5;
        float totalDistance = (to - from).magnitude;
        float distanceCovered = 0;
        while ((to - transform.position).magnitude > 0.2 || distanceCovered <= totalDistance)
        {
            transform.position += unit * Time.deltaTime;
            distanceCovered += (unit * Time.deltaTime).magnitude;
            yield return null;
        }
        transform.position = to;
        InputManager.movingCellAmount -= 1;
        isMoving = false;
    }
}
