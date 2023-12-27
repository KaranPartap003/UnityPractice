using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphs : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    [SerializeField ,Range(10, 100)]
    int resolution = 10;
    [SerializeField]
    FunctionLibrary.FunctionName function;
    [SerializeField, Min(0f)]
    float functionDuration = 1f, TransitionDuration = 1f;

    public enum TransitionMode {Cycle, Random};
    [SerializeField]
    TransitionMode transitionMode;

    Transform[] Points;
    private void Awake()
    {
        Points = new Transform[resolution * resolution];
        float step = 2f / resolution;//step defines the factor by which each point will be scaled down or up 
        var scale = Vector3.one * step;
        for (int i = 0; i < Points.Length; i++)
        {
            
            Transform point = Points[i] = Instantiate(pointPrefab);
            point.SetParent(transform, false);
            point.localScale = scale;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ? 
            FunctionLibrary.GetNextFunctionSerially(function):
            FunctionLibrary.GetNextFunctionRandomly(function);
    }

    float duration;
    bool Transitioning;
    FunctionLibrary.FunctionName transitionFunction;
    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;
        if (Transitioning) {//if currently transitioning, decrease transitioning time untill transition complete
            if (duration >= TransitionDuration)
            {
                duration -= TransitionDuration;
                Transitioning = false;
            }
        }
        else if(duration >= functionDuration)//if duration of current function exceeds, we change the function and go to transitioning phase
        {
            duration -= functionDuration;
            Transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }
        if(Transitioning)
        {
            UpdateFunctionTransition();
        }
        else
           UpdateFunction();
    }
    void UpdateFunction()//displays a function
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < Points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            v = (z + 0.5f) * step - 1f;
            Points[i].localPosition = f(u, v, time);
        }
    }
    void UpdateFunctionTransition()//displays transition between two functions
    {
        FunctionLibrary.Function
            from = FunctionLibrary.GetFunction(transitionFunction),
            to = FunctionLibrary.GetFunction(function);
        float progress = duration/TransitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < Points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            v = (z + 0.5f) * step - 1f;
            Points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}
