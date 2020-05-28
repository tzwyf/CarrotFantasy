using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 怪物宠物类
/// </summary>
public class MonsterPet : MonoBehaviour
{

    public MonsterPetData monsterPetData;

    private GameObject[] monsterLevelGo;//宠物对应的三个等级的游戏物体

    public Sprite[] buttonSprites;//0.可用milk 1.不可用milk 2 3

    //Egg
    private GameObject img_InstructionGo;

    //Baby
    private GameObject emp_FeedGo;
    private Text tex_milk;
    private Text tex_cookie;
    private Button btn_Milk;
    private Button btn_Cookie;
    private Image img_btn_Milk;
    private Image img_btn_Cookie;

    //Normal
    private GameObject img_TalkRightGo;
    private GameObject img_TalkLeftGo;

    public MonsterNestPanel monsterNestPanel;

    private void Awake()
    {
        monsterLevelGo = new GameObject[3];
        monsterLevelGo[0] = transform.Find("Emp_Egg").gameObject;
        monsterLevelGo[1] = transform.Find("Emp_Baby").gameObject;
        monsterLevelGo[2] = transform.Find("Emp_Normal").gameObject;

        //Egg
        img_InstructionGo = monsterLevelGo[0].transform.Find("Img_Instruction").gameObject;

        img_InstructionGo.SetActive(false);
        //Baby
        emp_FeedGo = monsterLevelGo[1].transform.Find("Emp_Feed").gameObject;
        emp_FeedGo.SetActive(false);
        btn_Milk = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Button>();
        img_btn_Milk = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").GetComponent<Image>();
        btn_Cookie = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Button>();
        img_btn_Cookie = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookie").GetComponent<Image>();
        tex_milk = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Milk").Find("Text").GetComponent<Text>();
        tex_cookie = monsterLevelGo[1].transform.Find("Emp_Feed").Find("Btn_Cookie").Find("Text").GetComponent<Text>();
        //Normal
        img_TalkLeftGo = transform.Find("Img_TalkLeft").gameObject;
        img_TalkRightGo = transform.Find("Img_TalkRight").gameObject;


    }

