using Beem.Core.Binding;
using System.IO;

namespace Beem.ViewModels
{
    public class PlaybackPageViewModel : BindableBase
    {
        static PlaybackPageViewModel instance = null;
        static readonly object padlock = new object();

        public PlaybackPageViewModel()
        {

        }

        public static PlaybackPageViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PlaybackPageViewModel();
                    }
                    return instance;
                }
            }
        }

        private MemoryStream _recordingContents;
        public MemoryStream RecordingContents
        {
            get
            {
                return _recordingContents;
            }
            set
            {
                SetProperty(ref _recordingContents, value);
            }
        }

        private int _recordingLength;
        public int RecordingLength
        {
            get
            {
                return _recordingLength;
            }
            set
            {
                SetProperty(ref _recordingLength, value);
            }
        }

        private bool _isRecording;
        public bool IsRecording
        {
            get
            {
                return _isRecording;
            }
            set
            {
                SetProperty(ref _isRecording, value);
            }
        }
    }
}
