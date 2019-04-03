﻿using AOI.Core.Interfaces;
using Splat;
using System;
using System.Collections.Generic;

namespace AOI.Core.OperationManager
{
    public sealed class OperationTextResolver : ICommandTextResolver, IEnableLogger
    {
        private readonly IOperationInvoker _operationInvoker;

        public OperationTextResolver(IOperationInvoker operationInvoker)
        {
            _operationInvoker = operationInvoker;
        }

        void ICommandTextResolver.Resolve(string operationText)
        {
            operationText = operationText.Trim();
            var texts = operationText.Split(' ');
            if (texts.Length == 0)
            {
                this.Log().Info("没有可用的命令");
                return;
            }
            _operationInvoker.InvokeOperation(texts[0]);
        }
    }
}