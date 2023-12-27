using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Mathf;
using UnityEngine;

public static class FunctionLibrary
{
    //A delegate is a special type that defines what kind of method(type of input/return etc..) something can reference.
    //the function that follows this template can be refrenced using this delegate
    public delegate Vector3 Function(float x,float z, float t);
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus };
    public static Function[] functions = { Wave, MultiWave, Ripple, Sphere, Torus};

    public static FunctionName GetNextFunctionSerially(FunctionName name)
    {
        return (int)name == functions.Length - 1 ? 0 : name + 1;
    }

    public static FunctionName GetNextFunctionRandomly(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }
    public static Function GetFunction(FunctionName Name)
    {
        return functions[(int)Name];
    }
    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.z = v;
        p.y = Sin(PI * (u + v + t));
        return p;
    }
    public static Vector3 MultiWave(float u,float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + t));
        p.y += 0.5f * Sin(2f * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t)); 
        p.y *= 1f / 2.5f;
        p.z = v;
        return p;
    }
    public static Vector3 Ripple(float u,float v, float t)
    {
        float d = Sqrt(u*u + v*v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;
        return p;
    }
    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    public static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f,1f,progress));
    }
}
