using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float selectAnimTime = 0.5f;
    [SerializeField] private float deselectAnimTime = 0.7f;

    [Header("Drag Settings")]
    [SerializeField] private float amplitude = 0.2f;

    [Header("Spring")]
    [SerializeField] private float stiffness = 15.05f;
    [SerializeField] private float clamp = 2f;

    [Header("Points")]
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private List<GameObject> launchPoints = new List<GameObject>();
    private List<Vector3> pointStartPos = new List<Vector3>();
    private GameObject selectedObject;

    [Header("Disparos")]
    [Tooltip("Número de tentativas disponíveis no início")]
    public int shotsLeft = 5;  // Valor editável no Inspector

    [Header("Referência ao Target")]
    [SerializeField] private Target targetScript; // Referência ao Target

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        foreach (GameObject point in points)
        {
            pointStartPos.Add(point.transform.localPosition);
        }
    }

    private void Start()
    {
        SyncShotsWithUI();
    }

    private void Update()
    {
        if (selectedObject != null)
        {
            StopAllCoroutines();
            Selected(selectedObject);
        }
    }

    public void SyncShotsWithUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateTentativas(shotsLeft);
        }
        else
        {
            Debug.LogWarning("UI Manager não atribuído no Reticle!");
        }
    }

    private void HandleRotation(GameObject item)
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        item.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandlePoint(Vector3 mousePosRelative, float distance, int i, Point point)
    {
        int dir = point.flip ? -1 : 1;
        Vector3 startPos = pointStartPos[i];
        float magnitude = (amplitude * distance) / distance;
        float pointIdentity = (startPos.x * magnitude) * dir;
        Vector3 targetPos = (mousePosRelative * pointIdentity) * dir;

        float lerpTime = (selectAnimTime / pointIdentity) * Time.deltaTime;
        Vector3 lerpPos = Vector3.Lerp(point.transform.localPosition, targetPos, lerpTime);

        lerpPos.z = 0;
        float pointDistance = Vector3.Distance(point.transform.position, this.transform.position);
        lerpPos = Vector3.ClampMagnitude(lerpPos, (pointIdentity / magnitude) + (pointDistance / clamp));

        point.transform.localPosition = lerpPos;
    }

    public void Selected(GameObject selected)
    {
        this.gameObject.SetActive(true);
        this.transform.position = selected.transform.position;
        selectedObject = selected;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 mousePosRelative = mouseWorldPos - this.transform.position;

        for (int i = 0; i < points.Count; i++)
        {
            HandleRotation(points[i].gameObject);
            float distance = Vector2.Distance(this.transform.position, mouseWorldPos);
            Point point = points[i].GetComponent<Point>();
            HandlePoint(mousePosRelative, distance, i, point);
        }
    }

    public void Deselect()
    {
        selectedObject = null;
        for (int i = 0; i < points.Count; i++)
        {
            StartCoroutine(LerpObject(points[i], Vector3.zero, deselectAnimTime));
        }
    }

    private void FireProjectiles()
    {
        if (shotsLeft <= 0)
        {
            Debug.Log("Sem disparos restantes!");
            if (targetScript != null && targetScript.GetScore() < targetScript.scoreToNextScene)
            {
                targetScript.CheckEndCondition();
            }
            return;
        }

        // Verificar se a pontuação máxima foi atingida
        if (targetScript != null && targetScript.GetScore() >= targetScript.scoreToNextScene)
        {
            Debug.Log("Pontuação máxima atingida. Não é possível disparar.");
            return;
        }

        for (int i = 0; i < launchPoints.Count; i++)
        {
            launchPoints[i].GetComponent<ShootProjectile>().FireProjectile();
        }

        shotsLeft--;
        SyncShotsWithUI();
        Debug.Log("Disparos restantes: " + shotsLeft);

        this.gameObject.SetActive(false);

        if (shotsLeft <= 0 && !Target.hitByLastArrow) // Verifica se não acertou
        {
            targetScript.CheckEndCondition();
        }
    }

    private int count = 0;
    private IEnumerator LerpObject(GameObject item, Vector3 pos, float time)
    {
        Vector3 currentPos = item.transform.localPosition;
        float elapsed = 0f;
        float ratio = 0;
        while (ratio < 1)
        {
            elapsed += Time.fixedDeltaTime;
            float offset = animCurve.Evaluate(ratio);
            float newOffset = (offset - ratio) / stiffness;
            offset = newOffset + ratio;
            float invertOffset = 1.0f - offset;
            item.transform.localPosition = Vector3.Lerp(currentPos, pos, ratio) * invertOffset;

            yield return null;
            ratio = (elapsed / time);
        }
        count++;
        if (count >= points.Count)
        {
            FireProjectiles();
        }
    }
}