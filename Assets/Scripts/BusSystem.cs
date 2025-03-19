using System;
using System.Collections.Generic;

public class BusObject
{
    public object Sender;
    public object Content;
}


public static class BusSystem
{
    private static Dictionary<string, Action<BusObject>> _dic = new Dictionary<string, Action<BusObject>>();

    public static void Register(string id, Action<BusObject> action)
    {
        if (_dic.ContainsKey(id) == false)
        {
            _dic.Add(id, new Action<BusObject>((obj) => {}));
            _dic[id] += action;
        }
        else
        {
            _dic[id] += action;
        }
    }
    
    public static void Unregister(string id, Action<BusObject> action)
    {
        _dic[id] -= action;
    }

    public static void Execute(string id, BusObject obj = null)
    {
        if (_dic.ContainsKey(id) == true)
        {
            _dic[id].Invoke(obj);
        }
    }
}