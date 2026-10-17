using System.Threading.Tasks;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public ShopSeedDisplay[] seeds;
    public void OpenClose()
    {
        if (UIController.instance.theIC.gameObject.activeSelf == false)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            if (gameObject.activeSelf)
            {
                foreach(var seed in seeds)
                {
                    seed.UpdateDisplay();
                }
            }
        }
    }
}
