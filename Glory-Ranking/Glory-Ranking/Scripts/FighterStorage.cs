using System;
using System.IO;
using System.Text.Json;

namespace Glory_Ranking
{
    public static class FighterStorage
    {
        private static readonly string FilePath = "Fighters.json";

        public static void Save(FighterData _data)
        {
            try
            {
                var _options = new JsonSerializerOptions { WriteIndented = true };
                string _json = JsonSerializer.Serialize(_data, _options);
                File.WriteAllText(FilePath, _json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving fighters: {ex.Message}");
            }
        }

        public static FighterData Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new FighterData { Fighters = Array.Empty<Fighter>() };

                string _json = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(_json))
                    return new FighterData { Fighters = Array.Empty<Fighter>() };

                var _data = JsonSerializer.Deserialize<FighterData>(_json);
                if (_data != null && _data.Fighters != null)
                    return _data;

                var _fightersOnly = JsonSerializer.Deserialize<Fighter[]>(_json);
                if (_fightersOnly != null)
                    return new FighterData { Fighters = _fightersOnly };

                return new FighterData { Fighters = Array.Empty<Fighter>() };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}. Resetting file...");
                Reset();
                return new FighterData { Fighters = Array.Empty<Fighter>() };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error loading fighters: {ex.Message}");
                return new FighterData { Fighters = Array.Empty<Fighter>() };
            }
        }

        public static void Reset()
        {
            try
            {
                var _emptyData = new FighterData { Fighters = Array.Empty<Fighter>() };
                Save(_emptyData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting fighters: {ex.Message}");
            }
        }
    }
}
