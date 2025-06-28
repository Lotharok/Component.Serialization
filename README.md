# ISerializator

[🇪🇸 Español](docs/README.es.md) | [🇺🇸 English](README.md)

A generic and flexible interface for serialization and deserialization operations in .NET, with full support for both synchronous and asynchronous operations.

## 🚀 Features

- **Generic and flexible**: Support for different buffer types (byte[], string, Stream, etc.)
- **Asynchronous operations**: Async/await methods for better performance and responsiveness
- **Multiple formats**: Compatible with JSON, XML, Protobuf, and other serialization formats
- **Complete documentation**: XML documentation with usage examples
- **Exception handling**: Detailed error control with specific exceptions
- **High performance**: Optimized for large objects and high-throughput applications

## 📦 Installation

```bash
# Package Manager Console
Install-Package Component.Serialization

# .NET CLI
dotnet add package Component.Serialization

# PackageReference
<PackageReference Include="Component.Serialization" Version="1.0.0" />
```

## 🔧 Basic Usage

### Synchronous Serialization

```csharp
// Example with JSON serializer
ISerializator<string> jsonSerializer = new JsonSerializer();

var data = new Person { Name = "John", Age = 30 };

// Serialize
string json = jsonSerializer.Serialize(data);

// Deserialize
var restored = jsonSerializer.Deserialize<Person>(json);
```

### Asynchronous Serialization

```csharp
// Example with binary serializer for large objects
ISerializator<byte[]> binarySerializer = new ProtobufSerializer();

var largeData = new LargeDataSet { /* ... */ };

// Asynchronous serialization (recommended for objects >1MB)
byte[] buffer = await binarySerializer.SerializeAsync(largeData);

// Asynchronous deserialization
var restored = await binarySerializer.DeserializeAsync<LargeDataSet>(buffer);
```

## 🏗️ Suggested Implementations

### JSON Serializer
```csharp
public class JsonSerializer : ISerializator<string>
{
    public string Serialize<TValue>(TValue value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    public TValue Deserialize<TValue>(string buffer)
    {
        return System.Text.Json.JsonSerializer.Deserialize<TValue>(buffer);
    }
    
    // Async implementations...
}
```

### XML Serializer
```csharp
public class XmlSerializer : ISerializator<string>
{
    // Implementation using System.Xml.Serialization.XmlSerializer
}
```

### Protobuf Serializer
```csharp
public class ProtobufSerializer : ISerializator<byte[]>
{
    // Implementation using Google.Protobuf or similar
}
```

## 📋 Use Cases

### Web Applications
```csharp
// In a Web API controller
[ApiController]
public class DataController : ControllerBase
{
    private readonly ISerializator<string> _serializer;
    
    public DataController(ISerializator<string> serializer)
    {
        _serializer = serializer;
    }
    
    [HttpPost]
    public async Task<IActionResult> ProcessData([FromBody] InputData data)
    {
        // Asynchronous serialization to maintain responsiveness
        var serialized = await _serializer.SerializeAsync(data);
        
        // Process...
        
        return Ok();
    }
}
```

### Desktop/UI Applications
```csharp
// In a WPF/WinUI application
public async Task SaveDataAsync(UserData data)
{
    try
    {
        // Use async to keep UI responsive
        var buffer = await _serializer.SerializeAsync(data);
        await File.WriteAllBytesAsync("data.bin", buffer);
        
        ShowNotification("Data saved successfully");
    }
    catch (Exception ex)
    {
        ShowError($"Error saving data: {ex.Message}");
    }
}
```

### Distributed Systems
```csharp
// Distributed cache
public class CacheService
{
    private readonly ISerializator<byte[]> _serializer;
    private readonly IDistributedCache _cache;
    
    public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        var buffer = await _serializer.SerializeAsync(value);
        await _cache.SetAsync(key, buffer, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        });
    }
}
```

## ⚡ When to Use Asynchronous Methods

Asynchronous methods are particularly useful for:

- **Large objects** (>1MB): Avoid blocking the main thread
- **Web applications**: Improve throughput in concurrent scenarios
- **UI applications**: Keep the interface responsive
- **High-performance systems**: Optimize thread pool usage

## 🔍 Exception Handling

```csharp
try
{
    var result = await serializer.DeserializeAsync<MyClass>(buffer);
}
catch (ArgumentNullException)
{
    // Null buffer
}
catch (ArgumentException)
{
    // Empty buffer or invalid data
}
catch (InvalidOperationException)
{
    // Serialization/deserialization error
}
catch (InvalidCastException)
{
    // Incompatible type
}
catch (NotSupportedException)
{
    // Type not supported by the serializer
}
```

## 🧪 Testing

```csharp
[Test]
public async Task SerializeAsync_LargeObject_ShouldComplete()
{
    // Arrange
    var serializer = new ProtobufSerializer();
    var largeData = CreateLargeTestData();
    
    // Act
    var buffer = await serializer.SerializeAsync(largeData);
    var restored = await serializer.DeserializeAsync<TestData>(buffer);
    
    // Assert
    Assert.AreEqual(largeData.Id, restored.Id);
    Assert.AreEqual(largeData.Name, restored.Name);
}
```

## 🎯 Best Practices

1. **Use asynchronous methods** for large objects or in UI/Web applications
2. **Implement type validation** in your custom serializers
3. **Handle exceptions** appropriately according to your usage context
4. **Consider performance** when choosing buffer type (byte[] vs string vs Stream)
5. **Document supported types** in your specific implementations

## 🌟 Key Benefits

### Performance Optimization
- **Asynchronous operations** prevent UI freezing and improve web app throughput
- **Generic design** allows choosing the most efficient buffer type for your use case
- **Memory efficient** serialization for large datasets

### Developer Experience
- **Clean interface** with intuitive method names
- **Comprehensive XML documentation** with examples
- **Flexible implementation** supports various serialization formats
- **Predictable exception handling** for robust error management

### Scalability
- **Thread-safe design** suitable for multi-threaded environments
- **High-throughput optimized** for distributed systems
- **Resource efficient** async operations for better server utilization

## 📚 Additional Documentation

- [FAQ](docs/faq.md)

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

⭐ If you find this project useful, don't forget to give it a star!