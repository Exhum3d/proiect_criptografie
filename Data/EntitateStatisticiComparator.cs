namespace ProiectCriptografie.Data;

public class EntitateStatisticiComparator : IComparer<int>
{
   public int Compare(int frecventa1, int frecventa2)
   {
      return frecventa1 - frecventa2;
   } 
}