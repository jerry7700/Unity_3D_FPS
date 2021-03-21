using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private int winNpc;

    public int killPlayer;
    public int killNpc1;
    public int killNpc2;
    public int killNpc3;

    public int deadPlayer;
    public int deadNpc1;
    public int deadNpc2;
    public int deadNpc3;

    [Header("勝利次數-玩家")]
    public Text textPlayer;
    [Header("勝利次數-電腦")]
    public Text textNpc;
    [Header("玩家")]
    public Text textDataPalyer;
    [Header("電腦1")]
    public Text textDataNpc1;
    [Header("電腦2")]
    public Text textDataNpc2;
    [Header("電腦3")]
    public Text textDataNpc3;
    [Header("畫布群組")]
    public CanvasGroup group;

    private float enemyCount;

    //傳址 ref
    public void UpdateDataKill(ref int kill, Text textKill, string content, int dead)
    {
        kill++;
        textKill.text = content + "    " + kill + "       |      " + dead;
    }

    public void UpdateDataDead(int kill, Text textDead, string content,ref int dead)
    {
        dead++;
        textDead.text = content + "    " + kill + "       |      " + dead;
        //content一定要一樣
        if (content == "玩家 ") StartCoroutine(ShowFinal());
        //content.Contains包含
        else if (content.Contains ("電腦"))
        {
            enemyCount++;

            if(enemyCount == 3) StartCoroutine(ShowFinal());
        }
    }

    private IEnumerator ShowFinal()
    {
        float a = group.alpha;
        while (a < 1)
        {
            a += 0.1f;
            group.alpha = a;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
