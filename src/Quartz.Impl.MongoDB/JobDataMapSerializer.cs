using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Quartz.Impl.MongoDB
{
    public class JobDataMapSerializer : IBsonSerializer
    {
        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
	        string message;
            if (nominalType != typeof(JobDataMap) || actualType != typeof(JobDataMap))
            {
                message = string.Format("Can't deserialize a {0} with {1}.", nominalType.FullName, GetType().Name);
                throw new BsonSerializationException(message);
            }

            var bsonType = bsonReader.CurrentBsonType;
            switch (bsonType)
            {
	            case BsonType.Document:
		            var item = new JobDataMap();
		            bsonReader.ReadStartDocument();

		            while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
		            {
			            string key = bsonReader.ReadName();
			            var value = BsonSerializer.Deserialize<object>(bsonReader);
			            item.Add(key, value);
		            }

		            bsonReader.ReadEndDocument();

		            return item;

				case BsonType.Null:
					bsonReader.ReadNull();
					return null;

				default:
					message = string.Format("Can't deserialize a {0} from BsonType {1}.", nominalType.FullName, bsonType);
					throw new BsonSerializationException(message);
            }
        }

        public object Deserialize(global::MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            return Deserialize(bsonReader, nominalType, nominalType, options);
        }

        public void Serialize(global::MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var item = (JobDataMap)value;
            bsonWriter.WriteStartDocument();

            foreach (string key in item.Keys)
            {
                bsonWriter.WriteName(key);
                BsonSerializer.Serialize(bsonWriter, item[key]);
            }
            
            bsonWriter.WriteEndDocument();
        }

		public IBsonSerializationOptions GetDefaultSerializationOptions()
		{
			throw new NotImplementedException();
		}
    }
}
