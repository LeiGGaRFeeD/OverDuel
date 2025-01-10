using UnityEngine;

public class ComputerCowboy : MonoBehaviour
{
    public CowboySettings settings; // ��������� ������ �� ScriptableObject
    public GameObject bulletPrefab; // ������ ����
    public Transform shootPoint; // ����� ��������

    private Transform[] waypoints; // ������ �����, ����� �������� �������� ������
    private int currentWaypointIndex = 0; // ������ ������� �����
    public bool hasShot = false; // �����������, ������� �� ���������
    public bool isBoss = false; // ����, �����������, �������� �� ������ ������

    public float speedIncreasePercentage = 10f; // ������� ���������� �������� ������ 5 �������

    void Start()
    {
        if (GameManager.Instance != null)
        {
            waypoints = GameManager.Instance.GetComputerWaypoints(); // �������� ����� �� GameManager
        }

        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("Waypoints ��� ���������� �� ���������!");
            return;
        }

        hasShot = true; // ������������� ��������� ���������, ����� �������� �������� ��������
        Invoke(nameof(ResetShootState), 0.1f); // ���������� ��������� ����� ��������� ���������� �������

        UpdateSpeedBasedOnLevel(); // ��������� �������� �� ������ �������� ������
    }

    // ����� ��������� "��������"
    void ResetShootState()
    {
        hasShot = false;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Move(); // �������� ����� �������

        if (!hasShot)
        {
            Shoot(); // ��������� �������, ���� �� ��������
        }
    }

    // �������� ����� �������
    void Move()
    {
        if (settings == null)
        {
            Debug.LogError("��������� ������ �� �����������!");
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, settings.speed * Time.deltaTime);

        Debug.Log($"Current Speed: {settings.speed}"); // ������� ��������

        // ���� �������� ������� �����, ������������� �� ���������
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // ��������
    void Shoot()
    {
        if (settings == null)
        {
            Debug.LogError("��������� ������ �� �����������!");
            return;
        }

        // ������� ��� �������� ������
        if (!isBoss)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // ������ ����
        }
        else
        {
            // ������� ������� ��� �����
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // ������ �������
            Invoke(nameof(BossSecondShot), 0.2f); // ������ ������� ����� 0.2 �������
        }

        hasShot = true; // ������������� ���� "��������"
        GameManager.Instance.CheckReloadState(); // ��������� ��������� �����������
    }

    // ������ ������� ��� �����
    void BossSecondShot()
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity); // ������ �������
    }

    // ����� ���������� ��� ���������
    public void OnHit()
    {
        Destroy(gameObject); // ���������� ������
        GameManager.Instance.NextLevel(); // ������� � ���������� ������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(); // ���� ��������� ������������, ������������ ���������
    }

    // ����� ��� ���������� �������� ������
    public void UpdateSettings(CowboySettings newSettings)
    {
        settings = newSettings;
        UpdateSpeedBasedOnLevel(); // ��������� �������� ��� ��������� ��������
    }

    // ��������� �������� ������ �� ������ �������� ������
    private void UpdateSpeedBasedOnLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); // �������� ������� ������� �� PlayerPrefs

        // ������ 5 ������� ����������� ��������
        if (currentLevel % 5 == 0)
        {
            float multiplier = 1 + (speedIncreasePercentage / 100f);
            settings.speed *= multiplier;
            Debug.Log($"Speed increased! New Speed: {settings.speed}");
        }
    }
}
