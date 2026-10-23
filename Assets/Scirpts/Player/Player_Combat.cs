using UnityEngine;
using UnityEngine.Serialization;

public class PLayer_Combat : Entity_Combat
{
    [FormerlySerializedAs("parryDuration")]
    [Header("Parry")] 
    [SerializeField] private float parryRecover;
    
    //khi xét thấy counterable != null, sẽ ngay lập tức gọi đến stunstate của enemy, vậy cần tạo ra biến bool hasparry làm gì?
    //biến hasparry có tác dụng trả về xem player đã parry trúng ai hay chưa và trong tương lai cái này sẽ được dùng khi mà 
    //tạo ra 1 đòn trọng kích chỉ parry trúng mới có và change state thông qua biến hasparry đang true hay false.
    public bool Parry()
    {
        bool hasParry = false;
        foreach (var target in GetDetectedCollider())
        {
            ICounterAble counterable = target.GetComponent<ICounterAble>();
            if (counterable == null) continue;
            
            if (counterable.CanBeParry)
            {
                counterable?.HandleCounter();
                hasParry = true;
            }
        }
        return hasParry;
    }
    
    public float getParryRecoverDuration() => parryRecover;
}
