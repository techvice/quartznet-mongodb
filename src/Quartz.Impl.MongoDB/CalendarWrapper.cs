using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Quartz.Impl.MongoDB
{
    public class CalendarWrapper : IBsonSerializable
    {
        public string Name { get; set; }
        public ICalendar Calendar { get; set; }

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            var item = new CalendarWrapper();
            
            bsonReader.ReadStartDocument();
            item.Name = bsonReader.ReadString("_id");
            var binaryData = bsonReader.ReadBinaryData("ContentStream");
            item.Calendar = (ICalendar)new BinaryFormatter().Deserialize(new MemoryStream(binaryData.Bytes));
            bsonReader.ReadEndDocument();
            
            return item;
        }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            id = Name;
            idNominalType = typeof(string);
            idGenerator = null;

            return true;
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, IBsonSerializationOptions options)
        {
            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("_id", Name);
            var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, Calendar);
            bsonWriter.WriteBinaryData("ContentStream", new BsonBinaryData(stream.ToArray(), BsonBinarySubType.Binary));
            bsonWriter.WriteEndDocument();
        }

        public void SetDocumentId(object id)
        {
            throw new NotImplementedException();
        }
    }
}
