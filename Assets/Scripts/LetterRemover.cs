using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterRemover : MonoBehaviour
{
    public int id;
    public int ownId;

    public LetterScript ls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveLetter()
    {
        ls.RemoveLetter(id,ownId);
    }
}
