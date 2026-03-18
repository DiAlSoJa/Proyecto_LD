using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LD.Client.Services;
using LD.Contracts.Category;
using LD.Contracts.Client;
using LD.Contracts.Currency;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Units;
using LD.Forms.Classes;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;



namespace LD.Forms.Views.Forms;

public partial class FrmCatalogos : Form
{
    private readonly DialogFormService _dialogFormService;

    private readonly UnitService _unitService;
    private readonly InventaryStatusService _statusService;
    private readonly CurrencyService _currencyService;
    private readonly CategoryService _categoryService;
    private GridFilter<UnitDto> _gridFilterU;
    private BindingSource _unitsBinding = new();
    private UnitDto? selectedUnit { get; set; }

    private GridFilter<InventaryStatusDto> _gridFilterS;
    private BindingSource _statusBinding = new();
    private InventaryStatusDto? selectedStatus { get; set; }

    private GridFilter<CurrencyDto> _gridFilterC;
    private BindingSource _currencyBinding = new();
    private CurrencyDto? selectedCurrency { get; set; }

    private GridFilter<CategoryDto> _gridFilterCa;
    private BindingSource _categoryBinding = new();
    private CategoryDto? selectedCategory { get; set; }




    public FrmCatalogos(UnitService unitService, InventaryStatusService statusService,
        CurrencyService currencyService, CategoryService categoryService,
        DialogFormService dialogFormService)
    {
        InitializeComponent();
        _dialogFormService = dialogFormService;
        _unitService = unitService;
        _statusService = statusService;
        _currencyService = currencyService;
        _categoryService = categoryService;
        dtUnidad.DataSource = _unitsBinding;
        dtStatus.DataSource = _statusBinding;
        dtMoneda.DataSource = _currencyBinding;
        dtCategoria.DataSource = _categoryBinding;
        _gridFilterU = new GridFilter<UnitDto>(dtUnidad, _unitsBinding);
        _gridFilterS = new GridFilter<InventaryStatusDto>(dtStatus, _statusBinding);
        _gridFilterC = new GridFilter<CurrencyDto>(dtMoneda, _currencyBinding);
        _gridFilterCa = new GridFilter<CategoryDto>(dtCategoria, _categoryBinding);

    }

