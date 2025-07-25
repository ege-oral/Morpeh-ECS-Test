using TriInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        
    }

    [Button]
    public void Test1()
    {
        var test = new TestStruct
        {
            Name = "Ege",
            Age = 10,
        };
        
        ref var test2 = ref test;
        
        test2.Age = 20;

        test.Name = "Efe";
        
        Debug.Log(test2.Name);
        Debug.Log(test2.Age);
    }

    [Button]
    public void Test2()
    {
        var test = new TestClass()
        {
            Name = "Ege",
            Age = 10,
        };
        
        var test2 = test;

        test.Name = "Efe";
        test.Age = 20;
        
        Debug.Log(test2.Name);
        Debug.Log(test2.Age);
    }
}

public struct TestStruct
{
    public string Name;
    public int Age;
}

public class TestClass
{
    public string Name;
    public int Age;
}