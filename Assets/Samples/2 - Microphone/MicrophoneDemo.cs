using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace Whisper.Samples
{
    /// <summary>
    /// Record audio clip from microphone and make a transcription.
    /// </summary>
    public class MicrophoneDemo : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public bool streamSegments = true;
        public bool printLanguage = true;

        [Header("UI")] 
        public Button button;
        public TextMeshProUGUI buttonText;
        public Text outputText;
        public Text timeText;
        public Dropdown languageDropdown;
        public Toggle translateToggle;
        public Toggle vadToggle;
        public ScrollRect scroll;
        public API_Manager apiManager;

        private string _buffer;

        private bool activated = false;
        private InputFieldPrompt inputField;
        private void Start()
        {
            inputField = GameObject.FindWithTag("input").GetComponent<InputFieldPrompt>();
        }

        private void Update()
        {
            if (inputField.activated == true) return;
            if (Input.GetKeyDown("y"))
            {
                AudioManager.instance.PlayRecordClip();
                OnButtonPressed();
            }
        }

        private void Awake()
        {
            whisper.OnNewSegment += OnNewSegment;
            whisper.OnProgress += OnProgressHandler;
            
            microphoneRecord.OnRecordStop += OnRecordStop;
            
            button.onClick.AddListener(OnButtonPressed);
            languageDropdown.value = languageDropdown.options
                .FindIndex(op => op.text == whisper.language);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            translateToggle.isOn = whisper.translateToEnglish;
            translateToggle.onValueChanged.AddListener(OnTranslateChanged);

            vadToggle.isOn = microphoneRecord.vadStop;
            vadToggle.onValueChanged.AddListener(OnVadChanged);
        }

        private void OnVadChanged(bool vadStop)
        {
            microphoneRecord.vadStop = vadStop;
        }

        public void OnButtonPressed()
        {
            if (!microphoneRecord.IsRecording)
            {
                activated = true;
                microphoneRecord.StartRecord();
                buttonText.text = "Stop";
            }
            else
            {
                activated = false;
                microphoneRecord.StopRecord();
                buttonText.text = "Record";
            }
        }
        
        private async void OnRecordStop(AudioChunk recordedAudio)
        {
            buttonText.text = "Record";
            _buffer = "";

            var sw = new Stopwatch();
            sw.Start();
            
            var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
            if (res == null || !outputText) 
                return;

            var time = sw.ElapsedMilliseconds;
            var rate = recordedAudio.Length / (time * 0.001f);
            // timeText.text = $"Time: {time} ms\nRate: {rate:F1}x";

            var text = res.Result;
            if (printLanguage)
                text += $"\n\nLanguage: {res.Language}";
            
            outputText.text = text;
            apiManager.RequestMemory(text);
            UiUtils.ScrollDown(scroll);
        }
        
        private void OnLanguageChanged(int ind)
        {
            var opt = languageDropdown.options[ind];
            whisper.language = opt.text;
        }
        
        private void OnTranslateChanged(bool translate)
        {
            whisper.translateToEnglish = translate;
        }

        private void OnProgressHandler(int progress)
        {
            if (!timeText)
                return;
            timeText.text = $"Progress: {progress}%";
        }
        
        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || !outputText)
                return;

            _buffer += segment.Text;
            outputText.text = _buffer + "...";
            UiUtils.ScrollDown(scroll);
        }
    }
}