    private async Task CargarUnidadesAsync()
    {
        var result = await _unitService.GetUnits();

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Message);
            return;
        }

        _unitsBinding.DataSource = result.Data;
        _gridFilterU.SetData(result.Data);
        dtUnidad = _gridFilterU.BuildFilterColumns();
    }
    private async Task CargarStatusAsync()
    {
        var result = await _statusService.GetInventaryStatus();

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Message);
            return;
        }

        _statusBinding.DataSource = result.Data;
        _gridFilterS.SetData(result.Data);
        dtStatus = _gridFilterS.BuildFilterColumns();
    }
    private async Task CargarCategoriasAsync()
    {
        var result = await _categoryService.GetCategory();

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Message);
            return;
        }

        _categoryBinding.DataSource = result.Data;
        _gridFilterCa.SetData(result.Data);
        dtCategoria = _gridFilterCa.BuildFilterColumns();
    }
    private async Task CargarMonedaAsync()
    {
        var result = await _currencyService.GetCurrency();

        if (!result.IsSuccess)
        {
            MessageBox.Show(result.Message);
            return;
        }

        _currencyBinding.DataSource = result.Data;
        _gridFilterC.SetData(result.Data);
        dtMoneda = _gridFilterC.BuildFilterColumns();
    }

    private async void btnActualizarS_Click(object sender, EventArgs e)
    {
        await LoaderManager.Run(dtStatus, async () => await CargarStatusAsync(), "Obteniendo Estatus");
    }

    private async void button8_Click(object sender, EventArgs e)
    {
        await LoaderManager.Run(dtUnidad, async () => await CargarUnidadesAsync(), "Obteniendo Unidades");
    }

    private async void button13_Click(object sender, EventArgs e)
    {
        await LoaderManager.Run(dtMoneda, async () => await CargarMonedaAsync(), "Obteniendo Monedas");
    }

    private async void btnActualizarC_Click(object sender, EventArgs e)
    {
        await LoaderManager.Run(dtCategoria, async () => await CargarCategoriasAsync(), "Obteniendo Categorías");
    }



    private void button10_Click(object sender, EventArgs e)
    {


    }
    // Status
    private void dtStatus_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (dtStatus.CurrentRow == null)
                return;

            var status = dtStatus.CurrentRow.DataBoundItem as InventaryStatusDto;

            if (status == null)
                return;

            selectedStatus = status;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void btnNuevoS_Click(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNewStatus>();
        if (form.ResponseForm) await LoaderManager.Run(dtStatus, async () => await CargarStatusAsync(), "Obteniendo Status");
    }

    private async void btnEditarS_Click(object sender, EventArgs e)
    {
        if (selectedStatus is null) return;
        var form = _dialogFormService.ShowDialog<FrmNewStatus>(frm =>
        {
            frm.SetInventaryStatus(selectedStatus);
        });
        if (form.ResponseForm) await LoaderManager.Run(dtStatus, async () => await CargarStatusAsync(), "Obteniendo Status");
    }
    // Categoria
    private void dtCategoria_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (dtCategoria.CurrentRow == null)
                return;

            var cate = dtCategoria.CurrentRow.DataBoundItem as CategoryDto;

            if (cate == null)
                return;

            selectedCategory = cate;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void btnNuevoCa_Click(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNewCategory>();
        if (form.ResponseForm) await LoaderManager.Run(dtUnidad, async () => await CargarCategoriasAsync(), "Obteniendo Categorías");
    }

    private async void btnEditarC_Click(object sender, EventArgs e)
    {
        if (selectedCategory is null) return;
        var form = _dialogFormService.ShowDialog<FrmNewCategory>(frm =>
        {
            frm.SetCategory(selectedCategory);
        });
        if (form.ResponseForm) await LoaderManager.Run(dtUnidad, async () => await CargarCategoriasAsync(), "Obteniendo Categorías");
    }
    // Unidad
    private void dtUnidad_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (dtUnidad.CurrentRow == null)
                return;

            var curr = dtUnidad.CurrentRow.DataBoundItem as UnitDto;

            if (curr == null)
                return;

            selectedUnit = curr;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void btnNuevoU_Click(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNewUnit>();
        if (form.ResponseForm) await LoaderManager.Run(dtUnidad, async () => await CargarUnidadesAsync(), "Obteniendo Unidades");
    }

    private async void button9_Click(object sender, EventArgs e)
    {
        if (selectedUnit is null) return;
        var form = _dialogFormService.ShowDialog<FrmNewUnit>(frm =>
        {
            frm.SetUnit(selectedUnit);
        });
        if (form.ResponseForm) await LoaderManager.Run(dtUnidad, async () => await CargarUnidadesAsync(), "Obteniendo Unidades");
    }
    //  Moneda
    private void dtMoneda_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            if (dtMoneda.CurrentRow == null)
                return;

            var mone = dtMoneda.CurrentRow.DataBoundItem as CurrencyDto;

            if (mone == null)
                return;

            selectedCurrency = mone;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }



    private async void btnNuevoM_Click(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNewCurrency>();
        if (form.ResponseForm) await LoaderManager.Run(dtMoneda, async () => await CargarMonedaAsync(), "Obteniendo Monedas");
    }

    private async void button11_Click(object sender, EventArgs e)
    {
        if (selectedCurrency is null) return;
        var form = _dialogFormService.ShowDialog<FrmNewCurrency>(frm =>
        {
            frm.SetCurrency(selectedCurrency);
        });
        if (form.ResponseForm) await LoaderManager.Run(dtMoneda, async () => await CargarMonedaAsync(), "Obteniendo Monedas");
    }

    private async void FrmCatalogos_Load(object sender, EventArgs e)
    {
        await LoaderManager.Run(dtMoneda, async () => await CargarMonedaAsync(), "Obteniendo Monedas");
        await LoaderManager.Run(dtUnidad, async () => await CargarUnidadesAsync(), "Obteniendo Unidades");
        await LoaderManager.Run(dtUnidad, async () => await CargarCategoriasAsync(), "Obteniendo Categorías");
        await LoaderManager.Run(dtStatus, async () => await CargarStatusAsync(), "Obteniendo Status");
    }
}
