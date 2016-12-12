using System;
using System.Numerics;

namespace DeviceEmulator
{
    public static class DataGenerator
    {
        public static float GenerateTemperatureValue(float baseValue)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            var signValue = rnd.NextDouble();
            var addValue = (float)rnd.NextDouble();
            var resValue = baseValue;

            if (signValue > 0.5)
                resValue += addValue;
            else
                resValue -= addValue;
            return resValue;
        }

        public static float GeneratePowerValue(float baseValue)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            var signValue = rnd.NextDouble();
            var addValue = (float)rnd.NextDouble();
            var resValue = baseValue;

            if (signValue > 0.5)
                resValue += addValue;
            else
                resValue -= addValue;
            return resValue;
        }

        public static Vector2 GenerateWindValue(float baseValue, float maxDelta)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            var signValueX = rnd.NextDouble();
            var addValueX = (float)rnd.NextDouble();
            var resValueX = baseValue;

            var signValueY = rnd.NextDouble();
            var addValueY = (float)rnd.NextDouble();
            var resValueY = baseValue;

            if (signValueX > 0.5)
            {
                resValueX += maxDelta * addValueX;
            }
            else
            {
                resValueX -= maxDelta * addValueX;
            }

            if (signValueY > 0.5)
            {
                resValueY += maxDelta * addValueY;
            }
            else
            {
                resValueY -= maxDelta * addValueY;
            }
            return new Vector2(resValueX,resValueY);
        }

        public static Vector2 GenerateMainValue(Vector2 baseValue, float maxDelta)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            var signValueX = rnd.NextDouble();
            var addValueX = (float)rnd.NextDouble();
            var resValueX = baseValue.X;

            var signValueY = rnd.NextDouble();
            var addValueY = (float)rnd.NextDouble();
            var resValueY = baseValue.Y;

            if (signValueX > 0.5)
            {
                resValueX += maxDelta * addValueX;
            }
            else
            {
                resValueX -= maxDelta * addValueX;
            }

            if (signValueY > 0.5)
            {
                resValueY += maxDelta * addValueY;
            }
            else
            {
                resValueY -= maxDelta * addValueY;
            }
            return new Vector2(resValueX, resValueY);
        }
    }
}
