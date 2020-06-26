using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordCycling : MonoBehaviour
{
    public string word = "SEAMLESS";    //Inout word
    private char[] letters;              //Holds the correct letters (characters) from the word
    public TextMeshProUGUI displayBox;  //Used to display the text
    string uppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";    //essentially our list of characters to chose from
    public float waitTime = .15f;       //time between each new letter

    private char randomLetter;          //the random letter to compare to the real letter
    public char[] displayLetters;      //the letters for the built word

    private void Start()
    {
        letters = word.ToCharArray();
        displayLetters = new char[letters.Length];

        StartCoroutine(RandomLetters());
    }

    public IEnumerator RandomLetters()
    {
        for (int a = 0; a < letters.Length;)
        {
            displayLetters[a] = randomLetter;

            if (randomLetter == letters[a])
            {
                a++;
            }
            randomLetter = UppercaseLetter();

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void Update()
    {
        string builtWord = new string(displayLetters);
        displayBox.text = builtWord;
    }

    public char UppercaseLetter()
    {
        char letter = uppercaseAlphabet[Random.Range(0, uppercaseAlphabet.Length)];
        //letter = (char)('A' + Random.Range(0, 26)); //Alternative way of getting letter using ASCII

        return letter;
    }
}