    private void OnEnable()
    {
        InitMonsterPet();
    }

    
    //点击宠物需要触发的事件方法
    public void ClickPet()
    {
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/PetSound"+monsterPetData.monsterLevel.ToString()));
        switch (monsterPetData.monsterLevel)
        {
            case 1:
                if (GameManager.Instance.playerManager.nest >= 1)
                {
                    GameManager.Instance.playerManager.nest--;
                    //升级 更新显示
                    ToNormal();
                    monsterPetData.monsterLevel++;
                    ShowMonster();
                    monsterNestPanel.UpdateText();
                }
                else
                {
                    img_InstructionGo.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }

                break;
            case 2:
                if (emp_FeedGo.activeSelf)
                {
                    emp_FeedGo.SetActive(false);
                }
                else
                {
                    emp_FeedGo.SetActive(true);
                    if (GameManager.Instance.playerManager.milk == 0)
                    {
                        img_btn_Milk.sprite = buttonSprites[1];
                        btn_Milk.interactable = false;
                    }
                    else
                    {
                        img_btn_Milk.sprite = buttonSprites[0];
                        btn_Milk.interactable = true;
                    }
                    if (GameManager.Instance.playerManager.cookies == 0)
                    {
                        img_btn_Cookie.sprite = buttonSprites[3];
                        btn_Cookie.interactable = false;
                    }
                    else
                    {
                        img_btn_Cookie.sprite = buttonSprites[2];
                        btn_Cookie.interactable = true;
                    }
                    if (monsterPetData.remainMilk == 0)
                    {
                        btn_Milk.gameObject.SetActive(false);
                    }
                    else
                    {
                        tex_milk.text = monsterPetData.remainMilk.ToString();
                        btn_Milk.gameObject.SetActive(true);
                    }
                    if (monsterPetData.remainCookies == 0)
                    {
                        btn_Cookie.gameObject.SetActive(false);
                    }
                    else
                    {
                        tex_cookie.text = monsterPetData.remainCookies.ToString();
                        btn_Cookie.gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                int randomNum = Random.Range(0, 2);
                if (randomNum == 1)
                {
                    img_TalkRightGo.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                else
                {
                    img_TalkLeftGo.SetActive(true);
                    Invoke("CloseTalkUI", 2);
                }
                break;
            default:
                break;
        }

    }

    //关闭对话框
    private void CloseTalkUI()
    {
        img_InstructionGo.SetActive(false);
        img_TalkRightGo.SetActive(false);
        img_TalkLeftGo.SetActive(false);
    }

    //显示当前等级的宠物
    private void ShowMonster()
    {
        for (int i = 0; i < monsterLevelGo.Length; i++)
        {
            monsterLevelGo[i].SetActive(false);
            if ((i + 1) == monsterPetData.monsterLevel)
            {
                monsterLevelGo[i].SetActive(true);
                Sprite petSprite = null;
                switch (monsterPetData.monsterLevel)
                {
                    case 1:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Egg/" + monsterPetData.monsterID.ToString());
                        break;
                    case 2:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Baby/" + monsterPetData.monsterID.ToString());
                        break;
                    case 3:
                        petSprite = GameManager.Instance.GetSprite("MonsterNest/Monster/Normal/" + monsterPetData.monsterID.ToString());
                        break;
                    default:
                        break;
                }
                Image monsterImage = monsterLevelGo[i].transform.Find("Img_Pet")
                    .GetComponent<Image>();
                monsterImage.sprite = petSprite;
                monsterImage.SetNativeSize();
                float imageScale = 0;
                if (monsterPetData.monsterLevel == 1)
                {
                    imageScale = 2;
                }
                else
                {
                    imageScale = 1 + (monsterPetData.monsterLevel - 1) * 0.5f;
                }

                monsterImage.transform.localScale = new Vector3(imageScale, imageScale, 1);
            }
        }
    }

    //喂牛奶
    public void FeedMilk()
    {
        //播放喂养动画与音效
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/Feed01"));
        GameObject heartGo = GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].GetItem("Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.playerManager.milk >= monsterPetData.remainMilk)
        {
            GameManager.Instance.playerManager.milk -= monsterPetData.remainMilk;
            monsterPetData.remainMilk = 0;
            //更新文本
            monsterNestPanel.UpdateText();
        }
        else
        {
            monsterPetData.remainMilk -= GameManager.Instance.playerManager.milk;
            GameManager.Instance.playerManager.milk = 0;
            btn_Milk.gameObject.SetActive(false);
        }
        emp_FeedGo.SetActive(false);
        Invoke("ToNormal", 0.433f);
    }

    //喂饼干
    public void FeedCookie()
    {
        //播放喂养动画与音效
        GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/Feed02"));
        GameObject heartGo = GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].GetItem("Img_Heart");
        heartGo.transform.position = transform.position;
        monsterNestPanel.SetCanvasTrans(heartGo.transform);
        if (GameManager.Instance.playerManager.cookies >= monsterPetData.remainCookies)
        {

            GameManager.Instance.playerManager.cookies -= monsterPetData.remainCookies;
            monsterPetData.remainCookies = 0;
            //更新文本
            monsterNestPanel.UpdateText();

        }
        else
        {
            monsterPetData.remainCookies -= GameManager.Instance.playerManager.cookies;
            GameManager.Instance.playerManager.cookies = 0;
            btn_Cookie.gameObject.SetActive(false);
        }
        emp_FeedGo.SetActive(false);
        Invoke("ToNormal", 0.433f);
    }

    //成长方法
    private void ToNormal()
    {

        if (monsterPetData.remainMilk == 0 && monsterPetData.remainCookies == 0)
        {
            GameManager.Instance.audioSourceManager.PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/PetChange"));
            monsterPetData.monsterLevel++;
            if (monsterPetData.monsterLevel >= 3)
            {
                GameManager.Instance.playerManager.unLockedNormalModelLevelList[monsterPetData.monsterID * 5 - 1].unLocked = true;
                GameManager.Instance.playerManager.burriedLevelNum++;
                ShowMonster();
            }
            else
            {
                ShowMonster();
            }
        }
        SaveMonsterData();
    }

    //保存宠物数据
    private void SaveMonsterData()
    {
        for (int i = 0; i < GameManager.Instance.playerManager.monsterPetDataList.Count; i++)
        {
            if (GameManager.Instance.playerManager.monsterPetDataList[i].monsterID == monsterPetData.monsterID)
            {
                GameManager.Instance.playerManager.monsterPetDataList[i] = monsterPetData;
            }
        }
    }

    //初始化宠物
    public void InitMonsterPet()
    {
        if (monsterPetData.remainMilk == 0)
        {
            monsterPetData.remainMilk = monsterPetData.monsterID * 60;
        }
        if (monsterPetData.remainCookies == 0)
        {
            monsterPetData.remainCookies = monsterPetData.monsterID * 30;
        }
        ShowMonster();
    }
}

//宠物数据信息
[System.Serializable]
public struct MonsterPetData
{
    public int monsterLevel;
    public int remainCookies;
    public int remainMilk;
    public int monsterID;
}
