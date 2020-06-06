using System.Collections.Generic;
using MHLab.InfectionsBlaster.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.SlayTheOrc.Dialogs
{
    public sealed class MainDialog : MonoBehaviour
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
                    Text = "Somewhere in a forest, a spry old man and his promising son are traveling at a good pace..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "... Daddy?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "... What?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Are we there yet?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "... again: no, we are not."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Ok... Sorry."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "They are coming from the near village of Caershire. The life there is quiet, but recently an orc is stealing food from their granary."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "That's not acceptable! Not for Benedict Smythe, the old man. He is the major and he has to protect his village."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "... Daddy?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "... WHAT?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Sorry, dad."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Son, don't say sorry. Real men don't say that. And you are a knight, aren't you?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "I-I guess I a-am, dad..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Perfect! Then you can totally enter the dungeon, slay the bad orc and save the village, right?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "I guess I can, dad... But..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "There is not \"buts\"! This mission is critical! The hope of all of us is..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "DAD, I HAVE TO PEE!!!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Oh... Why didn't you say that earlier? You can do it behind that bush..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "But dad: it's dark in there... And it's full of bugs!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "Yes, the guy is not really smart. At all. But he has a good size and a strong physique: perfect to fight orcs. At least: that's what Benedict said..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "For the Ynos sake, son! Don't act like a little girl! Where we are going is dark and full of bugs too!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "They are exploring the zone to find the entrance of the cave where the orc is hiding. Not an easy task on night time."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "But the time cannot stop and the night goes on. While bitching around they find some weird stuff on the ground: a trail of food!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Son! Look! That's our food!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "They follow it and in some minutes they reach a hole in the mountain: here it is! The orc's cave!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "Son! Do you know what it is?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "*CRUNCH* Yes, dad: I know... *SMUNCH* That's an apple!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "May Ynos hit you on the head with his big mug! Stop eating the damned apples on the ground! *STOMP*"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "We found the cave. Go in and kill the orc!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Ok, dad! I'll come back soon! I'll not disappoint you!"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "... Son?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Player,
                    Text = "Yes, dad?"
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Benedict,
                    Text = "You forgot the sword..."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The cave is cold, dark and full of monsters... It is composed by rooms. The orc is hiding in the last one for sure."
                },
                new DialogEntry()
                {
                    Speaker = DialogSpeaker.Narrator,
                    Text = "The not-so-smart guy takes the last big breath of fresh air and goes in..."
                },
            };

            Dialog.SetActive(false);
            Benedict.SetActive(false);
            Player.SetActive(false);
            
            Invoke(nameof(StartDialog), 1.5f);
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
                    SceneManager.LoadScene("LevelSelection");
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