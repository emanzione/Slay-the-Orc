using System.Collections.Generic;
using MHLab.InfectionsBlaster.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc.Dialogs
{
    public sealed class EndGameDialog : MonoBehaviour
    {
        public Text MainText;

        public GameObject Dialog;
        
        public GameObject Benedict;
        public GameObject Player;

        public AudioClip DialogSkipClip;
        private AudioSource _audioSource;
        
        private List<DialogEntry> _dialogs;
        private int _currentDialogIndex;

        private bool _canCaptureInput;
        private bool _canceled;
        
        private void Awake()
        {
            _audioSource = gameObject.GetComponentNoAlloc<AudioSource>();
            
            _dialogs = new List<DialogEntry>()
            {
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The battle was hard and wounds are deep..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "With his last energy, the knight tries to carry out of that cave all that food..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Son! You made it!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Yes, dad! We can go back to home!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "This little story finally comes to the end: both of them are heading Caershire..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "*STOMP* Son, for all gods! Don't eat the food!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Sorry, dad..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "... ... ..."
                },
            };

            Dialog.SetActive(false);
            Benedict.SetActive(false);
            Player.SetActive(false);
            
            Invoke(nameof(StartDialog), 1f);
        }

        private void StartDialog()
        {
            _canCaptureInput = true;
            Dialog.SetActive(true);
            ShowDialog(_currentDialogIndex);
        }

        private void Update()
        {
            if ((_canCaptureInput && Input.GetKeyDown(KeyCode.Space)) || _canceled)
            {
                _currentDialogIndex++;

                if (_currentDialogIndex >= _dialogs.Count)
                {
                    SceneManager.LoadScene("Credits");
                }
                else
                {
                    ShowDialog(_currentDialogIndex);
                    _audioSource.PlayOneShot(DialogSkipClip);
                }
            }

            if (_canCaptureInput && Input.GetKeyDown(KeyCode.Escape))
            {
                _currentDialogIndex = _dialogs.Count;
                _canceled = true;
            }
        }

        private void ShowDialog(int index)
        {
            var dialog = _dialogs[index];

            if (dialog.Speaker == DialogSpeaker.Benedict)
            {
                Benedict.SetActive(true);
                Player.SetActive(false);
            }
            else if (dialog.Speaker == DialogSpeaker.Player)
            {
                Benedict.SetActive(false);
                Player.SetActive(true);
            }
            else
            {
                Benedict.SetActive(false);
                Player.SetActive(false);
            }

            if (dialog.Speaker == DialogSpeaker.Narrator)
            {
                MainText.text = $"<i>\"{dialog.Text}\"</i>";
            }
            else
            {
                MainText.text = dialog.Text;
            }
        }
    }
}