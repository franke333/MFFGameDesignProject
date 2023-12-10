using CharlesEngine;

using System;
using System.Collections;
using System.IO;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private readonly IEnumerator advanceEnumerator;

    private string sceneDescriptor;
    private bool waiting;
    private SpriteRenderer spriteRenderer;
    private TMP_Text dialogueTMOText;
    private CEGameObject dialogueCEObject;

    public string SceneDescriptorFile;

    public SceneController()
    {
        advanceEnumerator = GetAdvanceEnumerator();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogueTMOText = GetComponentInChildren<TMP_Text>();
        dialogueCEObject = GetComponentInChildren<CEGameObject>();

        sceneDescriptor = File.ReadAllText(SceneDescriptorFile);
    }

    private void Start()
    {
        Advance();
    }

    public void Advance()
    {
        if (waiting)
            return;

        advanceEnumerator.MoveNext();
    }

    private IEnumerator GetAdvanceEnumerator()
    {
        foreach (var line in sceneDescriptor.Split(Environment.NewLine))
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                continue;

            char command = line[0];
            string argument = line.Substring(1).Trim();

            Debug.Log($"Executing comman \'{command}\' with argument \"{argument}\".");

            switch (command)
            {
                case 'D':
                    SwitchDialogue(argument);
                    yield return null;
                    break;
                case 'B':
                    SwitchBackground(argument);
                    break;
                case 'S':
                    SwitchScene(argument);
                    break;
                case 'W':
                    Wait(float.Parse(argument));
                    yield return null;
                    break;
                case 'V':
                    SetVoice(int.Parse(argument));
                    break;
                case 'A':
                    PlayAudio(argument); 
                    break;
                case 'H':
                    dialogueCEObject.Hide();
                    break;
                default:
                    throw new InvalidOperationException($"Invalid descriptor command: {command}.");

            }
        }
    }

    private void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SwitchBackground(string file)
    {
        var sprite = Resources.Load<Sprite>(file);

        if (sprite == null)
        {
            Debug.LogError($"Did not find sprite {file}.");
            return;
        }

        spriteRenderer.sprite = sprite;
    }

    private void SetVoice(int id)
    {
        // TODO
    }

    private void SwitchDialogue(string dialogue)
    {
        dialogueCEObject.Show();
        dialogueTMOText.text = dialogue;
        // TODO play sound
    }

    private void PlayAudio(string file)
    {
        // TODO
    }

    private void Wait(float seconds)
    {
        StartCoroutine(WaitCorutine(seconds));
    }

    private IEnumerator WaitCorutine(float seconds)
    {
        dialogueCEObject.Hide();
        waiting = true;
        yield return new WaitForSeconds(seconds);
        waiting = false;
        Advance();
    }
}