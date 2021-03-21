using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //一般欄位 載入場竟會恢復預設值
    //static = 靜態
    //靜態欄位 不會恢復
    //靜態不會顯示在遊戲面板
    private static int winNpc;
    private static int winPlayer;

    public static int killPlayer;
    public static int killNpc1;
    public static int killNpc2;
    public static int killNpc3;

    public static int deadPlayer;
    public static int deadNpc1;
    public static int deadNpc2;
    public static int deadNpc3;

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

    private bool gameOver;
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
        if (content == "玩家 ")
        {
            winNpc++;
            textNpc.text = "勝利次數:" + winNpc;
            StartCoroutine(ShowFinal());
        }
        //content.Contains包含
        else if (content.Contains("電腦"))
        {
            enemyCount++;

            if (enemyCount == 3)
            {
                winPlayer++;
                textPlayer.text = "勝利次數:" + winPlayer;
                StartCoroutine(ShowFinal());
            }
        }
    }

    private IEnumerator ShowFinal()
    {
        FindObjectOfType<FPSController>().enabled = false;
        float a = group.alpha;
        while (a < 1)
        {
            a += 0.1f;
            group.alpha = a;
            yield return new WaitForSeconds(0.2f);
            gameOver = true;
        }
    }

    /// <summary>
    /// 重新啟動遊戲
    /// </summary>
    private void Replay()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOver) SceneManager.LoadScene("遊戲場景");
    }

    private void Update()
    {
        Replay();
    }
}
