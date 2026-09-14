using CowCount.Models;
using CowCount.Services.Interfaces;
using System.IO;
using System.Text.Json;

namespace CowCount.Services.Implementations
{
    /// <summary>
    /// Сервис для работы с файлом, в котором хранятся все данные.
    /// </summary>
    public class SavedDataService : ISavedDataService
    {
        private const string DataFileName = "data.json";

        private Data? _data;

        public async Task<Data> GetDataAsync(CancellationToken token)
        {
            if (_data is not null)
            {
                return _data;
            }

            try
            {
                token.ThrowIfCancellationRequested();

                await using var stream = File.Open(DataFileName,
                    FileMode.OpenOrCreate,
                    FileAccess.Read);

                _data = await JsonSerializer.DeserializeAsync<Data>(
                    stream,
                    JsonSerializerOptions.Default,
                    token).ConfigureAwait(false);

                if (_data is null)
                {
                    _data = GetDefaultData();
                    await UpdateDataAsync(_data, token).ConfigureAwait(false);
                }

                return _data;
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }

            _data = GetDefaultData();
            return _data;
        }

        public async Task UpdateDataAsync(Data config, CancellationToken token)
        {
            _data = config;

            try
            {
                token.ThrowIfCancellationRequested();

                var directory = Path.GetDirectoryName(DataFileName);

                if (directory is not null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                await using var stream = File.Create(DataFileName);
                await JsonSerializer.SerializeAsync(stream,
                    _data,
                    options,
                    token).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        private static Data GetDefaultData()
        {
            return new Data();
        }
    }
}
