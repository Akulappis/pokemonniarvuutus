using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

public class LetterScript : MonoBehaviour
{
    public TextMeshProUGUI[] letterArray;
    public GameObject[] letterPlaces;
    public int currentLevel;
    public int normalizedLevel = 1;
    public TextMeshProUGUI levelIndicator;
    public string currentWord;
    public char[] randomizedLetters = new char[16];
    public List<char> newTexts;
    public int lettersSelected = 0;
    private bool[] usedLetterSlots = new bool[16]{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
    public List<string> currentGuess;
    public Image animalImage;

    public string url = "https://pokeres.bastionbot.org/images/pokemon/";
    public Sprite mySprite;
    public Image asd;
    private string[] names = new string[]{"Bulbasaur","Ivysaur","Venusaur","Charmander","Charmeleon","Charizard","Squirtle","Wartortle","Blastoise","Caterpie","Metapod","Butterfree","Weedle","Kakuna","Beedrill","Pidgey","Pidgeotto","Pidgeot","Rattata","Raticate","Spearow","Fearow","Ekans","Arbok","Pikachu","Raichu","Sandshrew","Sandslash","Nidoran","Nidorina","Nidoqueen","Nidoran","Nidorino","Nidoking","Clefairy","Clefable","Vulpix","Ninetales","Jigglypuff","Wigglytuff","Zubat","Golbat","Oddish","Gloom","Vileplume","Paras","Parasect","Venonat","Venomoth","Diglett","Dugtrio","Meowth","Persian","Psyduck","Golduck","Mankey","Primeape","Growlithe","Arcanine","Poliwag","Poliwhirl","Poliwrath","Abra","Kadabra","Alakazam","Machop","Machoke","Machamp","Bellsprout","Weepinbell","Victreebel","Tentacool","Tentacruel","Geodude","Graveler","Golem","Ponyta","Rapidash","Slowpoke","Slowbro","Magnemite","Magneton","Farfetch'd","Doduo","Dodrio","Seel","Dewgong","Grimer","Muk","Shellder","Cloyster","Gastly","Haunter","Gengar","Onix","Drowzee","Hypno","Krabby","Kingler","Voltorb","Electrode","Exeggcute","Exeggutor","Cubone","Marowak","Hitmonlee","Hitmonchan","Lickitung","Koffing","Weezing","Rhyhorn","Rhydon","Chansey","Tangela","Kangaskhan","Horsea","Seadra","Goldeen","Seaking","Staryu","Starmie","Mr. Mime","Scyther","Jynx","Electabuzz","Magmar","Pinsir","Tauros","Magikarp","Gyarados","Lapras","Ditto","Eevee","Vaporeon","Jolteon","Flareon","Porygon","Omanyte","Omastar","Kabuto","Kabutops","Aerodactyl","Snorlax","Articuno","Zapdos","Moltres","Dratini","Dragonair","Dragonite","Mewtwo","Mew"};
    public int indexNumber = 0;
    public List<int> pokemonNames;
    public GameObject raycastBlocker;

    // Start is called before the first frame update
    void Start()
    {

        currentLevel = PlayerPrefs.GetInt("CurrentLevel",0);
        indexNumber = currentLevel;
        normalizedLevel = indexNumber + 1;
        levelIndicator.text = normalizedLevel.ToString();


        if (currentLevel == 0)
        {
            for (int i = 0; i < 151; i++)
            {
                pokemonNames.Add(i);
            }
            ListShuffle(pokemonNames);
        }
        else
        {
            for (int i = 0; i < 151; i++)
            {
                pokemonNames.Add(PlayerPrefs.GetInt(i.ToString()));
            }
        }
        NextWord();

    }
    public void NextWord()
    {                
        if (currentLevel < 151)
        {   
            Shuffle(names[pokemonNames[indexNumber]].ToCharArray());
            foreach (var item in letterArray)
            {
                item.gameObject.transform.parent.gameObject.GetComponent<LetterIdentifier>().GetIdentifier();
            }
        }
        else
        {
            Debug.Log("End");
            EndGame();
        }
    }


    public void Shuffle(char[] texts)
    {
        foreach (var item in letterPlaces)
        {
            item.SetActive(false);
        }
        StartCoroutine(GetTexture(pokemonNames[indexNumber]+1));
        animalImage.sprite = mySprite;
        levelIndicator.text = normalizedLevel.ToString();
        currentWord = names[pokemonNames[indexNumber]].ToUpper();
        Debug.Log(currentWord);
        for (int k = 0; k < currentWord.Length; k++)
        {
            letterPlaces[k].SetActive(true);
        }
        int fillLenght = 16 - currentWord.Length;
        
        foreach (var item in texts)
        {
            newTexts.Add(Char.ToUpper(item));
        }

        for (int j = 0; j < fillLenght; j++)
        {
            string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char c = st[UnityEngine.Random.Range(0,st.Length)];
            newTexts.Add(c);
        }
        int i = 0;
         // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < 15; t++ )
        {
            char tmp = newTexts[t];
            int r = UnityEngine.Random.Range(t, 16);
            newTexts[t] = newTexts[r];
            newTexts[r] = tmp;
        }
        foreach (var item in newTexts)
        {
            letterArray[i].text = item.ToString(); 
            i++;
        }
        
        newTexts.Clear();
        
    }
    public void CheckWord()
    {
            currentGuess.Clear();
            for (int i = 0; i < currentWord.Length; i++)
            {
                currentGuess.Add(letterPlaces[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            }    
            string result = ""; 
            foreach (var item in currentGuess)
            {
                result = result + item;
            }   
            if (String.Equals(currentWord,result))
            {
                ChangeLevel();
            }
            Debug.Log(result);
            result = "";      
    }
    public void ChangeLevel()
    {      
        indexNumber++;
        currentLevel++;
        normalizedLevel++;
        lettersSelected = 0;
        for (int i = 0; i < usedLetterSlots.Length; i++)
        {
            usedLetterSlots[i] = false;
        }
        PlayerPrefs.SetInt("CurrentLevel",currentLevel);
        NextWord();
        for (int i = 0; i < 16; i++)
        {
            letterPlaces[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            letterArray[i].gameObject.transform.parent.gameObject.SetActive(true);
        }   
    }
    void ListShuffle(List<int> a)
    {
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = UnityEngine.Random.Range(0, i);
    
            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            int temp = a[i];
    
            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    
            // Print
        for (int i = 0; i < a.Count; i++)
        {
            Debug.Log(a[i]);
        }
        int tempI = 0;
        foreach (var item in pokemonNames)
        {
            PlayerPrefs.SetInt(tempI.ToString(),item);
            tempI++;
        }
    }
   
    public void SelectLetter(char letter, int id, GameObject disableThis)
    {
        if (lettersSelected < currentWord.Length)
        {          
            disableThis.SetActive(false);
            for (int i = 0; i < usedLetterSlots.Length; i++)
            {
                if (usedLetterSlots[i] == false)
                {
                    letterPlaces[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = letter.ToString();
                    letterPlaces[i].GetComponent<LetterRemover>().id = id;
                    usedLetterSlots[i] = true;
                    break;
                }
            }
            lettersSelected++;   
        }
        CheckWord();
    }
    public void RemoveLetter(int removableId, int ownId)
    {
        usedLetterSlots[ownId] = false;
        letterArray[removableId].gameObject.transform.parent.gameObject.SetActive(true);
        letterPlaces[ownId].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        lettersSelected--;
    }
    public void EndGame()
    {
        raycastBlocker.SetActive(true);
    }

     
    IEnumerator GetTexture(int rn) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url+rn.ToString()+".png");
        yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Texture2D tex2D = (Texture2D)myTexture;
            mySprite = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(.5f,.5f), 100.0f);
            animalImage.sprite = mySprite;
        }
    }
}
