using System;
using System.Threading.Tasks;

namespace Component.Serialization.Contract
{
   /// <summary>
   /// Defines a contract for serialization and deserialization operations.
   /// Provides both synchronous and asynchronous methods for converting objects
   /// to and from a buffer format.
   /// </summary>
   /// <typeparam name="TBuffer">
   /// The type of buffer used for serialized data (e.g., byte[], string, Stream).
   /// </typeparam>
   /// <remarks>
   /// This interface supports various buffer types to accommodate different serialization
   /// formats and storage mechanisms. Common implementations might use byte arrays for
   /// binary formats, strings for text-based formats, or streams for large data sets.
   ///
   /// The asynchronous methods are particularly useful for:
   /// - Large object serialization (>1MB)
   /// - Web applications handling multiple concurrent requests
   /// - UI applications requiring responsive interfaces
   /// - Distributed systems with high-throughput requirements
   ///
   /// Example implementations:
   /// - ProtobufSerializer : ISerializator&lt;byte[]&gt;
   /// - JsonSerializer : ISerializator&lt;string&gt;
   /// - XmlSerializer : ISerializator&lt;string&gt;.
   /// </remarks>
   /// <example>
   /// <code>
   /// // Usage example with a binary serializer
   /// ISerializator&lt;byte[]&gt; serializer = new ProtobufSerializer();
   ///
   /// // Synchronous serialization
   /// var data = new MyClass { Name = "Example" };
   /// byte[] buffer = serializer.Serialize(data);
   /// var restored = serializer.Deserialize&lt;MyClass&gt;(buffer);
   ///
   /// // Asynchronous serialization for large objects
   /// byte[] asyncBuffer = await serializer.SerializeAsync(largeData);
   /// var asyncRestored = await serializer.DeserializeAsync&lt;MyClass&gt;(asyncBuffer);
   /// </code>
   /// </example>
   public interface ISerializator<TBuffer>
   {
      /// <summary>
      /// Serializes an object into the specified buffer format.
      /// </summary>
      /// <typeparam name="TValue">
      /// The type of object to serialize. Must be serializable by the implementing serializer.
      /// </typeparam>
      /// <param name="value">
      /// The object to serialize. Cannot be null for most implementations.
      /// </param>
      /// <returns>
      /// A buffer containing the serialized representation of the object.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="value"/> is null and the implementation doesn't support null values.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// Thrown when the object cannot be serialized due to type constraints or serialization errors.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// Thrown when the type <typeparamref name="TValue"/> is not supported by the serializer.
      /// </exception>
      /// <remarks>
      /// This method performs synchronous serialization and may block the calling thread
      /// for large objects. Consider using <see cref="SerializeAsync{TValue}(TValue)"/>
      /// for better performance with large data sets or in UI applications.
      /// </remarks>
      TBuffer Serialize<TValue>(TValue value);

      /// <summary>
      /// Deserializes an object from the specified buffer format.
      /// </summary>
      /// <typeparam name="TValue">
      /// The type of object to deserialize to. Must match the original serialized type.
      /// </typeparam>
      /// <param name="buffer">
      /// The buffer containing the serialized data. Cannot be null or empty.
      /// </param>
      /// <returns>
      /// The deserialized object of type <typeparamref name="TValue"/>.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="buffer"/> is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// Thrown when <paramref name="buffer"/> is empty or contains invalid data.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// Thrown when the buffer cannot be deserialized to the specified type.
      /// </exception>
      /// <exception cref="InvalidCastException">
      /// Thrown when the deserialized object cannot be cast to <typeparamref name="TValue"/>.
      /// </exception>
      /// <remarks>
      /// This method performs synchronous deserialization and may block the calling thread
      /// for large buffers. Consider using <see cref="DeserializeAsync{TValue}(TBuffer)"/>
      /// for better performance with large data sets or in UI applications.
      ///
      /// The type <typeparamref name="TValue"/> must exactly match the type that was
      /// originally serialized, or be compatible with it depending on the serializer implementation.
      /// </remarks>
      TValue Deserialize<TValue>(TBuffer buffer);

      /// <summary>
      /// Asynchronously serializes an object into the specified buffer format.
      /// </summary>
      /// <typeparam name="TValue">
      /// The type of object to serialize. Must be serializable by the implementing serializer.
      /// </typeparam>
      /// <param name="value">
      /// The object to serialize. Cannot be null for most implementations.
      /// </param>
      /// <returns>
      /// A task that represents the asynchronous serialization operation.
      /// The task result contains a buffer with the serialized representation of the object.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="value"/> is null and the implementation doesn't support null values.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// Thrown when the object cannot be serialized due to type constraints or serialization errors.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// Thrown when the type <typeparamref name="TValue"/> is not supported by the serializer.
      /// </exception>
      /// <remarks>
      /// This method performs asynchronous serialization, allowing the calling thread to remain
      /// free for other operations. This is particularly beneficial for:
      /// - Large objects (>1MB) that take significant time to serialize
      /// - UI applications where maintaining responsiveness is critical
      /// - Web applications handling multiple concurrent requests
      /// - High-throughput scenarios where thread pool efficiency matters
      ///
      /// The actual implementation may use Task.Run() internally for CPU-bound serialization
      /// or true async I/O for stream-based operations.
      /// </remarks>
      Task<TBuffer> SerializeAsync<TValue>(TValue value);

      /// <summary>
      /// Asynchronously deserializes an object from the specified buffer format.
      /// </summary>
      /// <typeparam name="TValue">
      /// The type of object to deserialize to. Must match the original serialized type.
      /// </typeparam>
      /// <param name="buffer">
      /// The buffer containing the serialized data. Cannot be null or empty.
      /// </param>
      /// <returns>
      /// A task that represents the asynchronous deserialization operation.
      /// The task result contains the deserialized object of type <typeparamref name="TValue"/>.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="buffer"/> is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// Thrown when <paramref name="buffer"/> is empty or contains invalid data.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// Thrown when the buffer cannot be deserialized to the specified type.
      /// </exception>
      /// <exception cref="InvalidCastException">
      /// Thrown when the deserialized object cannot be cast to <typeparamref name="TValue"/>.
      /// </exception>
      /// <remarks>
      /// This method performs asynchronous deserialization, allowing the calling thread to remain
      /// free for other operations. This is particularly beneficial for:
      /// - Large buffers that take significant time to deserialize
      /// - UI applications where maintaining responsiveness is critical
      /// - Web applications handling multiple concurrent requests
      /// - High-throughput scenarios where thread pool efficiency matters
      ///
      /// The type <typeparamref name="TValue"/> must exactly match the type that was
      /// originally serialized, or be compatible with it depending on the serializer implementation.
      ///
      /// The actual implementation may use Task.Run() internally for CPU-bound deserialization
      /// or true async I/O for stream-based operations.
      /// </remarks>
      Task<TValue> DeserializeAsync<TValue>(TBuffer buffer);
   }
}
