using Newtonsoft.Json;

namespace Domain.Serialization; 



public static class JsonSettings {

	public static readonly JsonSerializerSettings JsonSerializerSettings = new() {
		TypeNameHandling = TypeNameHandling.All,
		Formatting = Formatting.Indented
	};

}