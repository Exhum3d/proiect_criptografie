namespace ProiectCriptografie.Data;

public class EntitateStatistici
{
    public int ID { get; set; }
    public char CaracterAlfabet { get; set; }
    public int Frecventa { get; set; }
    public double Probabilitate { get; set; }
    public double ProgresValoareUnitara { get; set; }
    public EntitateStatistici? Stanga { get; set; }
    public EntitateStatistici? Dreapta { get; set; }
    public string? CodHuffman { get; set; }
}