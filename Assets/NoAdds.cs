using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eiko.YaSDK;

public class NoAdds : MonoBehaviour
{
    public bool skins;
    void Start()
    {
        if (PurchaseProcess.Has("21") && skins==false)
            gameObject.SetActive(false);
        //Debug.Log(PlayerPrefs.GetInt("SkinsNoAddsw"));
        if(PurchaseProcess.Has("22"))
            gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
    public void BuyNoAdds()
    {
        YandexSDK.instance.onPurchaseSuccess += InstanceOnPurchaseSuccess;
        YandexSDK.instance.ProcessPurchase("21");
    }
    private void InstanceOnPurchaseSuccess(Purchase id)
    {
        gameObject.SetActive(false);
        YandexSDK.instance.onPurchaseSuccess -= InstanceOnPurchaseSuccess;
    }
}
