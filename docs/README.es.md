# ISerializator

[🇪🇸 Español](docs/README.es.md) | [🇺🇸 English](README.md)

Una interfaz genérica y flexible para operaciones de serialización y deserialización en .NET, con soporte completo para operaciones síncronas y asíncronas.

## 🚀 Características

- **Genérico y flexible**: Soporte para diferentes tipos de buffer (byte[], string, Stream, etc.)
- **Operaciones asíncronas**: Métodos async/await para mejor rendimiento y responsividad
- **Múltiples formatos**: Compatible con JSON, XML, Protobuf, y otros formatos de serialización
- **Documentación completa**: XML documentation con ejemplos de uso
- **Manejo de excepciones**: Control detallado de errores con excepciones específicas
- **Alto rendimiento**: Optimizado para objetos grandes y aplicaciones de alto throughput

## 📦 Instalación

```bash
# Package Manager Console
Install-Package YourProject.Serialization

# .NET CLI
dotnet add package YourProject.Serialization

# PackageReference
<PackageReference Include="YourProject.Serialization" Version="1.0.0" />
```

## 🔧 Uso Básico

### Serialización Síncrona

```csharp
// Ejemplo con serializador JSON
ISerializator<string> jsonSerializer = new JsonSerializer();

var data = new Person { Name = "Juan", Age = 30 };

// Serializar
string json = jsonSerializer.Serialize(data);

// Deserializar
var restored = jsonSerializer.Deserialize<Person>(json);
```

### Serialización Asíncrona

```csharp
// Ejemplo con serializador binario para objetos grandes
ISerializator<byte[]> binarySerializer = new ProtobufSerializer();

var largeData = new LargeDataSet { /* ... */ };

// Serialización asíncrona (recomendado para objetos >1MB)
byte[] buffer = await binarySerializer.SerializeAsync(largeData);

// Deserialización asíncrona
var restored = await binarySerializer.DeserializeAsync<LargeDataSet>(buffer);
```

## 🏗️ Implementaciones Sugeridas

### Serializador JSON
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
    
    // Implementaciones async...
}
```

### Serializador XML
```csharp
public class XmlSerializer : ISerializator<string>
{
    // Implementación usando System.Xml.Serialization.XmlSerializer
}
```

### Serializador Protobuf
```csharp
public class ProtobufSerializer : ISerializator<byte[]>
{
    // Implementación usando Google.Protobuf o similar
}
```

## 📋 Casos de Uso

### Aplicaciones Web
```csharp
// En un controlador Web API
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
        // Serialización asíncrona para mantener responsividad
        var serialized = await _serializer.SerializeAsync(data);
        
        // Procesar...
        
        return Ok();
    }
}
```

### Aplicaciones Desktop/UI
```csharp
// En una aplicación WPF/WinUI
public async Task SaveDataAsync(UserData data)
{
    try
    {
        // Usar async para mantener la UI responsiva
        var buffer = await _serializer.SerializeAsync(data);
        await File.WriteAllBytesAsync("data.bin", buffer);
        
        ShowNotification("Datos guardados correctamente");
    }
    catch (Exception ex)
    {
        ShowError($"Error al guardar: {ex.Message}");
    }
}
```

### Sistemas Distribuidos
```csharp
// Cache distribuido
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

## ⚡ Cuándo Usar Métodos Asíncronos

Los métodos asíncronos son especialmente útiles en:

- **Objetos grandes** (>1MB): Evitan bloquear el hilo principal
- **Aplicaciones web**: Mejoran el throughput en escenarios concurrentes
- **Aplicaciones UI**: Mantienen la interfaz responsiva
- **Sistemas de alto rendimiento**: Optimizan el uso del thread pool

## 🔍 Manejo de Excepciones

```csharp
try
{
    var result = await serializer.DeserializeAsync<MyClass>(buffer);
}
catch (ArgumentNullException)
{
    // Buffer nulo
}
catch (ArgumentException)
{
    // Buffer vacío o datos inválidos
}
catch (InvalidOperationException)
{
    // Error de serialización/deserialización
}
catch (InvalidCastException)
{
    // Tipo incompatible
}
catch (NotSupportedException)
{
    // Tipo no soportado por el serializador
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

## 🎯 Mejores Prácticas

1. **Usa métodos asíncronos** para objetos grandes o en aplicaciones UI/Web
2. **Implementa validación** de tipos en tus serializadores personalizados
3. **Maneja excepciones** apropiadamente según tu contexto de uso
4. **Considera el rendimiento** al elegir el tipo de buffer (byte[] vs string vs Stream)
5. **Documenta tipos soportados** en tus implementaciones específicas

## 📚 Documentación Adicional

- [FAQ](docs/faq.md)

## 🤝 Contribución

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

---

⭐ Si este proyecto te es útil, ¡no olvides darle una estrella!