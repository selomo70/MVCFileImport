namespace MVCFileImport.Models
{
    public static class Processor
    {

        public static (List<OutputModel>, List<OutputModel2>) ProcessCsv(string filePath)
        {
            var valueCounts = new Dictionary<string, int>();
            var uniqueCities = new HashSet<string>(); // Store unique addresses

            using (var reader = new StreamReader(filePath))
            {
                // Read and discard the header row
                var header = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split into columns (assuming comma-separated)
                    var values = line.Split(',');

                    if (values.Length < 3) continue; // Skip invalid lines

                    var firstColumn = values[0].Trim();
                    var secondColumn = values[1].Trim();
                    var thirdColumn = values[2].Trim(); // address

                    // Count occurrences of first and second columns
                    CountValue(valueCounts, firstColumn);
                    CountValue(valueCounts, secondColumn);

                    // Collect unique addresses
                    if (!string.IsNullOrEmpty(thirdColumn))
                        uniqueCities.Add(thirdColumn);
                }
            }

            // Convert dictionary to sorted list
            var sortedEntries = valueCounts.OrderByDescending(kvp => kvp.Value)
                                           .Select(kvp => new OutputModel
                                           {
                                               Name = kvp.Key,
                                               Count = kvp.Value
                                           })
                                           .ToList();

            // Convert unique address to a list
            var output2 = uniqueCities.Select(city => new OutputModel2 { Address = city }).ToList();

            return (sortedEntries, output2);
        }
        private static void CountValue(Dictionary<string, int> valueCounts, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (valueCounts.ContainsKey(value))
                    valueCounts[value]++;
                else
                    valueCounts[value] = 1;
            }
        }
    }
}
