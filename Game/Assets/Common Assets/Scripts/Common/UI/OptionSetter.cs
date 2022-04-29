using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public abstract class OptionSetter : MonoBehaviour
{
    [Header("Label")]
    [SerializeField] protected string label;
    [SerializeField] protected TextMeshProUGUI labelTMP;

    [Header("Errors")]
    [SerializeField] protected Image errorImage;
    [SerializeField] protected TextMeshProUGUI errorText;
    [SerializeField] protected float errorDuration = 3f;

    protected abstract void OnSceneChange(Scene scene, LoadSceneMode mode);

    protected void RemoveLastCharacter(TMP_InputField text)
    {
        if (text.text.Length == 0) return;
        text.text = text.text.Remove(text.text.Length - 1, 1);
    }

    protected IEnumerator ErrorAnimation(string errorText, float duration)
    {
        errorImage.gameObject.SetActive(true);
        this.errorText.text = errorText;
        yield return new WaitForSeconds(duration);
        errorImage.gameObject.SetActive(false);
    }

    protected void InitErrorMessage(string errorText, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ErrorAnimation(errorText, duration));
    }
}
