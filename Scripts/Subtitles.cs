using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{

    public static Subtitles instance;

    public enum Section {None, Intro, Puzzle1Intro, Puzzle1Hint, Puzzle2Intro, Puzzle2Hint, Puzzle3Intro, Outro};
    public GameObject textBox;

    void Start()
    {
        //StartCoroutine(IntroSequence());
    }

    public void StartSubtitles(Section section)
    {
        switch (section)
        {
            case Section.None:
                Debug.Log("A None section has been called - if this was a voiceover, it has to be set to a specific section.");
                break;
            case Section.Intro:
                StartCoroutine(IntroSequence());
                break;
            case Section.Puzzle1Intro:
                StartCoroutine(Puzzle1Intro());
                break;
            case Section.Puzzle1Hint:
                StartCoroutine(Puzzle1Hint());
                break;
            case Section.Puzzle2Intro:
                StartCoroutine(Puzzle2Intro());
                break;
            case Section.Puzzle2Hint:
                StartCoroutine(Puzzle2Hint());
                break;
            case Section.Puzzle3Intro:
                StartCoroutine(Puzzle3Intro());
                break;
            case Section.Outro:
                StartCoroutine(OutroSequence());
                break;
        }
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(0.1f);
        textBox.GetComponent<Text>().text = "What happens to a House when it is left alone for too long?";
        yield return new WaitForSeconds(3.9f); // The amount of time the text will stay there
        textBox.GetComponent<Text>().text = ""; // Pause between subtitles
        yield return new WaitForSeconds(1.1f);
        textBox.GetComponent<Text>().text = "What does it think? What does it feel?";
        yield return new WaitForSeconds(3.6f); // The amount of time the text will stay there
        textBox.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(1.1f);
        textBox.GetComponent<Text>().text = "Does it bear resentment towards humanity? For being put into existence, just for it to be abandoned.";
        yield return new WaitForSeconds(7f); // The amount of time the text will stay there
        textBox.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(1.2f);
        textBox.GetComponent<Text>().text = "Does it cease being a home?";
        yield return new WaitForSeconds(2f); // The amount of time the text will stay there
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator Puzzle1Intro()
    {
        yield return new WaitForSeconds(0.1f);
        textBox.GetComponent<Text>().text = "As a scorned child, the House is distrusting of attention towards its flaws.";
        yield return new WaitForSeconds(6.2f);
        textBox.GetComponent<Text>().text = "It will reject every notion of this unwanted attention.";
        yield return new WaitForSeconds(4.5f);
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator Puzzle1Hint()
    {
        yield return new WaitForSeconds(0.07f);
        textBox.GetComponent<Text>().text = "Like a child averting their gaze from the prospect of a needle piercing the skin.";
        yield return new WaitForSeconds(5.1f);
        textBox.GetComponent<Text>().text = "You must distract the House from its impurities and conceal your help.";
        yield return new WaitForSeconds(5f);
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator Puzzle2Intro()
    {
        yield return new WaitForSeconds(0.07f);
        textBox.GetComponent<Text>().text = "Out of bitterness, it put up barriers to avoid further neglect.";
        yield return new WaitForSeconds(4.4f);
        textBox.GetComponent<Text>().text = "It will try to sabotage attempts of help through defiance.";
        yield return new WaitForSeconds(4.6f);
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator Puzzle2Hint()
    {
        yield return new WaitForSeconds(0.1f);
        textBox.GetComponent<Text>().text = "Through continuous display of kindness the barriers must be broken";
        yield return new WaitForSeconds(4.7f);
        textBox.GetComponent<Text>().text = "Do not fall for its rebellious acts of distraction";
        yield return new WaitForSeconds(3.7f);
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator Puzzle3Intro()
    {
        yield return new WaitForSeconds(0.1f);
        textBox.GetComponent<Text>().text = "A hopeful trust has been ignited.";
        yield return new WaitForSeconds(2.5f);
        textBox.GetComponent<Text>().text = "To nourish this trust, a fire must be built through a united effort.";
        yield return new WaitForSeconds(5.5f);
        textBox.GetComponent<Text>().text = "";
    }

    IEnumerator OutroSequence()
    {
        textBox.GetComponent<Text>().color = Color.black;
        yield return new WaitForSeconds(0.1f);
        textBox.GetComponent<Text>().text = "A House of no one is not a home";
        yield return new WaitForSeconds(2.9f);
        textBox.GetComponent<Text>().text = "The House had stood by itself, alone in its foundation for far too long;";
        yield return new WaitForSeconds(5.9f);
        textBox.GetComponent<Text>().text = "Building a darkness within, a misery, born from neglect.";
        yield return new WaitForSeconds(5.4f);
        textBox.GetComponent<Text>().text = ""; // ...Slight pause...
        yield return new WaitForSeconds(1.3f);
        textBox.GetComponent<Text>().text = "Through the acts of compassion, the shadow of neglect was swept away.";
        yield return new WaitForSeconds(4.5f);
        textBox.GetComponent<Text>().text = "The House no longer stood alone; from within, it was no longer full of silent noise,";
        yield return new WaitForSeconds(6.8f);
        textBox.GetComponent<Text>().text = "but started to hum with life.";
        yield return new WaitForSeconds(3.6f);
        textBox.GetComponent<Text>().text = ""; // ...Pause...
        yield return new WaitForSeconds(1f);
        textBox.GetComponent<Text>().text = "As such, the House became a Home.";
        yield return new WaitForSeconds(4f);
        textBox.GetComponent<Text>().text = "";
    }
}
