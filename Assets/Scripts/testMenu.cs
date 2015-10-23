using UnityEngine;
using System.Collections;

public class testMenu : MonoBehaviour {

    // Use this for initialization
    public Renderer rend;
    float offset;

    Vector3 one = new Vector3(1f, 1f, 1f);

    public Transform frontMenu;
    Vector3 frontMenuScale;

    public Transform levelSelect;
    Vector3 levelSelectScale;

    int state;

    int currrentLevelSelected;
    public Transform selector;
    public Transform[] levels;

    void Start () {
        state = 0;
        currrentLevelSelected = 0;
        rend = GetComponent<Renderer>();
        frontMenuScale = frontMenu.localScale;
        levelSelectScale = levelSelect.localScale;
        levelSelect.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update ()
    {
        backgroundScroller();
        if (state == 0) // front menu
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(levelSelectMenuIn());
                
            }
        }
        if (state == 1) //level selector menu
        {
            if (Input.GetKeyDown(KeyCode.Q)) //go back
            {
                StartCoroutine(frontMenuIn());
                
            }

            selector.position = Vector3.Lerp(selector.position, levels[currrentLevelSelected].position, 0.1f);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currrentLevelSelected > 0)
                {
                    currrentLevelSelected -= 1;
                }
                else
                {
                    currrentLevelSelected = 3;
                }
                Debug.Log(currrentLevelSelected);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currrentLevelSelected = (currrentLevelSelected + 1) % 4;
                Debug.Log(currrentLevelSelected);
            }


            if (Input.GetKeyDown(KeyCode.Space)) //go to level
            {
                if(currrentLevelSelected == 1 || currrentLevelSelected == 2)
                Application.LoadLevel(currrentLevelSelected+1);
            }
        }


    }

    void backgroundScroller()
    {
        offset = (offset + Time.deltaTime * 0.1f) % 1;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }

    IEnumerator levelSelectMenuIn()
    {
        levelSelect.gameObject.SetActive(true);
        float t = 0;
        while (t <= 1f)
        {
            frontMenu.localScale = Vector3.Lerp(frontMenuScale,Vector3.zero, t);
            
            levelSelect.localScale = Vector3.Lerp(Vector3.zero,levelSelectScale, t);
            yield return null;
            t += Time.deltaTime;
        }
        state = 1;
        frontMenu.gameObject.SetActive(false);
    }


    IEnumerator frontMenuIn()
    {
        frontMenu.gameObject.SetActive(true);
        float t = 0;
        while (t <= 1f)
        {
            frontMenu.localScale = Vector3.Lerp(Vector3.zero, frontMenuScale, t);
            levelSelect.localScale = Vector3.Lerp(levelSelectScale, Vector3.zero, t);
            yield return null;
            t += Time.deltaTime;
        }
        state = 0;
        levelSelect.gameObject.SetActive(false);
    }

}
