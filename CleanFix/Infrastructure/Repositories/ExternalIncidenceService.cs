using System.Text;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Repositories;
public class ExternalIncidenceService : IExternalIncidenceRepository
{
    private readonly HttpClient _httpClient;

    public ExternalIncidenceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result> UpdateIncidenceCost(int incidenceId, string companyName)
    {
        try
        {
            var requestBody = new
            {
                companyName,
                date = DateTime.UtcNow,
                statusId = "R"

            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"QFIncidence/{incidenceId}", content);

            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Failure(new[] {
                    $"External API request failed with status {response.StatusCode}: {errorContent}"
                });
            }
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure(new[] {
                $"Network error while updating request cost: {ex.Message}"
            });
        }
        catch (TaskCanceledException ex)
        {
            return Result.Failure(new[] {
                $"Request timeout while updating request cost: {ex.Message}"
            });
        }
        catch (Exception ex)
        {
            return Result.Failure(new[] {
                $"Unexpected error while updating request cost: {ex.Message}"
            });
        }
    }
}
