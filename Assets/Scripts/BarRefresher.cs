using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*==========================================================================================
 * This is a component of a Status bar that 
 * refreshes the fillAmount in the mask depending on a Stat's value
 * It is a subclass of MonoBehaviour
 * Author: Hanwei Li (Compasslg)
 ==========================================================================================*/
public class BarRefresher : MonoBehaviour {
    [SerializeField]
    private Image content;
    [SerializeField]
    private Text text;
    private Stat stat;
    //== Update is called once per frame ========================================================
    void Update () {
        if (stat != null)
        {
            if (content.fillAmount != stat.GetValuePercentage())
            {
                // Update fill Amount
                content.fillAmount = stat.GetValuePercentage();
                // Update Text
                text.text = stat.ToString();
            }
        }
    }

    //== Set the binded Stat object =============================================================
    public void SetStat(Stat stat)
    {
        this.stat = stat;
        text.text = stat.ToString();
    }
}
