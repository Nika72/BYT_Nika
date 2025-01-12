using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConsoleApp1.Services
{
    public abstract class SerializableObject<T> 
    {
        public static List<T> Instances { get; private set; } = new List<T>();

        public static void AddInstance(T instance)
        {
            if (Instances.Contains(instance))
            {
                Console.WriteLine($"Duplicate instance detected. Skipping addition: {instance}");
                return;
            }
            Instances.Add(instance);
            Console.WriteLine($"Added new instance: {instance}");
        }

        public static void SaveExtent()
        {
            var filePath = $"{typeof(T).Name}_Extent.xml";
            try
            {
                using var writer = new StringWriter();
                var serializer = new XmlSerializer(typeof(List<T>));
                
                serializer.Serialize(writer, Instances);
                
                var document = XDocument.Parse(writer.ToString());
                
                var root = document.Root;
                if (root != null)
                {
                    var metadata = new XElement("Metadata",
                        new XElement("GeneratedBy", "MyApp"),
                        new XElement("GeneratedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
                    root.AddFirst(metadata);
                }
                
                document.Save(filePath);
                Console.WriteLine($"Extent for {typeof(T).Name} saved to {filePath} with custom metadata.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent for {typeof(T).Name}: {ex.Message}");
            }
        }

        public static void LoadExtent()
        {
            var filePath = $"{typeof(T).Name}_Extent.xml";
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} not found. Creating an empty extent for {typeof(T).Name}.");
                Instances = new List<T>();
                return;
            }

            try
            {
                using var reader = new StreamReader(filePath);
                var serializer = new XmlSerializer(typeof(List<T>));
                Instances = (List<T>)serializer.Deserialize(reader) ?? new List<T>();
                Console.WriteLine($"Extent for {typeof(T).Name} loaded from {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent for {typeof(T).Name}: {ex.Message}");
                Instances = new List<T>();
            }
        }
    }
}