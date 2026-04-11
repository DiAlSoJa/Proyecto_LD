using System;
using System.ComponentModel;
using LD.Contracts.ASN;
using LD.Contracts.Requests;

public class AsnReceiptItem : INotifyPropertyChanged
{
    private int _asnReceiptDetailId;
    public int AsnReceiptDetailId
    {
        get => _asnReceiptDetailId;
        set { _asnReceiptDetailId = value; OnPropertyChanged(nameof(AsnReceiptDetailId)); }
    }

    private int _asnId;
    public int AsnId
    {
        get => _asnId;
        set { _asnId = value; OnPropertyChanged(nameof(AsnId)); }
    }

    private int _asnDetailId;
    public int AsnDetailId
    {
        get => _asnDetailId;
        set { _asnDetailId = value; OnPropertyChanged(nameof(AsnDetailId)); }
    }

    private int? _productId;
    public int? ProductId
    {
        get => _productId;
        set { _productId = value; OnPropertyChanged(nameof(ProductId)); }
    }

    private int? _standardId;
    public int? StandardId
    {
        get => _standardId;
        set { _standardId = value; OnPropertyChanged(nameof(StandardId)); }
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

    private string? _sd;
    public string? SD
    {
        get => _sd;
        set { _sd = value; OnPropertyChanged(nameof(SD)); }
    }

    private decimal? _receivedQuantity;
    public decimal? ReceivedQuantity
    {
        get => _receivedQuantity;
        set { _receivedQuantity = value; OnPropertyChanged(nameof(ReceivedQuantity)); }
    }

    private string? _status;
    public string? Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(nameof(Status)); }
    }

    private int? _locationId;
    public int? LocationId
    {
        get => _locationId;
        set { _locationId = value; OnPropertyChanged(nameof(LocationId)); }
    }

    private string? _locationCode;
    public string? LocationCode
    {
        get => _locationCode;
        set { _locationCode = value; OnPropertyChanged(nameof(LocationCode)); }
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

    private string? _reference;
    public string? Reference
    {
        get => _reference;
        set { _reference = value; OnPropertyChanged(nameof(Reference)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void ApplyDefaultsFromDetail(AsnDetailItem detailItem, int asnId)
    {
        AsnId = asnId;
        AsnDetailId = detailItem.AsnDetailId;
        ProductId = detailItem.ProductId;

        if (string.IsNullOrWhiteSpace(PartNumber))
            PartNumber = detailItem.PartNumber;

        if (string.IsNullOrWhiteSpace(Description))
            Description = detailItem.Description;

        if (string.IsNullOrWhiteSpace(SD))
            SD = detailItem.SD;
    }

    public AsnReceiptRequest ToRequest()
    {
        return new AsnReceiptRequest
        {
            AsnReceiptDetailId = AsnReceiptDetailId,
            AsnDetailId = AsnDetailId,
            ProductId = ProductId,
            StandardId = StandardId,
            PartNumber = PartNumber,
            Description = Description,
            StandardQuantity = StandardQuantity,
            MaximumQuantity = MaximumQuantity,
            SD = SD,
            ReceivedQuantity = ReceivedQuantity,
            Status = Status,
            LocationId = LocationId,
            LocationCode = LocationCode,
            LotNumber = LotNumber,
            ExpirationDate = ExpirationDate,
            Reference = Reference
        };
    }

    public static AsnReceiptItem FromDto(AsnReceiptDetailDto dto)
    {
        return new AsnReceiptItem
        {
            AsnReceiptDetailId = dto.AsnReceiptDetailId,
            AsnDetailId = dto.AsnDetailId,
            ProductId = dto.ProductId,
            StandardId = dto.StandardId,
            PartNumber = dto.PartNumber,
            Description = dto.Description,
            StandardQuantity = dto.StandardQuantity,
            MaximumQuantity = dto.MaximumQuantity,
            SD = dto.SD,
            ReceivedQuantity = dto.ReceivedQuantity,
            Status = dto.Status,
            LocationId = dto.LocationId,
            LocationCode = dto.LocationCode,
            LotNumber = dto.LotNumber,
            ExpirationDate = dto.ExpirationDate,
            Reference = dto.Reference
        };
    }

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
