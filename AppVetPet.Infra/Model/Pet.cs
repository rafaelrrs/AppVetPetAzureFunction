using Newtonsoft.Json;
using System;

namespace AppVetPet.Infra
{
    public class Pet
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "nome")]
        public String Nome { get; set; }

        [JsonProperty(PropertyName = "raca")]
        public String Raca { get; set; }

        [JsonProperty(PropertyName = "agendamento")]
        public DateTime Agendamento { get; set; }

        [JsonProperty(PropertyName = "nomedono")]
        public string NomeDono { get; set; }

        [JsonProperty(PropertyName = "telefonedono")]
        public string TelefoneDono { get; set; }

        [JsonProperty(PropertyName = "status")]
        public State Status { get; set; }

        [JsonProperty(PropertyName = "pk")]
        public string SKU { get; set; } = "sku";
    }

    public enum State
    {
        Confirmado = 1,
        Cancelado = 2,
        Finalizado = 3
    }
}
