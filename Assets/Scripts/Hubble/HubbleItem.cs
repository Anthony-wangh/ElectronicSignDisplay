using UnityEngine;
using UnityEngine.UI;


//ÅÝÅÝItem
public class HubbleItem : MonoBehaviour
{

    public Vector2 ForceRange=new Vector2 (10,50);
    public RawImage Icon;
    private float _speed=1;
    private float _updateTime=0.2f;
    private float _curTime;

    private Vector2 _force=new Vector2(0,0);
    private Rigidbody2D _rigidbody;

    public bool IsFree = false;

    private void Awake()
    {
        _rigidbody=GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _curTime = 0;
    }

    public void SetTxture(Texture2D texture) {

        if (Icon == null)
            return;
        Icon.texture = texture;
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsFree) return;
        if (_curTime < _updateTime)
        {
            _curTime += Time.deltaTime;
        }
        else {         
            _curTime = 0;
            _updateTime=Random.Range(1f, 3f);

            _force = new Vector2(Random.Range(ForceRange.x, ForceRange.y), Random.Range(ForceRange.x, ForceRange.y));

            _rigidbody.AddForce(_force);
            //transform.position = Vector3.Lerp(transform.position, _pos, _speed);
        }
    }
}
