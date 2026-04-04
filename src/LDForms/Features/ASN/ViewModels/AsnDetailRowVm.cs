using System;
using System.ComponentModel;

namespace LD.FormsX.ViewModels.ASN
{
    

  
        public class AsnDetailRowVm : INotifyPropertyChanged
        {
            private int? _productId;
            private string _numeroParte = string.Empty;
            private string _descripcion = string.Empty;
            private decimal? _cantidad;
            private string _status = string.Empty;
            private string _sd = string.Empty;
            private string _numeroLote = string.Empty;
            private DateTime? _fechaCaducidad;
            private string _referenciaCliente = string.Empty;
            private decimal? _tipoCambio;
            private string _ordenCompra = string.Empty;
            private string _pedimento = string.Empty;
            private string _split = string.Empty;

            public int? ProductId
            {
                get => _productId;
                set { _productId = value; OnPropertyChanged(nameof(ProductId)); }
            }

            public string NumeroParte
            {
                get => _numeroParte;
                set { _numeroParte = value; OnPropertyChanged(nameof(NumeroParte)); }
            }

            public string Descripcion
            {
                get => _descripcion;
                set { _descripcion = value; OnPropertyChanged(nameof(Descripcion)); }
            }

            public decimal? Cantidad
            {
                get => _cantidad;
                set { _cantidad = value; OnPropertyChanged(nameof(Cantidad)); }
            }

            public string Status
            {
                get => _status;
                set { _status = value; OnPropertyChanged(nameof(Status)); }
            }

            public string Sd
            {
                get => _sd;
                set { _sd = value; OnPropertyChanged(nameof(Sd)); }
            }

            public string NumeroLote
            {
                get => _numeroLote;
                set { _numeroLote = value; OnPropertyChanged(nameof(NumeroLote)); }
            }

            public DateTime? FechaCaducidad
            {
                get => _fechaCaducidad;
                set { _fechaCaducidad = value; OnPropertyChanged(nameof(FechaCaducidad)); }
            }

            public string ReferenciaCliente
            {
                get => _referenciaCliente;
                set { _referenciaCliente = value; OnPropertyChanged(nameof(ReferenciaCliente)); }
            }

            public decimal? TipoCambio
            {
                get => _tipoCambio;
                set { _tipoCambio = value; OnPropertyChanged(nameof(TipoCambio)); }
            }

            public string OrdenCompra
            {
                get => _ordenCompra;
                set { _ordenCompra = value; OnPropertyChanged(nameof(OrdenCompra)); }
            }

            public string Pedimento
            {
                get => _pedimento;
                set { _pedimento = value; OnPropertyChanged(nameof(Pedimento)); }
            }

            public string Split
            {
                get => _split;
                set { _split = value; OnPropertyChanged(nameof(Split)); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string propertyName)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }