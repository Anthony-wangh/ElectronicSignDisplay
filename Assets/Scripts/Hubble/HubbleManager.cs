using DG.Tweening;
using System;
using UnityEngine;

public class RecieveSignEventArg: EventArgs
{
    public Texture2D Texture;
}

public class HubbleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ItemPrefabs;

    public const string RecieveSignEventTag = "OnRecieveSign";
    // Start is called before the first frame update
    void Awake()
    {
        EventManager.Instance.AddListener(RecieveSignEventTag, OnRecieveSign);
    }

    private void OnRecieveSign(object sender, System.EventArgs e)
    {
        if(e is RecieveSignEventArg eventArg) {

            Debug.Log("收到图片数据，开始展示！！");
            CreateItem(eventArg.Texture);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(RecieveSignEventTag, OnRecieveSign);
    }

    /// <summary>
    /// 创建泡泡
    /// </summary>
    /// <param name="texture"></param>
    public void CreateItem(Texture2D texture) { 
    
        GameObject item=GameObject.Instantiate(ItemPrefabs,transform);
        item.transform.position = Vector3.zero;
        item.transform.localScale = 0.5f*Vector3.one;
        var render=item.GetComponent<Renderer>();
        render.material.color = Color.red;
        var hubble = item.GetComponent<HubbleItem>();
        hubble.SetTxture(texture);
        var targetScale = UnityEngine.Random.Range(0.9f, 1.2f);
        item.transform.DOScale(targetScale * Vector3.one,1).SetEase(Ease.OutBack).onComplete=()=> {

            hubble.IsFree = true;
            render.material.color = Color.white;
        };
    }
}
