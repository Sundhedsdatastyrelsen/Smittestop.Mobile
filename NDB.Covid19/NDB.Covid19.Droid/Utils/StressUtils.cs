using System;
using Android.OS;

namespace NDB.Covid19.Droid.Utils
{
    public class StressUtils
    {
        public class GenericSingleAction<T1, T2>
        {
            private readonly int _delayMilliseconds;
            private readonly Action<T1, T2> _setOnAction;
            private bool _hasStarted;

            public GenericSingleAction(Action<T1, T2> setOnAction, int delayMilliseconds = 1000)
            {
                _setOnAction = setOnAction;
                _delayMilliseconds = delayMilliseconds;
            }

            public void Run(T1 v, T2 e)
            {
                if (!_hasStarted)
                {
                    _hasStarted = true;
                    _setOnAction?.Invoke(v, e);
                }

                Reset();
            }

            private void Reset()
            {
                Handler mHandler = new Handler();
                mHandler.PostDelayed(() => { _hasStarted = false; }, _delayMilliseconds);
            }
        }

        public class SingleAction<T> : GenericSingleAction<object, T>
        {
            public SingleAction(Action<object, T> setOnAction, int delayMilliseconds = 1000) : base(setOnAction,
                delayMilliseconds)
            {
            }
        }

        public class SingleClick : SingleAction<EventArgs>
        {
            public SingleClick(Action<object, EventArgs> setOnAction, int delayMilliseconds = 1000) : base(setOnAction,
                delayMilliseconds)
            {
            }
        }
    }
}