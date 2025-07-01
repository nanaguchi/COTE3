using UnityEngine;

// ���̃X�N���v�g���@�\����ɂ́ACollider��PlanetData�R���|�[�l���g���K�v
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlanetData))]
public class PlanetTooltipTrigger : MonoBehaviour
{
    private PlanetData planetData;

    void Awake()
    {
        // ���g������PlanetData�R���|�[�l���g���擾
        planetData = GetComponent<PlanetData>();
    }

    // �}�E�X�J�[�\�������̃I�u�W�F�N�g�̃R���C�_�[�ɏ�������ɌĂ΂��
    void OnMouseEnter()
    {
        // TooltipManager�ɁA���g�̏���n���ĕ\�����˗�����
        if (planetData != null)
        {
            TooltipManager.Instance.ShowTooltip(planetData, this.transform);
        }
    }

    // �}�E�X�J�[�\�������̃I�u�W�F�N�g�̃R���C�_�[����O�ꂽ���ɌĂ΂��
    void OnMouseExit()
    {
        // TooltipManager�ɔ�\�����˗�����
        TooltipManager.Instance.HideTooltip();
    }
}