using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.API.Model;

public class CatalogBrand
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; }

    /// <summary>
    /// Gibt einen Gutscheinwert (in Prozent) zurück, wenn die Marke "Solstix" ist (case-insensitive).
    /// Der Wert liegt zufällig zwischen 10 und 30. Sonst 0.
    /// </summary>
    public int VoucherPercent
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Brand) && Brand.Equals("Solstix", System.StringComparison.OrdinalIgnoreCase))
            {
                var rnd = new System.Random();
                return rnd.Next(10, 31); // 10 bis inkl. 30
            }
            return 0;
        }
    }
}
