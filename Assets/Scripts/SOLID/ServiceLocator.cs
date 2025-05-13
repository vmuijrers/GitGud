using UnityEngine;
using System.Collections.Generic;


public static class AudioServiceLocator
{
    private static IAudioHandler instance;
    public static IAudioHandler GetItem
    {
        get
        {
            if (instance == null)
            {
                instance = new NULLAudioHandler();
                Debug.LogError("No Instance Provided!");
            }
            return instance;
        }
    }

    public static void Provide(IAudioHandler item)
    {
        instance = item;
    }
}

public static class ServiceLocator<T>
{
    private static T instance;
    public static T GetItem
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No Instance Provided!");
            }
            return instance;
        }
    }

    public static void Provide(T item)
    {
        instance = item;
    }
}

public static class MultiServiceLocator
{
    private static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();

    public static T GetService<T>()
    {
        if (services.ContainsKey(typeof(T)))
        {
            return (T)services[typeof(T)];
        }
        return default(T);
    }

    public static void Provide<T>(T _value)
    {
        if (!services.ContainsKey(typeof(T)))
        {
            services.Add(typeof(T), _value);
        }
        services[typeof(T)] = _value;
    }

    public static void ClearAllServices()
    {
        services.Clear();
    }
}


public interface IAudioHandler
{
    void PlayAudio(AudioClip clip);
}

public class ConcreteAudioHandler : IAudioHandler
{
    public void PlayAudio(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }
}

public class NULLAudioHandler : IAudioHandler
{
    public void PlayAudio(AudioClip clip)
    {
        Debug.Log("Playing clip: " + clip.name);
    }
}

public class TestClass1
{
    private IAudioHandler audioHandler;

    void Setup()
    {
        ServiceLocator<IAudioHandler>.Provide(new ConcreteAudioHandler());
        audioHandler = ServiceLocator<IAudioHandler>.GetItem;
        audioHandler.PlayAudio(null);

        MultiServiceLocator.Provide<IAudioHandler>(new ConcreteAudioHandler());
        IAudioHandler handler = MultiServiceLocator.GetService<IAudioHandler>();
        handler.PlayAudio(null);
    }
}
