using System;
using System.ComponentModel;
using LD.Contracts.Requests;

public class AsnDetailItem : INotifyPropertyChanged
{
    private int _asnDetailId;
    public int AsnDetailId
    {
        get => _asnDetailId;
        set { _asnDetailId = value; OnPropertyChanged(nameof(AsnDetailId)); }
    }

    private int _asnId;
    public int AsnId
    {
        get => _asnId;
        set { _asnId = value; OnPropertyChanged(nameof(AsnId)); }
    }

    private int _productId;
    public int ProductId
    {
        get => _productId;
        set { _productId = value; OnPropertyChanged(nameof(ProductId)); }
    }

    private string _partNumber = string.Empty;
    public string PartNumber
    {
        get => _partNumber;
        set { _partNumber = value; OnPropertyChanged(nameof(PartNumber)); }
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(nameof(Description)); }
    }

    private decimal _quantity;
    public decimal Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
    }

    private decimal? _standardQuantity;
    public decimal? StandardQuantity
    {
        get => _standardQuantity;
        set { _standardQuantity = value; OnPropertyChanged(nameof(StandardQuantity)); }
    }

    private decimal? _maximumQuantity;
    public decimal? MaximumQuantity
    {
        get => _maximumQuantity;
        set { _maximumQuantity = value; OnPropertyChanged(nameof(MaximumQuantity)); }
    }

    private string? _status;
    public string? Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(nameof(Status)); }
    }

    private string? _sd;
    public string? SD
    {
        get => _sd;
        set { _sd = value; OnPropertyChanged(nameof(SD)); }
    }

    private string? _lotNumber;
    public string? LotNumber
    {
        get => _lotNumber;
        set { _lotNumber = value; OnPropertyChanged(nameof(LotNumber)); }
    }

    private DateTime? _expirationDate;
    public DateTime? ExpirationDate
    {
        get => _expirationDate;
        set { _expirationDate = value; OnPropertyChanged(nameof(ExpirationDate)); }
    }

    private string? _customerReference;
    public string? CustomerReference
    {
        get => _customerReference;
        set { _customerReference = value; OnPropertyChanged(nameof(CustomerReference)); }
    }

    private decimal? _exchangeRate;
    public decimal? ExchangeRate
    {
        get => _exchangeRate;
        set { _exchangeRate = value; OnPropertyChanged(nameof(ExchangeRate)); }
    }

    private string? _purchaseOrder;
    public string? PurchaseOrder
    {
        get => _purchaseOrder;
        set { _purchaseOrder = value; OnPropertyChanged(nameof(PurchaseOrder)); }
    }

    private string? _customsDeclarationNumber;
    public string? CustomsDeclarationNumber
    {
        get => _customsDeclarationNumber;
        set { _customsDeclarationNumber = value; OnPropertyChanged(nameof(CustomsDeclarationNumber)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    // 🔁 convertir a DTO para guardar
    public AsnDetailRequest ToRequest()
    {
        return new AsnDetailRequest
        {
            AsnDetailId = this.AsnDetailId,
            AsnId = this.AsnId,
            ProductId = this.ProductId,
            PartNumber = this.PartNumber,
            Description = this.Description,
            Quantity = this.Quantity,
            StandardQuantity = this.StandardQuantity,
            MaximumQuantity = this.MaximumQuantity,
            Status = this.Status,
            SD = this.SD,
            LotNumber = this.LotNumber,
            ExpirationDate = this.ExpirationDate,
            CustomerReference = this.CustomerReference,
            ExchangeRate = this.ExchangeRate,
            PurchaseOrder = this.PurchaseOrder,
            CustomsDeclarationNumber = this.CustomsDeclarationNumber
        };
    }

    // 🔁 crear desde DTO
    public static AsnDetailItem FromRequest(AsnDetailRequest dto)
    {
        return new AsnDetailItem
        {
            AsnDetailId = dto.AsnDetailId,
            AsnId = dto.AsnId,
            ProductId = dto.ProductId,
            PartNumber = dto.PartNumber,
            Description = dto.Description,
            Quantity = dto.Quantity,
            StandardQuantity = dto.StandardQuantity,
            MaximumQuantity = dto.MaximumQuantity,
            Status = dto.Status,
            SD = dto.SD,
            LotNumber = dto.LotNumber,
            ExpirationDate = dto.ExpirationDate,
            CustomerReference = dto.CustomerReference,
            ExchangeRate = dto.ExchangeRate,
            PurchaseOrder = dto.PurchaseOrder,
            CustomsDeclarationNumber = dto.CustomsDeclarationNumber
        };
    }
}
