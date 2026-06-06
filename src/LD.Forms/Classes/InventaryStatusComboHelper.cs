using LD.Contracts.InventaryStatus;

namespace LD.Forms.Classes
{
    public static class InventaryStatusComboHelper
    {
        public static ComboBox CreateComboBox()
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
        }

        public static void LoadOptions(
            ComboBox combo,
            IEnumerable<InventaryStatusDto>? statuses,
            string? selectedValue = null)
        {
            combo.BeginUpdate();
            try
            {
                var currentValue = GetValue(combo) ?? selectedValue;

                combo.Items.Clear();

                foreach (var option in BuildOptions(statuses))
                {
                    combo.Items.Add(option);
                }

                AddFallbackOption(combo, selectedValue);
                AddFallbackOption(combo, currentValue);
                SetValue(combo, currentValue);
            }
            finally
            {
                combo.EndUpdate();
            }
        }

        public static void SetValue(ComboBox combo, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                combo.SelectedIndex = -1;
                combo.Text = string.Empty;
                return;
            }

            var trimmedValue = value.Trim();
            var match = combo.Items
                .Cast<object>()
                .OfType<InventaryStatusOption>()
                .FirstOrDefault(x => x.Matches(trimmedValue));

            if (match is not null)
            {
                combo.SelectedItem = match;
                combo.Text = match.DisplayText;
                return;
            }

            combo.SelectedIndex = -1;
            combo.Text = trimmedValue;
        }

        public static string? GetValue(ComboBox combo)
        {
            if (combo.SelectedItem is InventaryStatusOption selected)
            {
                return selected.StatusId;
            }

            var text = combo.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var match = combo.Items
                .Cast<object>()
                .OfType<InventaryStatusOption>()
                .FirstOrDefault(x => x.Matches(text));

            return match?.StatusId ?? text;
        }

        private static IEnumerable<InventaryStatusOption> BuildOptions(IEnumerable<InventaryStatusDto>? statuses)
        {
            return (statuses ?? Enumerable.Empty<InventaryStatusDto>())
                .Where(x => !string.IsNullOrWhiteSpace(x.StatusId))
                .GroupBy(x => x.StatusId.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(x => x.First())
                .OrderBy(x => x.StatusId, StringComparer.OrdinalIgnoreCase)
                .Select(x => new InventaryStatusOption(
                    x.StatusId.Trim(),
                    string.IsNullOrWhiteSpace(x.Descripcion) ? null : x.Descripcion.Trim(),
                    x.Disponible));
        }

        private static void AddFallbackOption(ComboBox combo, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var trimmedValue = value.Trim();
            var exists = combo.Items
                .Cast<object>()
                .OfType<InventaryStatusOption>()
                .Any(x => x.Matches(trimmedValue));

            if (exists)
            {
                return;
            }

            combo.Items.Add(new InventaryStatusOption(trimmedValue, null, false));
        }

        private sealed class InventaryStatusOption
        {
            public InventaryStatusOption(string statusId, string? description, bool disponible)
            {
                StatusId = statusId;
                Description = description;
                Disponible = disponible;
            }

            public string StatusId { get; }
            public string? Description { get; }
            public bool Disponible { get; }

            public string DisplayText =>
                string.IsNullOrWhiteSpace(Description) ||
                string.Equals(Description, StatusId, StringComparison.OrdinalIgnoreCase)
                    ? StatusId
                    : $"{StatusId} - {Description}";

            public bool Matches(string value)
            {
                return string.Equals(StatusId, value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Description, value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(DisplayText, value, StringComparison.OrdinalIgnoreCase);
            }

            public override string ToString() => DisplayText;
        }
    }
}
