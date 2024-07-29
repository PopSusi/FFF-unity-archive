using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextBox : MonoBehaviour
{
    [SerializeField] private string tutorialText;
    bool used;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            if (!used)
            {
                UIManager.instance.Tutorialize(tutorialText);
                used = true;
            }
        }
    }
}
