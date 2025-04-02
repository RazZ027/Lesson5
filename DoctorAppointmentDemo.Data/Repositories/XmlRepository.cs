using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace MyDoctorAppointment.Data.Repositories
{
    public class XmlRepository<T> : IDataStorage<T> where T : Auditable, new()
    {
        private readonly string _filePath;
        private int _lastId = 0;

        public XmlRepository()
        {
            var config = JsonConvert.DeserializeObject<dynamic>(
                File.ReadAllText("appsettings.json"));

            string entityName = typeof(T).Name;
            string configPath = $"XmlDatabase.{entityName}.Path";
            string? path = config?.SelectToken(configPath)?.ToString();

            if (string.IsNullOrWhiteSpace(path))
                throw new Exception($"XML path for {entityName} not found in config");

            _filePath = path;

            string? dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(_filePath))
                Save(new List<T>());

            var items = Load();
            if (items.Any())
                _lastId = items.Max(i => i.Id);
        }

        public T Create(T source)
        {
            var items = Load();
            source.Id = ++_lastId;
            source.CreatedAt = DateTime.Now;
            items.Add(source);
            Save(items);
            return source;
        }

        public bool Delete(int id)
        {
            var items = Load();
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) return false;

            items.Remove(item);
            Save(items);
            return true;
        }

        public IEnumerable<T> GetAll() => Load();

        public T? GetById(int id) => Load().FirstOrDefault(i => i.Id == id);

        public T Update(int id, T source)
        {
            var items = Load();
            var index = items.FindIndex(i => i.Id == id);
            if (index == -1) return null!;

            source.Id = id;
            source.UpdatedAt = DateTime.Now;
            items[index] = source;
            Save(items);
            return source;
        }

        private List<T> Load()
        {
            using var stream = new FileStream(_filePath, FileMode.Open);
            var serializer = new XmlSerializer(typeof(List<T>));
            return (List<T>)serializer.Deserialize(stream)!;
        }

        private void Save(List<T> data)
        {
            using var stream = new FileStream(_filePath, FileMode.Create);
            var serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(stream, data);
        }
    }
}
