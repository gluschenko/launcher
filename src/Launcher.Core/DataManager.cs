using System;
using System.IO;

namespace Launcher.Core
{
    public class DataManager<T> where T : class
    {
        public string Path { get; set; }

        public DataManager(string path)
        {
            Path = path;
        }

        public T Load(Action<Exception> onError)
        {
            if (File.Exists(Path))
            {
                try
                {
                    return JsonUtility.FromJson<T>(File.ReadAllText(Path));
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                    return null;
                }
            }
            return Activator.CreateInstance<T>();
        }

        public T Load()
        {
            if (File.Exists(Path))
            {
                return JsonUtility.FromJson<T>(File.ReadAllText(Path));
            }
            return Activator.CreateInstance<T>();
        }

        public void Save(T obj, Action<Exception> onError)
        {
            try
            {
                string data = JsonUtility.ToJson(obj);
                File.WriteAllText(Path, data);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}
