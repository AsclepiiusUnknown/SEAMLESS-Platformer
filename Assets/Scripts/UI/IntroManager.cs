using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    #region Variables
    // * //
    #region LETTER CYCLING
    [Header("Letter Cycling")]
    [Tooltip("The word we get at the end of cycling.")]
    public string endWord = "SEAMLESS";
    [Tooltip("The time between each new letter.")]
    public float waitTime = .15f;
    [Tooltip("Text elements for each letter we want to display.")]
    public TextMeshProUGUI[] lettersDisplay;

    //Our character lists to chose from:
    string uppercaseLatin = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string lowercaseLatin = "abcdefghijklmnopqrstuvwxyz";
    string symbols = "!#$%&''()*+,-./:;<=>?@[]^_`{|}~ ";
    string digits = "1234567890";

    public bool useUppercase;
    public bool useLowercase;
    public bool useSymbols;
    public bool useDigits;

    private char[] letters;                 //Holds the correct letters (characters) from the word
    private char randomLetter;              //the random letter to compare to the real letter
    private char[] displayLetters;          //the letters for the built word

    private int completionCount = 0;
    private bool isCycled = false;

    private bool[] letterDone;
    #endregion
    #endregion


    private void Start()
    {
        #region Cycling Setup
        letters = endWord.ToCharArray();
        displayLetters = new char[letters.Length];
        letterDone = new bool[letters.Length];

        StartCoroutine(CycleLetters(0));
        #endregion
    }

    private void Update()
    {
        if (completionCount == letters.Length && !isCycled)
        {
            isCycled = true;
        }
    }

    public void StartCycle()
    {
        for (int i = 0; i < lettersDisplay.Length && (i == 0 || letterDone[i - 1]); i++)
        {
            StartCoroutine(CycleLetters(i));
        }
    }

    #region SEAMLESS Cycling
    // * //
    #region Cycle Through Till We Reach Correct Letter
    public IEnumerator CycleLetters(int index)
    {
        while (index > 0 && !letterDone[index - 1])
        {
            yield return null;
        }

        while (randomLetter != letters[index])
        {
            displayLetters[index] = randomLetter;

            UpdateLetter(index, randomLetter);
            randomLetter = UppercaseLetter();

            yield return new WaitForSeconds(waitTime);
        }

        UpdateLetter(index, randomLetter);

        letterDone[index] = true;
        completionCount++;

        if ((index + 1) < letters.Length)
            StartCoroutine(CycleLetters(index + 1));
    }
    #endregion

    #region Update the Letters Text Element
    public void UpdateLetter(int index, char letter)
    {
        string builtChar = letter.ToString();
        lettersDisplay[index].text = builtChar;
    }
    #endregion

    #region Choose a Random Something
    public char UppercaseLetter()
    {
        int randomIndex = Random.Range(1, 5);
        char letter;

        #region Double Checking
        //Checking if the index is one that we dont wanna use
        while (!useUppercase && randomIndex == 1)
        {
            randomIndex = Random.Range(1, 5);
        }
        while (!useLowercase && randomIndex == 2)
        {
            randomIndex = Random.Range(1, 5);
        }
        while (!useSymbols && randomIndex == 3)
        {
            randomIndex = Random.Range(1, 5);
        }
        while (!useDigits && randomIndex == 4)
        {
            randomIndex = Random.Range(1, 5);
        }
        #endregion

        switch (randomIndex)
        {
            case 1:
                letter = uppercaseLatin[Random.Range(0, uppercaseLatin.Length)];
                break;
            case 2:
                letter = lowercaseLatin[Random.Range(0, lowercaseLatin.Length)];
                break;
            case 3:
                letter = symbols[Random.Range(0, symbols.Length)];
                break;
            case 4:
                letter = digits[Random.Range(0, digits.Length)];
                break;
            default:
                letter = uppercaseLatin[Random.Range(0, uppercaseLatin.Length)];
                break;
        }

        return letter;
    }
    #endregion
    #endregion
}
