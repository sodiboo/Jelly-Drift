using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class RaceDetails : MonoBehaviour
{
    // Token: 0x0600010D RID: 269 RVA: 0x00006978 File Offset: 0x00004B78
    public void UpdateStars(int map)
    {
        var num = SaveManager.Instance.state.races[map] + 1;
        if (num <= 0)
        {
            text.text = "None";
        }
        else
        {
            text.text = ((DifficultyCycle.Difficulty)(num - 1)).ToString();
        }
        for (var i = 0; i < pbStars.Length; i++)
        {
            if (i < num)
            {
                pbStars[i].color = Color.yellow;
            }
            else
            {
                pbStars[i].color = Color.gray;
            }
        }
    }

    // Token: 0x0400012A RID: 298
    public Image[] pbStars;

    // Token: 0x0400012B RID: 299
    public TextMeshProUGUI text;
}
