using AOI.Core.Interfaces;
using AOI.Core.OperationManager.Interfaces;
using System;

namespace AOI.Core.OperationManager.Operations
{
    public sealed class GetHelp : IOperation
    {
        private readonly IOperationBuilderDictionary _operationBuilderDictionary;

        private readonly IOutput _output;

        public GetHelp(IOperationBuilderDictionary operationBuilderDictionary, IOutput output)
        {
            _operationBuilderDictionary = operationBuilderDictionary;
            _output = output;
        }

        void IOperation.Make()
        {
            foreach(var operationBuilder in _operationBuilderDictionary.Operations.Values)
            {
                _output.Output(operationBuilder.Name);
            }
        }

        void IOperation.Unmake()
        {
            throw new NotImplementedException();
        }
    }
}
