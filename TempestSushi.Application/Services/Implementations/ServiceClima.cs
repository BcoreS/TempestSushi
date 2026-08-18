using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceClima : IServiceClima
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // Coordenadas fijas de San José, Costa Rica.
        private const double LATITUD = 9.9281;
        private const double LONGITUD = -84.0907;

        public ServiceClima(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ClimaDto?> ObtenerClimaEntregaAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // InvariantCulture evita que las coordenadas se conviertan
                // usando coma decimal según la configuración regional del equipo.
                var url =
                    $"https://api.open-meteo.com/v1/forecast" +
                    $"?latitude={LATITUD.ToString(CultureInfo.InvariantCulture)}" +
                    $"&longitude={LONGITUD.ToString(CultureInfo.InvariantCulture)}" +
                    $"&current=temperature_2m,weather_code";

                var respuesta = await client.GetStringAsync(url);

                using var json = JsonDocument.Parse(respuesta);

                var climaActual = json.RootElement.GetProperty("current");

                var temperatura =
                    climaActual.GetProperty("temperature_2m").GetDecimal();

                var codigoClima =
                    climaActual.GetProperty("weather_code").GetInt32();

                return new ClimaDto
                {
                    Temperatura = temperatura,
                    Descripcion = TraducirCodigoClima(codigoClima)
                };
            }
            catch
            {
                // Si el servicio externo no está disponible,
                // el detalle del pedido debe seguir funcionando.
                return null;
            }
        }

        private static string TraducirCodigoClima(int codigo)
        {
            return codigo switch
            {
                0 => "Despejado",
                1 or 2 or 3 => "Parcialmente nublado",
                45 or 48 => "Niebla",
                51 or 53 or 55 => "Llovizna",
                61 or 63 or 65 => "Lluvia",
                80 or 81 or 82 => "Lluvia fuerte",
                95 or 96 or 99 => "Tormenta",
                _ => "Condición variable"
            };
        }
    }
}