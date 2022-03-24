namespace ProiectCriptografie.Data;

public class StatisticiService
{
    private const int NrLitereAlfabet = 31;

    private PriorityQueue<EntitateStatistici, int>? _huffmanHeap;

    private readonly char[] _litereAlfabet = new[]
    {
        'a', 'ă', 'â', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'î', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 'ș', 't', 'ț', 'u', 'v', 'w', 'x', 'y', 'z'
    };

    public int TotalFrecvente { get; set; }
    public double Entropie { get; set; }
    public double LungimeMedie { get; set; }
    public double Eficienta { get; set; }
    public double Redundanta { get; set; }
    public string? Sursa { get; set; }
    public List<EntitateStatistici> ListaStatistici = new();
    

    public StatisticiService()
    {
        for (int i = 0; i < NrLitereAlfabet; i++)
        {
            ListaStatistici.Add(new EntitateStatistici()
            {
                ID = i + 1,
                CaracterAlfabet = _litereAlfabet[i],
                Frecventa = 0,
                Probabilitate = 0,
                Stanga = null,
                Dreapta = null,
                ProgresValoareUnitara = 0
            });
        }
    }

    public List<EntitateStatistici> InitializareListaStatistici()
    {
        return ListaStatistici;
    }

    private int GetIndexCaracter(char caracter)
    {
        for (int i = 0; i < NrLitereAlfabet; i++)
        {
            if (_litereAlfabet[i] == caracter)
            {
                return i;
            }
        }

        return -1;
    }

    public void ProcesareSursa(string sursa)
    {
        if (string.IsNullOrWhiteSpace(sursa))
            return;


        Sursa = sursa.ToLower();

        foreach (var caracter in Sursa)
        {
            var pozitie = GetIndexCaracter(caracter);

            if (pozitie < 0)
            {
                continue;
            }

            ListaStatistici[pozitie].Frecventa++;
        }

        GetCoduriHuffman();

        double progres = 0;


        foreach (var elem in ListaStatistici)
        {
            elem.Probabilitate = (double) elem.Frecventa / TotalFrecvente;
            progres += elem.Probabilitate;
            elem.ProgresValoareUnitara = progres;
        }


        CalculEntropie();
        CalculLungimeMedie();
        CalculEficienta();
        CalculRedundanta();
    }

    public void ParcurgereHuffmanHeap(EntitateStatistici? varf, string cod)
    {
        if (varf is null)
            return;

        if (varf.CaracterAlfabet != '*')
        {
            int indexCaracterCurent = GetIndexCaracter(varf.CaracterAlfabet);
            ListaStatistici[indexCaracterCurent].CodHuffman = cod;
        }

        ParcurgereHuffmanHeap(varf.Stanga, cod + "0");
        ParcurgereHuffmanHeap(varf.Dreapta, cod + "1");
    }

    public void GetCoduriHuffman()
    {
        _huffmanHeap = new PriorityQueue<EntitateStatistici, int>(new EntitateStatisticiComparator());
        EntitateStatistici? varf = null;


        foreach (var element in ListaStatistici)
        {
            _huffmanHeap.Enqueue(element, element.Frecventa);
        }

        while (_huffmanHeap.Count > 1)
        {
            var stanga = _huffmanHeap.Dequeue();
            var dreapta = _huffmanHeap.Dequeue();

            varf = new EntitateStatistici
            {
                Frecventa = stanga.Frecventa + dreapta.Frecventa,
                CaracterAlfabet = '*',
                Stanga = stanga,
                Dreapta = dreapta,
                ID = -1,
                Probabilitate = -1
            };

            _huffmanHeap.Enqueue(varf, varf.Frecventa);
        }

        if (varf != null)
        {
            TotalFrecvente = varf.Frecventa;
            ParcurgereHuffmanHeap(varf, string.Empty);
        }
    }

    public void CalculEntropie()
    {
        Entropie = 0;

        foreach (var element in ListaStatistici)
        {
            if (element.Frecventa == 0)
                continue;
            Entropie += element.Probabilitate * Math.Log2(1 / element.Probabilitate);
        }
    }

    public void CalculLungimeMedie()
    {
        LungimeMedie = 0;

        foreach (var element in ListaStatistici)
        {
            if (element.CodHuffman != null) LungimeMedie += element.Probabilitate * element.CodHuffman.Length;
        }
    }

    public void CalculEficienta()
    {
        Eficienta = Entropie / LungimeMedie;
    }

    public void CalculRedundanta()
    {
        Redundanta = 1 - Eficienta;
    }
}