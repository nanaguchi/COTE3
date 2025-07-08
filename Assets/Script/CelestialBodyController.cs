using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    // --- Inspector�Őݒ肷�鍀�� ---

    [Header("���]�̐ݒ�")]
    public Transform orbitCenter; // ���]�̒��S�ƂȂ�V�́i��F���z�j
    public float orbitSpeed = 1f; // ���]�̑����i�x/�b�j

    [Header("rotation")]
    public float rotationSpeed = 1f; // ���]�̑����i�x/�b�j

    // Update is called once per frame
    void Update()
    {
        // --- 1. ���]���� ---
        // �������g��Y���𒆐S�ɁA�w�肵�����x�ŉ�]������
        // Time.deltaTime���|���邱�ƂŁA�t���[�����[�g�Ɉˑ����Ȃ����炩�ȓ����ɂȂ�
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);


        // --- 2. ���]���� ---
        // �������]�̒��S(orbitCenter)���ݒ肳��Ă���΁A���]���s��
        if (orbitCenter != null)
        {
            // orbitCenter�̎�����AY�������Ƃ��āA�w�肵�����x�Ō��]������
            transform.RotateAround(orbitCenter.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
}