# Copilot Instructions — LD.Forms (Legacy WinForms Client)

## Project Description
Legacy Windows Forms desktop client (net10.0-windows). **Not the active desktop client** — new desktop features go in `LD.FormsX` (WPF). This project is maintained only for bug fixes when specifically requested.

## Architectural Constraints

- Uses MVP (Model-View-Presenter) pattern, not MVVM.
- Views are `Form` classes (`FrmLogin`, `FrmClientes`, etc.).
- Presenters in `Presenters/` contain presentation logic.
- Interfaces in `Views/Interfaces/` define view contracts.
- All API calls through `LD.Client` services.
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`.

## Pattern

```csharp
// Interface
public interface IFooView
{
    string FooName { get; }
    void ShowError(string message);
    void RefreshList(List<FooDto> items);
}

// Presenter
public class FooPresenter
{
    private readonly IFooView _view;
    private readonly FooService _fooService;

    public FooPresenter(IFooView view, FooService fooService)
    {
        _view = view;
        _fooService = fooService;
    }

    public async Task LoadAsync()
    {
        var result = await _fooService.GetFooAsync();
        if (!result.IsSuccess)
        {
            _view.ShowError(result.Message);
            return;
        }
        _view.RefreshList(result.Data ?? []);
    }
}

// Form (View)
public partial class FrmFoo : Form, IFooView
{
    private readonly FooPresenter _presenter;
    // Implement IFooView members...
}
```

## Development Policy

This project is **low priority**. Do not:
- Add new major features — use `LD.FormsX` instead
- Refactor to MVVM — MVP is accepted for this legacy project
- Optimize or modernize — only fix specific bugs when asked

## What to Avoid

- Do not add HTTP calls without going through `LD.Client`
- Do not reference `LD.Domain`, `LD.Application`, or `LD.Infrastructure`
- Do not create new screens for features that exist in `LD.FormsX`
