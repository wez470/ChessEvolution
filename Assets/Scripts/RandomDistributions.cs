using UnityEngine;

public class RandomDistributions {
    public static float RandNormal(float mean, float stdDev) {
        float u1 = Random.value; //these are uniform(0,1) random doubles
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
            Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
        return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
    }
}