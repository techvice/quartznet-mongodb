﻿using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Quartz.Impl.MongoDB
{
    public class TriggerKeySerializer : IBsonSerializer
    {
        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            if (nominalType != typeof(TriggerKey) || actualType != typeof(TriggerKey))
            {
                var message = string.Format("Can't deserialize a {0} from {1}.", nominalType.FullName, GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            switch (bsonType)
            {
	            case BsonType.Document:
		            bsonReader.ReadStartDocument();
		            string name = bsonReader.ReadString("Name");
		            string group = bsonReader.ReadString("Group");
		            bsonReader.ReadEndDocument();

		            return new TriggerKey(name, group);
	            case BsonType.Null:
		            bsonReader.ReadNull();
		            return null;
	            default:
		            var message = string.Format("Can't deserialize a {0} from BsonType {1}.", nominalType.FullName, bsonType);
		            throw new BsonSerializationException(message);
            }
        }

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            return Deserialize(bsonReader, nominalType, nominalType, options);
        }

        public IBsonSerializationOptions GetDefaultSerializationOptions()
        {
            throw new NotImplementedException();
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var item = (TriggerKey)value;

            bsonWriter.WriteStartDocument();
            bsonWriter.WriteString("Name", item.Name);
            bsonWriter.WriteString("Group", item.Group);
            bsonWriter.WriteEndDocument();
        }
    }
}