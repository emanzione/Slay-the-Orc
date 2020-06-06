using System.Collections.Generic;
using MHLab.InfectionsBlaster.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc.Dialogs
{
    public sealed class PreBossDialog : MonoBehaviour
    {
        public Text MainText;

        public GameObject Dialog;
        
        public GameObject Orc;
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
                    Text = "And in the end, a big room opens up in front of the not-so-brave knight. It is really dark in there. It seems to be empty..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "But in the middle of the room he can spot something... This smell... Yeah, it seems... A huge pile of food! Here we are: he found it!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "This smell..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "Yeah, it seems..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "A huge pile of food! Here we are: he found it!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Hey, that's our stolen food! I can recognize the apple pie my mom made!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "I want it!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "But while the big guy tries to reach the pie, something emerges from the darkness..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Orc,
                    Text = "*ROOOAAARGH!*"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "And with the hit of its big mace, the orc destroys the apple pie..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "*NOOOOOOOO!!!*"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Orc,
                    Text = "*ROOOOOAAAAAARGH!!!*"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The orc wants to defend its loot... But the orc does not know that..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "... the food is the only huge passion of this not-so-heroic hero..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "He loves to cook and he can't stand the idea that someone is hurting a pie!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "With a big leap he jumps on the orc to hit with his sword... The weird creature hesitates for a second and falls on the ground."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Orc,
                    Text = "*ROOOOOAAAAAARGH!!!*"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The creature screams of anger, while trying to stand up."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "It's time to die, ignoble beast! You're a murderer of pies!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The showdown has come, it's time to fight!"
                },
            };

            Dialog.SetActive(false);
            Orc.SetActive(false);
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
                    SceneManager.LoadScene("Boss");
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

            if (dialog.Speaker == DialogSpeaker.Orc)
            {
                Orc.SetActive(true);
                Player.SetActive(false);
            }
            else if (dialog.Speaker == DialogSpeaker.Player)
            {
                Orc.SetActive(false);
                Player.SetActive(true);
            }
            else
            {
                Orc.SetActive(false);
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