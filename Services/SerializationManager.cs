using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    public static class SerializationManager
    {
        public static void SerializeToXml<T>(List<T> objects, string filePath = null)
        {
            filePath ??= $"{typeof(T).Name}_Extent.xml";

            try
            {
                using var writer = new StreamWriter(filePath);
                var serializer = new XmlSerializer(typeof(List<T>));
                serializer.Serialize(writer, objects);
                Console.WriteLine($"Serialized {objects.Count} {typeof(T).Name} objects to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during XML serialization of {typeof(T).Name}: {ex.Message}");
            }
        }
       
        public static List<T> DeserializeFromXml<T>(string filePath = null)
        {
            filePath ??= $"{typeof(T).Name}_Extent.xml";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} not found. Returning an empty list for {typeof(T).Name}.");
                return new List<T>();
            }

            try
            {
                using var reader = new StreamReader(filePath);
                var serializer = new XmlSerializer(typeof(List<T>));
                var objects = (List<T>)serializer.Deserialize(reader) ?? new List<T>();
                Console.WriteLine($"Deserialized {objects.Count} {typeof(T).Name} objects from {filePath}.");
                return objects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during XML deserialization of {typeof(T).Name}: {ex.Message}");
                return new List<T>();
            }
        }
    }
}