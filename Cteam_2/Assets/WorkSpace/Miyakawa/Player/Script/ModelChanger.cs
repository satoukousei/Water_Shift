using UnityEngine;

public class ModelChanger : MonoBehaviour
{
    [SerializeField] private ModelTracking modelTracking;

    [SerializeField] private GameObject model1_ice;
    [SerializeField] private GameObject model2_water;
    [SerializeField] private GameObject model3_steam;
    [SerializeField] private WaterMove water;
    [SerializeField] private Steam_Playe_Move steam;
    [SerializeField] private PlayerMove ice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model1_ice.SetActive(false);
        model2_water.SetActive(true);
        model3_steam.SetActive(false);
        
    }

 public void ChangeModel_ice()
    {
        model1_ice.SetActive(true);
        model2_water.SetActive(false);
        model3_steam.SetActive(false);
        ice.IceRbSwitch();
    }
 public void ChangeModel_water()
    {
        model1_ice.SetActive(false);
        model2_water.SetActive(true);
        model3_steam.SetActive(false);
        water.WaterRbSwitch();
    }
 public void ChangeModel_steam()
    {
        model1_ice.SetActive(false);
        model2_water.SetActive(false);
        model3_steam.SetActive(true);
        steam.SteamRbSwitch();
    }
}
