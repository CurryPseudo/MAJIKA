﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;

public class ConversationUI : Singleton<ConversationUI>, IPointerClickHandler
{
    public float WPS = 10;
    public GameObject Wrapper;
    public Text TextRenderer;
    public Image ImageRenderer;
    public Graphic ContinueHint;
    public Sprite Transperant;
    public Talker[] Talkers;
    public IConversation Conversation;
    public bool Talking = false;

    DoubleBuffer<bool> touchScreenSkip = new DoubleBuffer<bool>();

    void Update()
    {
        touchScreenSkip.Update();
        touchScreenSkip.Value = false;
    }

    public void TouchScreenSkip()
    {
        touchScreenSkip.Value = true;
    }

    public IEnumerator StartConversation(IConversation conversation, Talker[] talkers, bool lockPlayer = false)
    {
        TextRenderer.text = "";
        Guid lockID;
        if (lockPlayer)
            lockID = GameSystem.Instance.PlayerInControl.GetComponent<EntityController>().Lock();

        Talkers = talkers;
        Conversation = conversation;
        Talking = true;
        Wrapper.SetActive(true);
        foreach (var text in conversation)
        {
            yield return ShowSentence(text);
        }
        Talkers = null;
        Conversation = null;
        Wrapper.SetActive(false);
        // InputManager.Instance.Jumped = false;
        yield return null;
        // InputManager.Instance.Jumped = false;
        GameSystem.Instance.PlayerInControl.GetComponent<EntityController>().UnLock(lockID);
    }

    public void EndConversation()
    {
        Conversation = null;
    }

    public IEnumerator ShowSentence(string text)
    {
        yield return null;
        var (talker, renderedText) = RenderSentence(text);
        if (talker)
            ImageRenderer.sprite = talker.Image;
        else
            ImageRenderer.sprite = Transperant;
        var secondsPerWord = 1 / WPS;
        foreach(var t in Utility.TimerNormalized(secondsPerWord*renderedText.Length))
        {
            if (InputManager.Instance.Controller.Actions.Accept.WasPressedThisFrame() || touchScreenSkip.Value)
                break;
            TextRenderer.text = renderedText.Substring(0, Mathf.FloorToInt(t * renderedText.Length));
            yield return null;
        }
        TextRenderer.text = renderedText;
        yield return null;
        yield return Utility.ShowUI(ContinueHint, 0.2f);
        while (!InputManager.Instance.Actions.Accept.WasPressedThisFrame() && !touchScreenSkip.Value)
            yield return null;
        // yield return InputManager.Instance.Controller.Actions.Accept.WaitPerform();
        yield return Utility.HideUI(ContinueHint, 0);
    }
    (Talker Talker, string Text) RenderSentence(string textTemplate)
    {
        var headerReg = new Regex(@"\$\{(.*?)\}:");
        var reg = new Regex(@"\$\{(.*?)\}");
        var talkerTemplate = headerReg.Match(textTemplate)?.Groups[1].Value;
        Talker talker = null;
        switch(talkerTemplate)
        {
            case "player":
                talker = FindObjectOfType<Player>().GetComponent<Talkable>().Talker;
                break;
            default:
                try
                {
                    talker = Talkers[Convert.ToInt32(talkerTemplate)];
                }
                catch
                {
                    talker = null;
                }
                break;
        }
        var text = reg.Replace(textTemplate, match => TemplateReplacer(match.Groups[1].Value));
        return (talker, text);
    }

    string TemplateReplacer(string template)
    {
        var replacer = new Dictionary<string, Func<string>>();
        if (template == "player")
            return FindObjectOfType<Player>().GetComponent<Talkable>().Talker.Name;
        try
        {
            return Talkers[Convert.ToInt32(template)].Name;
        }
        catch
        {
            return $"{{{template}}}";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TouchScreenSkip();
    }
}

class Sentence
{
    public Talker Talker = null;
    public string Text = null;
}
