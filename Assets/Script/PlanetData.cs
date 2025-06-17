using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData", menuName = "ScriptableObjects/PlanetData", order = 1)]
public class PlanetData : ScriptableObject
{
    public string planetName; //文字型のデータ
    public string brief_info;//ツールチップ
    public string detailed_info;//詳細説明
    public float mass;//ここから数字　質量
    public float radius;//設計書は直径だができれば半径直径にするならdiameterにかえてくれ　
    public float rotation_speed;//自転
    public float revolution_speed;//交転
    public float orebit_radius;//軌道半径
    public float initial_position_x;//初期位置
    public float initial_position_y;
    public float initial_position_z;
    public float gravity;
    public float temperature;//温度
    public float scale;
}