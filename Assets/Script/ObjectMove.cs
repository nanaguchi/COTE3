using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public PlanetData planetData;
    public Transform orbitCenter;

    private bool isOrbiting = true;
    private Vector3 rogueVelocity;
    
    // ★追加：1フレーム前の位置を記憶するための変数
    private Vector3 previousPosition;

    void Start()
    {
        // 起動時の位置を記録
        if (orbitCenter != null)
        {
            previousPosition = transform.position;
        }
    }

    void Update()
    {
        if (isOrbiting)
        {
            if (TimeController.Instance == null) return;
            
            float currentTime = TimeController.Instance.simulationTime;
            UpdatePlanetState(currentTime);

            // ★追加：現在の速度を計算するために、毎フレーム位置を記録
            rogueVelocity = (transform.position - previousPosition) / Time.deltaTime;
            previousPosition = transform.position;
        }
        else
        {
            // 軌道を離脱したら、保存した最後の速度でまっすぐ進み続ける
            transform.position += rogueVelocity * Time.deltaTime;
        }
    }

    void UpdatePlanetState(float time)
    {
        // (このメソッドの中身に変更はありません)
        if (planetData == null || orbitCenter == null) return;
        if (Mathf.Abs(planetData.revolution_speed) > 0.001f)
        {
            float revolutionDegreesPerHour = 360f / planetData.revolution_speed;
            float currentRevolutionAngle = revolutionDegreesPerHour * time;
            float radian = currentRevolutionAngle * Mathf.Deg2Rad;
            Vector3 orbitPos = new Vector3(
                Mathf.Cos(radian) * planetData.orebit_radius * 10,
                0,
                Mathf.Sin(radian) * planetData.orebit_radius * 10 
            );
            transform.position = orbitCenter.position + orbitPos;
        }
        if (Mathf.Abs(planetData.rotation_speed) > 0.001f)
        {
            float rotationDegreesPerHour = 360f / planetData.rotation_speed;
            float currentRotationAngle = rotationDegreesPerHour * time / 10;
            Quaternion axialTilt = Quaternion.Euler(0, 0, planetData.angle);
            Quaternion rotation = Quaternion.AngleAxis(currentRotationAngle, Vector3.up);
            transform.rotation = axialTilt * rotation;
        }
    }
    
    // ★変更点：このメソッドはシンプルになります
    public void GoRogue()
    {
        if (!isOrbiting) return;
        Debug.Log(gameObject.name + " が軌道を離脱しました。");
        isOrbiting = false;
        // 速度はUpdateで常に計算されているので、ここではフラグを変えるだけ
    }
}