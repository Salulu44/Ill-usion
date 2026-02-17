using UnityEngine;
namespace UsefulClasses
{

    [System.Serializable]
    public class UnityTimer
    {
        [SerializeField] private float _duration;
        [SerializeField] private bool _hasRandomRange;
        private float _currentTime;
        private bool _isRunning;

        public UnityTimer(float duration)
        {
            this._duration = duration;
        }

        public void PrepareStart()
        {
            if (_hasRandomRange)
            {
                _currentTime = Random.Range(0, _duration);
            }
            else
            {
                _currentTime = _duration;
            }
            _isRunning = true;
        }

        public void Tick()
        {
            if (_isRunning)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0)
                {
                    _isRunning = false;
                }
            }
        }
        public bool IsFinished() => !_isRunning && _currentTime <= 0;
        public bool IsRunning() => _isRunning;
        public float Progress() => 1 - (_currentTime / _duration);
        public void Reset() => _currentTime = _duration;
        public void Stop() => _isRunning = false;
    }
}
