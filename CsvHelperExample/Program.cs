using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

const string fileName = "output.csv";
const string expectedHeader = "MyFirstUsageMyDateTime,MyFirstUsageMyDescription,MySecondUsageMyDateTime,MySecondUsageMyDescription";

// Main --

var mySample = new MyContainer
{
    MyFirstUsage = new Foo
    {
        MyDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
        MyDescription = "This is the first usage"
    },
    MySecondUsage = new Foo
    {
        MyDateTime = DateTime.UtcNow.AddDays(1),
        MyDescription = "This is the second usage"
    }
};

File.Delete(fileName);
await using var streamWriter = new StreamWriter(fileName, false);
await using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
{
    ShouldQuote = _ => true,
});

await csvWriter.WriteRecordsAsync(new[] { mySample });

var actualHeader = string.Join(',', csvWriter.HeaderRecord);

Console.WriteLine(expectedHeader.Equals(actualHeader)
    ? "The header matches the expected output"
    : $"The header does not match. Actual: {actualHeader} Expected: {expectedHeader}");

// -- Main


// Sample classes --

public class Foo
{
    public DateTime? MyDateTime { get; set; }
    public string? MyDescription { get; set; }
}

public class MyContainer
{
    public Foo? MyFirstUsage { get; set; }
    public Foo? MySecondUsage { get; set; }
}

// -- Sample classes