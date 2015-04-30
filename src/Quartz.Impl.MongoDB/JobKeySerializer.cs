﻿using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Quartz.Impl.MongoDB
{
    public class JobKeySerializer : IBsonSerializer
    {
        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            if (nominalType != typeof(JobKey) || actualType != typeof(JobKey))
            {
                throw new BsonSerializationException(
					string.Format("Can't deserialize a {0} from {1}.", nominalType.FullName, GetType().Name));
            }

            var bsonType = bsonReader.CurrentBsonType;
            switch (bsonType)
            {
	            case BsonType.Document:
		            bsonReader.ReadStartDocument();
		            var item = new JobKey(bsonReader.ReadString("Name"), bsonReader.ReadString("Group"));
		            bsonReader.ReadEndDocument();

		            return item;
	            case BsonType.Null:
		            bsonReader.ReadNull();
		            return null;
	            default:
		            throw new BsonSerializationException(
			            string.Format("Can't deserialize a {0} from BsonType {1}.", nominalType.FullName, bsonType));
            }
        }

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            return Deserialize(bsonReader, nominalType, nominalType, options);
        }

		public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			var item = (JobKey)value;

			bsonWriter.WriteStartDocument();
			bsonWriter.WriteString("Name", item.Name);
			bsonWriter.WriteString("Group", item.Group);
			bsonWriter.WriteEndDocument();
		}

        public IBsonSerializationOptions GetDefaultSerializationOptions()
        {
            throw new NotImplementedException();
        }
    }
}