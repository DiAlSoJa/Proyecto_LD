using LD.Contracts.Enums;
using LD.Forms.Views.Dialogs;
using LD.Forms.Views.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services.FormServices
{
    public class DialogMessageService
    {
        private readonly IServiceProvider _serviceProvider;
        public DialogMessageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show(string message,DialogMessageEnum type = DialogMessageEnum.Info)
        {
            Form dialog = type switch
            {
                DialogMessageEnum.Success => _serviceProvider.GetRequiredService<FrmSuccess>(),
                DialogMessageEnum.Error => _serviceProvider.GetRequiredService<FrmError>(),
                DialogMessageEnum.Warning => _serviceProvider.GetRequiredService<FrmWarning>(),
                DialogMessageEnum.Info => _serviceProvider.GetRequiredService<FrmInfo>(),
                //DialogType.Confirm => _serviceProvider.GetRequiredService<FrmConfirm>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
            if (dialog is IBaseMessageDialog baseDialog)
            {
                baseDialog.SetMessage(message);
            }

            dialog.ShowDialog();
        }

    }
}
