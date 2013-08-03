using Beem.Core.Binding;
using System.IO;

namespace Beem.ViewModels
{
    public class PlaybackPageViewModel : BindableBase
    {
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
