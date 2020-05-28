using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 奖品
/// </summary>
public class Prize : MonoBehaviour {

    private void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/GiftGot");
        GameController.Instance.isPause = true;
#if Game
        GameController.Instance.ShowPrizePage();
#endif
        GameController.Instance.PushGameObjectToFactory("Prize",gameObject);
    }
}
