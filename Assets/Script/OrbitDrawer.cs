using UnityEngine;

// ���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g��LineRenderer�R���|�[�l���g��K�{�ɂ���
[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    [Header("�O���̐ݒ�")]
    [Range(3, 360)]
    public int segments = 100; // �O�����\�����钸�_�̐��i�����قǊ��炩�j
    public float radius = 579.1; // �O���̔��a

    void Start()
    {
        // ���̃I�u�W�F�N�g�ɂ��Ă���LineRenderer�R���|�[�l���g���擾���ĕ`�悷��
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        DrawOrbit(lineRenderer);
    }

    // �O����`�悷�郁�C���̏���
    void DrawOrbit(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = segments; // ���_�̐���ݒ�
        lineRenderer.loop = true; // �n�_�ƏI�_�������Ō��сA�~�����

        // �~����̊e�_�̍��W���v�Z���Đݒ�
        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / (float)segments) * 2f * Mathf.PI; // �p�x�����W�A���Ōv�Z
            float x = Mathf.Sin(angle) * radius; // X���W
            float z = Mathf.Cos(angle) * radius; // Z���W

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z)); // �v�Z�������W����ɐݒ�
        }
    }
}