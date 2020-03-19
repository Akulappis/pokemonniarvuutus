using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LetterIdentifier : MonoBehaviour
{
    public char[] identifierArray;
    public char identifier;
    public LetterScript ls;
    public int id;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetIdentifier()
    {
        identifierArray = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text.ToCharArray();
        identifier = identifierArray[0];
    }
    public void LetterSelected()
    {
        ls.SelectLetter(identifier, id, gameObject);
        //gameObject.SetActive(false);
    }
}
