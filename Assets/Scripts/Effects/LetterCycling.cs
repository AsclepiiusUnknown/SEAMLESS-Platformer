using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterCycling : MonoBehaviour
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

    [Header("Character Lists")]
    string uppercaseLatin = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //List of Uppercase letters we cycle through to make our word

    [Header("Letters")]
    private char[] letters; //Holds the correct letters (characters) from the word to check against
    private char randomLetter; //A random letter to compare to the real letter (changed constantly)

    [Header("Completion Checks")]
    public string menuScene;
    private int completionCount = 0; //Used to check how many of the letters are complete
    private bool isComplete = false; //Used to check if all of the letters are complete (triggers animations and stuff)
    private bool[] letterDone; //Used to check which specific letters are complete
    #endregion
    #endregion

    private void Start()
    {
        #region Cycling Setup
        letters = endWord.ToCharArray();
        letterDone = new bool[letters.Length];

        StartCoroutine(CycleLetters(0));

        for (int i = 1; i < letters.Length; i++)
        {
            StartCoroutine(RandomLetters(i));
        }
        #endregion
    }

    private void Update()
    {
        if (completionCount == letters.Length && !isComplete)
        {
            isComplete = true;
            SceneManager.LoadScene(menuScene);
        }
    }

    #region SEAMLESS Cycling
    // * //
    #region Cycle Through Till Correct
    public IEnumerator CycleLetters(int index)
    {
        #region Bug Proofing
        //If we shouldnt be cycling this letter yet then exit
        while (index > 0 && !letterDone[index - 1])
        {
            yield return null;
        }

        if (index != 0 && letters[index] == letters[index - 1])
        {
            randomLetter = UppercaseLetter();
        }
        #endregion

        #region Cycling Till Correct
        while (randomLetter != letters[index]) //While the letter isnt correct
        {
            DisplayLetter(index, randomLetter); //Update the display of our letter using the index and our random letter
            randomLetter = UppercaseLetter(); //Get a new random letter

            yield return new WaitForSeconds(waitTime); //wait the wait time
        }
        #endregion

        #region Correct Letter Reached
        DisplayLetter(index, randomLetter); //Update the display of our letter using the index and our random letter (actually correct this time though)

        letterDone[index] = true; //We have completed this letter
        completionCount++; //we have completed another letter

        #region Start Next Cycle
        if ((index + 1) < letters.Length) //if this wasnt the last letter
        {
            StartCoroutine(CycleLetters(index + 1)); //do the next letter
        }
        #endregion
        #endregion
    }
    #endregion

    #region Cycle while we wait
    public IEnumerator RandomLetters(int index)
    {
        while (!letterDone[index - 1])
        {
            DisplayLetter(index, randomLetter);
            randomLetter = UppercaseLetter();

            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion

    #region Update the Letters Text Element
    public void DisplayLetter(int index, char letter)
    {
        string builtChar = letter.ToString();
        lettersDisplay[index].text = builtChar;
    }
    #endregion

    #region Choose Random Characters of a Type
    public char UppercaseLetter()
    {
        char letter = uppercaseLatin[Random.Range(0, uppercaseLatin.Length)];

        return letter;
    }
    #endregion
    #endregion
}