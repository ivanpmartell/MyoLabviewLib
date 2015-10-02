using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyoLabviewLib
{
    public class MyMyo
    {
        private readonly IHub _hub;
        private readonly IChannel _channel;
        private readonly int[] _pointPairs;
        private readonly int _sensors;

        public MyMyo(int sensors)
        {
            _channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            _hub = Hub.Create(_channel);

            _hub.MyoConnected += Hub_MyoConnected;
            _hub.MyoDisconnected += Hub_MyoDisconnected;

            _sensors = sensors;
            _pointPairs = new int[_sensors];
            // start listening for Myo data
            _channel.StartListening();
        }

        public string[] GetArm(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var armArray = new string[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                armArray[i] = myosArray[i].Arm.ToString();
            }
            return armArray;
        }

        public string[] GetPose(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var poseArray = new string[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                poseArray[i] = myosArray[i].Pose.ToString();
            }
            return poseArray;
        }

        public int[] GetHandles(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var handleArray = new int[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                handleArray[i] = myosArray[i].Handle.ToInt32();
            }
            return handleArray;
        }

        public bool[] IsConnected(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var connectionArray = new bool[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                connectionArray[i] = myosArray[i].IsConnected;
            }
            return connectionArray;
        }

        public bool[] IsUnlocked(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var lockArray = new bool[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                lockArray[i] = myosArray[i].IsUnlocked;
            }
            return lockArray;
        }

        public string[] GetXDirectionOnArm(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var xdirArray = new string[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                xdirArray[i] = myosArray[i].XDirectionOnArm.ToString();
            }
            return xdirArray;
        }

        public bool Lock(int handle)
        {
            foreach(var myo in _hub.Myos)
            {
                if(myo.Handle.ToInt32() == handle)
                {
                    myo.Lock();
                    return true;
                }
            }
            return false;
        }

        public bool Unlock(int handle, bool hold)
        {
            foreach (var myo in _hub.Myos)
            {
                if (myo.Handle.ToInt32() == handle)
                {
                    if (hold)
                        myo.Unlock(UnlockType.Hold);
                    else
                        myo.Unlock(UnlockType.Timed);
                    return true;
                }
            }
            return false;
        }

        public bool Vibrate(int handle, string type)
        {
            foreach (var myo in _hub.Myos)
            {
                if (myo.Handle.ToInt32() == handle)
                {
                    switch(type.ToLower())
                    {
                        case "short":
                            myo.Vibrate(VibrationType.Short);
                            break;
                        case "medium":
                            myo.Vibrate(VibrationType.Medium);
                            break;
                        case "long":
                            myo.Vibrate(VibrationType.Long);
                            break;
                    }
                    return true;
                }
            }
            return false;
        }

        public float[,] GetGyroscopeXYZ(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var gyroscopeArray = new float[numberOfMyos,3];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                gyroscopeArray[i, 0] = myosArray[i].Gyroscope.X;
                gyroscopeArray[i, 1] = myosArray[i].Gyroscope.Y;
                gyroscopeArray[i, 2] = myosArray[i].Gyroscope.Z;
            }
            return gyroscopeArray;
        }

        public float[] GetGyroscopeMagnitude(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var magnitudeArray = new float[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                magnitudeArray[i] = myosArray[i].Gyroscope.Magnitude();
            }
            return magnitudeArray;
        }

        public float[,] GetAccelerometerXYZ(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var gyroscopeArray = new float[numberOfMyos, 3];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                gyroscopeArray[i, 0] = myosArray[i].Accelerometer.X;
                gyroscopeArray[i, 1] = myosArray[i].Accelerometer.Y;
                gyroscopeArray[i, 2] = myosArray[i].Accelerometer.Z;
            }
            return gyroscopeArray;
        }

        public float[] GetAccelerometerMagnitude(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var magnitudeArray = new float[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                magnitudeArray[i] = myosArray[i].Accelerometer.Magnitude();
            }
            return magnitudeArray;
        }

        public float[,] GetOrientationWXYZ(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var orientationeArray = new float[numberOfMyos, 4];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                orientationeArray[i, 0] = myosArray[i].Orientation.W;
                orientationeArray[i, 1] = myosArray[i].Orientation.X;
                orientationeArray[i, 2] = myosArray[i].Orientation.Y;
                orientationeArray[i, 3] = myosArray[i].Orientation.Z;
            }
            return orientationeArray;
        }

        public float[] GetOrientationMagnitude(int numberOfMyos = 0)
        {
            if (!ValidateMyoNum(numberOfMyos))
                numberOfMyos = 1;

            var magnitudeArray = new float[numberOfMyos];
            var myosArray = _hub.Myos.ToArray();
            for (int i = 0; i < myosArray.Length; i++)
            {
                magnitudeArray[i] = myosArray[i].Orientation.Magnitude();
            }
            return magnitudeArray;
        }

        public int[] GetEMGData()
        {
            return _pointPairs;
        }

        public bool SetEMGStreaming(int handle, bool value)
        {
            foreach (var myo in _hub.Myos)
            {
                if (myo.Handle.ToInt32() == handle)
                {
                    myo.SetEmgStreaming(true);
                    return true;
                }
            }
            return false;
        }

        public void Lock()
        {
            foreach(var myo in _hub.Myos)
            {
                myo.Unlock(UnlockType.Timed);
            }
            _channel.Dispose();
            _hub.Dispose();
        }

        private bool ValidateMyoNum(int? num)
        {
            if (num == null || num < 1)
                return false;
            else
                return true;
        }

        private void Myo_EmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            // pull data from each sensor
            for (var i = 0; i < _sensors; ++i)
            {
                _pointPairs[i] = e.EmgData.GetDataForSensor(i);
            }
        }

        private void Hub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired -= Myo_EmgDataAcquired;
        }

        private void Hub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired += Myo_EmgDataAcquired;
            e.Myo.Unlock(UnlockType.Timed);
        }
    }
}
